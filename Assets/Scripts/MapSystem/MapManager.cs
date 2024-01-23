using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace MapSystem
{
    public class MapManager : MonoBehaviour
    {
        //Spawning Units.
        
        [SerializeField] public Tilemap allySpawners;
        [HideInInspector] public List<Vector3> allySpawns;
        [SerializeField] public List<Unit> unitPrefab;
        [SerializeField] public List<Unit> enemyPrefab;


        [SerializeField] public List<Vector3> enemySpawn;
        [SerializeField] public List<int> unitType;
        [SerializeField] public List<int> unitIA;

        //TurnSystem Data

        [SerializeField] private TurnSystem.TurnSystem turnSystem;
        

        private void Start()
        { 
            //allySpawners = GetComponent<Tilemap>();
            allySpawns = new List<Vector3>();
        }

        public void GetSpawners(Tilemap spawn, List<Vector3> spawners)
        {
            for (int n = spawn.cellBounds.xMin; n < spawn.cellBounds.xMax; n++)
            {
                for (int p = spawn.cellBounds.yMin; p < spawn.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int) spawn.transform.position.y));
                    Vector3 place = spawn.CellToWorld(localPlace);
                    if (spawn.HasTile(localPlace))
                    {
                        //Tile at "place"
                        spawners.Add(place);
                    }
                    else
                    {
                        //No tile at "place"
                    }
                }
            }
        }

        public void SpawnUnit()
        {
            GetSpawners(allySpawners, allySpawns);
            for(var i = 0; i < allySpawns.Count; i++)
            {
                var newAllyUnit = Instantiate(unitPrefab[i], allySpawns[i], Quaternion.identity);
                turnSystem.allyTeam.Add(newAllyUnit);
            }
        }

        public void SpawnEnemies()
        {
            for (var i = 0; i < enemySpawn.Count; i++)
            {
                int enemyClass = 0;

                if (unitType[i] == 0)
                {
                    enemyClass = unitType[i];
                }
                else if (unitType[i] == 1)
                {
                    enemyClass = unitType[i];
                }
                else if (unitType[i] == 2)
                {
                    enemyClass = unitType[i];
                }

                var newEnemyUnit = Instantiate(enemyPrefab[enemyClass], enemySpawn[i], Quaternion.identity);
                turnSystem.enemyTeam.Add(newEnemyUnit);

                if (unitIA[i] == 0)
                {
                    turnSystem.enemyTeam[i].aggressive = true;
                }
                else if (unitIA[i] == 1)
                {
                    turnSystem.enemyTeam[i].inRange = true;
                }
                else if (unitIA[i] == 2)
                {
                    turnSystem.enemyTeam[i].passive = true;
                }
            }
        }
    }
}
