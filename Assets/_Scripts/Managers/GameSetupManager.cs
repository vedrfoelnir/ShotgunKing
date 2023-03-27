using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupManager : Singleton<GameSetupManager>
{

    // Dependencies
    private GameUnitManager unitManager;

    const float scalingFactor = 3.0f;

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

    private void Start()
    {
        unitManager = GameUnitManager.Instance;
    }

    public void CameraSetup()
    {
        mainCamera.transform.position = new Vector3(scalingFactor * 3.5f, 40f, scalingFactor * 2.73f);
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
                var plt = Instantiate(platFormPrefab, new Vector3(scalingFactor * file, 0, scalingFactor * rank), Quaternion.identity);

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

    public void SpawnPlayer(int rank, int file)
{
    player = Instantiate(playerModel, new Vector3((file-1) * scalingFactor, 1.5f, (rank-1) * scalingFactor), Quaternion.identity);
    unitManager.AddUnit(player, rank, file);
}

public void SetupLevelFromFEN(string fenstr)
{
    if (string.IsNullOrEmpty(fenstr))
    {
        throw new ArgumentException("FEN string input is invalid.");
    }

    Dictionary<char, (GameObject prefab, int id)> pieceMap = new Dictionary<char, (GameObject, int)>()
    {
        {'p', (enemyPawn, 1)},
        {'n', (enemyKnight, 3)},
        {'b', (enemyBishop, 3)},
        {'r', (enemyRook, 5)},
        {'q', (enemyQueen, 9)}
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
            
            var (prefab, value) = pieceMap[input];
            GameObject unit = Instantiate(prefab, new Vector3(file * scalingFactor, 0, (7 - rank) * scalingFactor), Quaternion.identity);
            unitManager.AddUnit(unit, (7 - rank), file);
            file++;
        }
        else
        {
            throw new ArgumentException("FEN string input is invalid.");
        }
    }
}


}