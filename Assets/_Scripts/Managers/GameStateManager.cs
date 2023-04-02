using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameStateManager : Singleton<GameStateManager>
{

    // Export
    //public static event Action<GameState> OnBeforeStateChanged;
    //public static event Action<GameState> OnAfterStateChanged;

    // Dependencies


    //Variables
    public GameState State { get; private set; }
    private int currentLevelIndex = 0;
    private static readonly List<string> Levels = new List<string> {
        "2p1p1p/8/8/8/8/8/8/8/",
        "8/4pprr/8/8/8/8/8/8/",
        "rnbqqbnr/pppppppp/8/8/8/8/8/8/",
        "rnbqkbnr/8/8/8/8/8/8/8/"
    };

    void Start()
    {
        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState)
    {
        //OnBeforeStateChanged(newState);

        State = newState;

        switch (newState)
        {
            case GameState.Starting:
                HandleStart();
                break;
            case GameState.InitPlayer:
                HandlePlayerInit();
                break;
            case GameState.InitOpponent:
                HandleOpponentInit();
                break;
            case GameState.TurnPlayer:
                StartCoroutine(HandlePlayerTurn());
                break;
            case GameState.TurnOpponent:
                StartCoroutine(HandleOpponentTurn());
                break;
            case GameState.Win:
                HandleWin();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State), State, null);
        }

        //OnAfterStateChanged(newState);

        Debug.Log($"New State: {newState}");
    }

    private void HandleStart()
    {
        GameSetupManager.Instance.CameraSetup();
        GameSetupManager.Instance.CameraSetup();
        GameSetupManager.Instance.CreateBoard();
        ChangeState(GameState.InitPlayer);
    }

    private void HandlePlayerInit()
    {
        GameSetupManager.Instance.SpawnPlayer(2, 2);
        ChangeState(GameState.InitOpponent);
    }

    private void HandleOpponentInit()
    {
        string level = Levels[currentLevelIndex];
        GameSetupManager.Instance.SetupLevelFromFEN(level);
        currentLevelIndex = (currentLevelIndex + 1) % Levels.Count;
        ChangeState(GameState.TurnPlayer);
    }

    private IEnumerator HandlePlayerTurn()
    {

        Debug.Log("Player Action");
        yield return StartCoroutine(PlayerController.Instance.SetActiveAndWaitForInputToMove());
        yield return null;
        yield return StartCoroutine(PlayerController.Instance.WaitForInputToShoot());
        yield return new WaitForSeconds(0.5f); // Yielding null to allow the next frame to start the opponent's turn
        ChangeState(GameState.TurnOpponent);
    }

    private IEnumerator HandleOpponentTurn()
    {
        yield return new WaitForSeconds(0.5f);
        List<GameObject> enemies = new List<GameObject>(GameUnitManager.Instance.enemies);
        foreach (GameObject enemy in enemies)
        {
            EnemyBehaviour enemyBehavior = enemy.GetComponent<EnemyBehaviour>();
            if (enemyBehavior == null)
            {
                Debug.Log("Couldn't find behaviour on: " + enemy.ToString());
                continue;
            }
            yield return StartCoroutine(enemyBehavior.MoveAction());

            if (enemy == null)
            {
                GameUnitManager.Instance.enemies.Remove(enemy);
            }
        }

        yield return new WaitForSeconds(0.5f); // Yielding null to allow the next frame to start the player's turn

        Debug.Log("Enemy Finished");

        yield return StartCoroutine(EvaluateGameState());
    }

    private IEnumerator EvaluateGameState()
    {
        yield return new WaitForEndOfFrame();
        if (PlayerController.Instance.HP < 1)
        {
            ChangeState(GameState.Lose);
        }
        else if (!GameUnitManager.Instance.HasEnemies())
        {
            Debug.Log("HP left: " + PlayerController.Instance.HP);
            ChangeState(GameState.Win);
        }
        else
        {
            ChangeState(GameState.TurnPlayer);
        }
    }

    private void HandleWin()
    {
        PlayerController.Instance.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void HandleLose()
    {
        PlayerController.Instance.gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

}

[Serializable]
public enum GameState
{
    Starting = 0,
    InitPlayer = 1,
    InitOpponent = 2,
    TurnPlayer = 3,
    TurnOpponent = 4,
    Win = 5,
    Lose = 6,
}