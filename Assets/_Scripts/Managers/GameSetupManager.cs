using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupManager : SingletonPersistent<GameSetupManager>
{

    // Dependencies
    private GameUnitManager unitManager;

    const float gameSize = 3.0f;

    [SerializeField]
    private GameObject platFormPrefab;
    [SerializeField]
    private GameObject playerModel;
    [SerializeField]
    private GameObject enemyPawn;
    [SerializeField]
    private GameObject enemyKnight;
    [SerializeField]
    private GameObject enemyBishop;
    [SerializeField]
    private GameObject enemyRook;
    [SerializeField]
    private GameObject enemyQueen;

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();

    public Camera mainCamera;
    private bool isBlackTile;

    public int[,] board = new int[8, 8];

    private void Start()
    {
        unitManager = GameUnitManager.Instance;
    }

    public void CameraSetup()
    {
        mainCamera.transform.position = new Vector3(gameSize * 3.5f, 40f, gameSize * 2.73f);
        mainCamera.transform.rotation = new Quaternion(0.688228011f, 0, 0, 0.725494504f);
        mainCamera.fieldOfView = 33.7f;
    }

    public void CreateBoard()
    {

        for (int rank = 0; rank < 8; rank++)
        {
            isBlackTile = (rank % 2 == 0) ? false : true;

            for (int file = 0; file < 8; file++)
            {
                var plt = Instantiate(platFormPrefab, new Vector3(gameSize * file, 0, gameSize * rank), Quaternion.identity);

                if (!isBlackTile)
                {
                    plt.GetComponent<Renderer>().material.color = Color.white;
                }
                else
                {
                    plt.GetComponent<Renderer>().material.color = Color.black;
                }

                isBlackTile = !isBlackTile;
            }
        }
    }

    public void SpawnPlayer()
    {
        player = Instantiate(playerModel, new Vector3(4 * gameSize, 1.5f, 3 * gameSize), Quaternion.identity);
        unitManager.player = player;
    }

    public void SetupLevelFromFEN(string fenstr)
    {
        if (string.IsNullOrEmpty(fenstr))
        {
            throw new ArgumentException("FEN string input is invalid.");
        }

        Dictionary<char, (GameObject prefab, int id)> pieceMap = new Dictionary<char, (GameObject, int)>()
        {
            {'p', (enemyPawn, 2)},
            {'n', (enemyKnight, 3)},
            {'b', (enemyBishop, 4)},
            {'r', (enemyRook, 5)},
            {'q', (enemyQueen, 6)}
        };

        int rank = 0;
        int file = 0;

        foreach (char input in fenstr)
        {
            if (rank >= 8)
            {
                break;
            }

            if (input == '/')
            {
                rank++;
                file = 0;
            }
            else if (char.IsDigit(input))
            {
                file += (input - '0');
            }
            else if (pieceMap.ContainsKey(input))
            {
                var (prefab, id) = pieceMap[input];
                GameObject unit = Instantiate(prefab, new Vector3(file * gameSize, 0, (7 - rank) * gameSize), Quaternion.identity);
                board[rank, file] = id;
                file++;
                unitManager.AddUnit(unit);
            }
            else
            {
                throw new ArgumentException("FEN string input is invalid.");
            }
        }
    }

}