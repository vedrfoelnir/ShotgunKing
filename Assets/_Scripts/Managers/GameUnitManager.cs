using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUnitManager : SingletonPersistent<GameUnitManager>
{
    [HideInInspector]
    public GameObject player { get; set; }
    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> allUnits = new List<GameObject>();

    public void AddUnit(GameObject unit)
    {
        allUnits.Add(unit);
        if (unit.CompareTag("Player"))
        {
            player = unit;
        }
        else if (unit.CompareTag("Enemy"))
        {
            enemies.Add(unit);
        }
    }

    public void RemoveUnit(GameObject unit)
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
    }

    public bool HasEnemies()
    {
        return enemies.Count > 0;
    }

}
