using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupManager : Singleton<GameSetupManager>
{

    public const float scalingFactor = 3.0f;

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

    public Camera mainCamera;
    private bool isBlackTile;

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
                var plt = Instantiate(platFormPrefab, new Vector3(scalingFactor * file, -0.5f, scalingFactor * rank), Quaternion.identity);

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

    public GameObject SpawnPlayer(int rank, int file)
    {
        GameObject player = Instantiate(playerModel, new Vector3((file-1) * scalingFactor, 0.0f, (rank-1) * scalingFactor), Quaternion.Euler(-90, 0, 0));
        GameUnitManager.Instance.AddUnit(player, rank, file);
        return player;
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

        int rank = 1;
        int file = 1;

        foreach (char input in fenstr)
        {
            if (rank > 8)
            {
                break;
            }

            if (input == '/')
            {
                rank++;
                file = 1;
            }
            else if (char.IsDigit(input))
            {
                file += (input - '0');
            }
            else if (pieceMap.ContainsKey(input))
            {
            
                var (prefab, value) = pieceMap[input];
                GameObject unit = Instantiate(prefab, new Vector3((file-1) * scalingFactor, 0, (8 - rank) * scalingFactor), Quaternion.Euler(-90,0,0));
                GameUnitManager.Instance.AddUnit(unit, (9 - rank), file);
                file++;
            }
            else
            {
                throw new ArgumentException("FEN string input is invalid.");
            }
        }
    }

    internal float GetScaling()
    {
        Debug.Log("Scaling returned: " + scalingFactor);
        return scalingFactor;
    }
}