using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameStateManager : Singleton<GameStateManager>
{

    // Export
    //public static event Action<GameState> OnBeforeStateChanged;
    //public static event Action<GameState> OnAfterStateChanged;

    // Dependencies
    public GameSetupManager gameSetup;
    public GameUnitManager unitManager;

    //Variables
    public GameState State { get; private set; }
    private int currentLevelIndex = 0;
    private static readonly List<string> Levels = new List<string> {
        "rnbqqbnr/pppppppp/8/8/8/8/8/8/",
        "rnbqkbnr/8/8/8/8/8/8/8/"
    };

    void Start() => ChangeState(GameState.Starting);

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
        gameSetup.CameraSetup();
        gameSetup.CreateBoard();
        ChangeState(GameState.InitPlayer);
    }

    private void HandlePlayerInit()
    {
        gameSetup.SpawnPlayer();
        ChangeState(GameState.InitOpponent);

    }

    private void HandleOpponentInit()
    {
        string level = Levels[currentLevelIndex];
        gameSetup.SetupLevelFromFEN(level);
        currentLevelIndex = (currentLevelIndex + 1) % Levels.Count;
    }


    private void HandlePlayerTurn()
    {
        throw new NotImplementedException();
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

