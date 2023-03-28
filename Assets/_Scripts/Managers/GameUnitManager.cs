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
}