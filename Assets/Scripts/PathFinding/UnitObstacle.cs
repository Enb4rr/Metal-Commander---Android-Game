using System.Collections;
using System.Collections.Generic;
using CombatSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitObstacle : MonoBehaviour
{
    [SerializeField] private TurnSystem.TurnSystem turnSystem;
    [SerializeField] public List<Unit> units;
    [SerializeField] public Tilemap obstacleTilemap;
    [SerializeField] public Tile tile;

    public void UpdateObstacleMapForAllies()
    {
        if (units != null)
        {
            foreach (var t in units)
            {
                var position = t.transform.position;
                var intPosition = Vector3Int.FloorToInt(position);
                obstacleTilemap.SetTile(intPosition, null);
            }

            units.Clear();
        }
        
        foreach (var t in turnSystem.enemyTeam)
        {
            if (t.hitPoints > 0)
            {
                units?.Add(t);
            }
        }


        if (units != null)
        {
            foreach (var t in units)
            {
                var position = t.transform.position;
                var intPosition = Vector3Int.FloorToInt(position);
                obstacleTilemap.SetTile(intPosition, tile);
            }
        }
    }
    
    public void UpdateObstacleMapForEnemies()
    {
        if (units != null)
        {
            foreach (var t in units)
            {
                var position = t.transform.position;
                var intPosition = Vector3Int.FloorToInt(position);
                obstacleTilemap.SetTile(intPosition, null);
            }
            
            units.Clear();
        }
        
        foreach (var t in turnSystem.allyTeam)
        {
            if (t.hitPoints > 0)
            {
                units?.Add(t);
            }
        }

        if (units != null)
        {
            foreach (var t in units)
            {
                var position = t.transform.position;
                var intPosition = Vector3Int.FloorToInt(position);
                obstacleTilemap.SetTile(intPosition, tile);
            }
        }
    }

    public void ClearObstacleMap()
    {
        if (units != null)
        {
            foreach (var t in units)
            {
                var position = t.transform.position;
                var intPosition = Vector3Int.FloorToInt(position);
                obstacleTilemap.SetTile(intPosition, null);
            }
            
            units.Clear();
        }
    }
}
