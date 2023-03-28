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
                HandlePlayerTurn();
                break;
            case GameState.TurnOpponent:
                HandleOpponenTurn();
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
        GameSetupManager.Instance.SpawnPlayer(2, 4);
        ChangeState(GameState.InitOpponent);
    }

    private void HandleOpponentInit()
    {
        string level = Levels[currentLevelIndex];
        GameSetupManager.Instance.SetupLevelFromFEN(level);
        currentLevelIndex = (currentLevelIndex + 1) % Levels.Count;
        ChangeState(GameState.TurnPlayer);
    }

    private void HandlePlayerTurn()
    {
        Debug.Log("Player Action");
        StartCoroutine(PlayerMovement.Instance.TurnGreenAndWaitForInputToMove());
    }

    private void HandleOpponenTurn()
    {
        throw new NotImplementedException();
    }

    private void HandleWin()
    {
        throw new NotImplementedException();
    }

    private void HandleLose()
    {
        throw new NotImplementedException();
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

