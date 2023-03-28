using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUnitManager : Singleton<GameUnitManager>
{
    [HideInInspector]
    public GameObject player { get; set; }
    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> allUnits = new List<GameObject>();

    public Dictionary<(int, int), GameObject> gameUnits = new Dictionary<(int, int), GameObject>();

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


    public void UpdateUnit(int currentRank, int currentFile, int newRank, int newFile)
    {
        if (gameUnits.TryGetValue((currentRank, currentFile), out GameObject unit))
        {
            gameUnits.Remove((currentRank, currentFile));
            gameUnits.Add((newRank, newFile), unit);
        }
        else
        {
            Debug.LogError("Unit not found at position: " + currentRank + ", " + currentFile);
        }
    }
}
