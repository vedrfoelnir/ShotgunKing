using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUnitManager : Singleton<GameUnitManager>
{
    // Export
    [SerializeField]
    public Dictionary<(int, int), GameObject> gameUnits = new Dictionary<(int, int), GameObject>();
    [HideInInspector]
    public GameObject player { get; set; }
    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> allUnits = new List<GameObject>();

    // Import
    private float scalingFactor;
    
    
    private void Start()
    {
        scalingFactor = GameSetupManager.Instance.GetScaling();
    }

    public void AddUnit(GameObject unit, int rank, int file)
    {
        allUnits.Add(unit);
        if (unit.CompareTag("Player"))
        {
            player = unit;
            Debug.Log("Player is: " + unit);
        }
        else if (unit.CompareTag("Enemy"))
        {
            enemies.Add(unit);
        }

        if (!gameUnits.ContainsKey((rank, file)))
        {
            gameUnits.Add((rank, file), unit);
            Debug.Log("Added Unit " + unit.ToString() + " at " + rank + ", " + file);
        }
        else
        {
            Debug.LogError("Duplicate unit position detected at: " + rank + ", " + file + "; GameUnit: " + gameUnits[(rank, file)]);
        }
    }

    public void RemoveUnit(GameObject unit, int rank, int file)
    {
        allUnits.Remove(unit);

        if (unit.CompareTag("Player"))
        {
            player = null;
        }
        else if (unit.CompareTag("Enemy"))
        {
            enemies.Remove(unit);
        }

        if (gameUnits.ContainsKey((rank, file)))
        {
            gameUnits.Remove((rank, file));
        }
        Destroy(unit);
    }

    public bool HasEnemies()
    {
        return enemies.Count > 0;
    }

    public GameObject IsOccupied(int rank, int file)
    {
        if (gameUnits.ContainsKey((rank, file)))
        {
            return gameUnits[(rank, file)];
        }
        else
        {
            return null;
        }
    }

    public (int, int) GetUnitPosition(GameObject searchedObject)
    {
        foreach (var gameUnit in gameUnits)
        {
            if (gameUnit.Value == searchedObject)
            {
                return gameUnit.Key;
            }
        }

        // If the given unit is not found in the dictionary, return (-1, -1) or throw an exception.
        return (-1, -1);
    }


    public void UpdateUnit(int currentRank, int currentFile, int newRank, int newFile)
    {
        if (gameUnits.TryGetValue((currentRank, currentFile), out GameObject unit))
        {
            if ( IsOccupied(newRank, newFile) == null ) // if Object on Target
            {
                // TODO: What do when something there where you wanna go
            }

            gameUnits.Remove((currentRank, currentFile));
            gameUnits.Add((newRank, newFile), unit);
            unit.transform.position = new Vector3((newFile - 1) * scalingFactor, 0, (newRank - 1) * scalingFactor);
        }
        else
        {
            Debug.LogError("Unit not found at position: " + currentRank + ", " + currentFile);
        }
    }
}