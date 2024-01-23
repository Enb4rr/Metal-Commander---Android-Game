using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using TurnSystem.States;
using UnityEngine.Serialization;
using CombatSystem;
using Menu___UI;

namespace PathFinding
{

    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Pathfinding2D enemyMovement;
        [SerializeField] private UnitObstacle unitObstacle;
        [SerializeField] public List<Unit> enemies;
        [SerializeField] public List<Unit> allies;
        [SerializeField] private Unit currentEnemy;
        [SerializeField] private GameObject currentTarget;
        [SerializeField] private Unit currentPlayer;
        [SerializeField] public TurnSystem.TurnSystem turnSystem;

        [SerializeField] private CombatManager combatManager;

        public bool startCombat;

        [SerializeField] private ButtonBehaviour fader;

        private void Start()
        {
            FindEntities();
        }

        private void Update()
        {
            if (startCombat)
            {
                StartCoroutine(StartEnemy());
            }
        }
        
        private IEnumerator StartEnemy()
        {
            startCombat = false;
            
            foreach (var t in enemies)
            {
                currentEnemy = t;

                if (currentEnemy.hitPoints > 0)
                {
                    enemyMovement = currentEnemy.GetComponent<Pathfinding2D>();
                    
                    unitObstacle.UpdateObstacleMapForEnemies();
                    var unitPos = unitObstacle.obstacleTilemap.WorldToCell(currentEnemy.transform.position);
                    if (unitObstacle.obstacleTilemap.GetTile(unitPos) != null)
                    {
                        unitObstacle.obstacleTilemap.SetTile(unitPos, null);
                    }
                    enemyMovement.UpdateGrid();
                    
                    SearchForAllies();

                    if (currentEnemy.foundRival)
                    {
                        yield return new WaitForSeconds(7);
                    }
                    else
                    {
                        yield return new WaitForSeconds(2);
                    }
                }
            }
            
            turnSystem.SetState(new PlayerTurnState(turnSystem));
        }
        
        private void FindEntities()
        {
            enemies = turnSystem.enemyTeam;
            allies = turnSystem.allyTeam;
        }
        
        private void SearchForAllies()
        {
            float nearest = 10000;
            
            foreach (var t in allies)
            {
                var actualMinDistance = Vector3.Distance(currentEnemy.transform.position, t.transform.position);

                if (actualMinDistance < nearest && t.hitPoints > 0)
                {
                    nearest = actualMinDistance;
                    currentTarget = t.gameObject;
                    currentPlayer = currentTarget.GetComponent<Unit>();
                    
                }
            }

            if (currentTarget != null && currentEnemy.inRange)
            {
                enemyMovement.FindPath(currentEnemy.transform.position, currentTarget.transform.position);
                
                if (enemyMovement.path.Count <= currentEnemy.movement)
                {
                    currentEnemy.foundRival = true;
                    
                    StartCoroutine(MoveInRange(enemyMovement));
                }
            }
            
            if (currentTarget != null && currentEnemy.passive)
            {
                currentEnemy.foundRival = false;
            }
            
            if (currentTarget != null && currentEnemy.aggressive)
            {
                enemyMovement.FindPath(currentEnemy.transform.position, currentTarget.transform.position);
                if (enemyMovement.path.Count <= currentEnemy.movement)
                {
                    currentEnemy.foundRival = true;
                }
                else
                {
                    currentEnemy.foundRival = false;
                }
                
                StartCoroutine(MoveAggressive(enemyMovement));
            }
        }
        
        private IEnumerator MoveInRange(Pathfinding2D unitPath)
        {
            if (currentEnemy.className != "Mage")
            {
                if (enemyMovement.path.Count > 1)
                {
                    var hitAlly = Physics2D.Raycast(enemyMovement.path[enemyMovement.path.Count - 2].worldPosition, Vector2.zero, 0);
                    if (hitAlly)
                    {
                        var newCol = unitObstacle.obstacleTilemap.WorldToCell(enemyMovement.path[enemyMovement.path.Count - 2].worldPosition);
                        unitObstacle.obstacleTilemap.SetTile(newCol, unitObstacle.tile);
                        enemyMovement.UpdateGrid();
                    }
                
                    enemyMovement.FindPath(currentEnemy.transform.position, currentTarget.transform.position);
                    unitPath = enemyMovement;
                }

                if (unitPath.path.Count <= currentEnemy.movement)
                {
                    var maxCount = unitPath.path.Count - 1;
                    var path = new Vector3[maxCount];
                    for (var i = 0; i < path.Length; i++)
                    {
                        path[i] = unitPath.path[i].worldPosition - new Vector3(0.5f, 0.5f, 0);
                    }
            
                    var camPath = new Vector3[path.Length];
                    for (var i = 0; i < path.Length; i++)
                    {
                        camPath[i] = path[i] + new Vector3(0, 0, -10);
                    }

                    turnSystem.mainCamera.transform.DOMove(currentEnemy.transform.position + new Vector3(0, 0, -10), 1);
                    yield return new WaitForSeconds(1.1f);
            
                    currentEnemy.transform.DOPath(path, 1, PathType.Linear, PathMode.TopDown2D);
                    turnSystem.mainCamera.transform.DOPath(camPath, 1, PathType.Linear, PathMode.TopDown2D);

                    if (currentEnemy.hitPoints > 0 && currentPlayer.hitPoints > 0)
                    {
                        EnemyCombat();
                    }
                
                    currentEnemy.foundRival = false;
                }
            }
            else if (currentEnemy.className == "Mage")
            {
                if (enemyMovement.path.Count > 2)
                {
                    var hitAlly = Physics2D.Raycast(enemyMovement.path[enemyMovement.path.Count - 3].worldPosition, Vector2.zero, 0);
                    if (hitAlly)
                    {
                        var newCol = unitObstacle.obstacleTilemap.WorldToCell(enemyMovement.path[enemyMovement.path.Count - 3].worldPosition);
                        unitObstacle.obstacleTilemap.SetTile(newCol, unitObstacle.tile);
                        enemyMovement.UpdateGrid();
                    }
                
                    enemyMovement.FindPath(currentEnemy.transform.position, currentTarget.transform.position);
                    unitPath = enemyMovement;
                }
                
                if (enemyMovement.path.Count <= 2)
                {
                    yield break;
                }
                
                if (unitPath.path.Count - 1 <= currentEnemy.movement && enemyMovement.path.Count > 2)
                {
                    var maxCount = unitPath.path.Count - 2;
                    var path = new Vector3[maxCount];
                    for (var i = 0; i < path.Length; i++)
                    {
                        path[i] = unitPath.path[i].worldPosition - new Vector3(0.5f, 0.5f, 0);
                    }
            
                    var camPath = new Vector3[path.Length];
                    for (var i = 0; i < path.Length; i++)
                    {
                        camPath[i] = path[i] + new Vector3(0, 0, -10);
                    }

                    turnSystem.mainCamera.transform.DOMove(currentEnemy.transform.position + new Vector3(0, 0, -10), 1);
                    yield return new WaitForSeconds(1.1f);
            
                    currentEnemy.transform.DOPath(path, 1, PathType.Linear, PathMode.TopDown2D);
                    turnSystem.mainCamera.transform.DOPath(camPath, 1, PathType.Linear, PathMode.TopDown2D);

                    if (currentEnemy.hitPoints > 0 && currentPlayer.hitPoints > 0)
                    {
                        EnemyCombat();
                    }
                
                    currentEnemy.foundRival = false;
                }
            }
        }
        
        private IEnumerator MoveAggressive(Pathfinding2D unitPath)
        {
            if (unitPath.path.Count - 1 <= currentEnemy.movement && currentEnemy.className != "Mage")
            {
                if (enemyMovement.path.Count > 1)
                {
                    var hitAlly = Physics2D.Raycast(enemyMovement.path[enemyMovement.path.Count - 2].worldPosition, Vector2.zero, 0);
                    if (hitAlly)
                    {
                        var newCol = unitObstacle.obstacleTilemap.WorldToCell(enemyMovement.path[enemyMovement.path.Count - 2].worldPosition);
                        unitObstacle.obstacleTilemap.SetTile(newCol, unitObstacle.tile);
                        enemyMovement.UpdateGrid();
                    }
                
                    enemyMovement.FindPath(currentEnemy.transform.position, currentTarget.transform.position);
                    unitPath = enemyMovement;
                }

                if (unitPath.path.Count - 1 <= currentEnemy.movement)
                {
                    var maxCount = unitPath.path.Count - 1;
                    var path = new Vector3[maxCount];
                    for (var i = 0; i < path.Length; i++)
                    {
                        path[i] = unitPath.path[i].worldPosition - new Vector3(0.5f, 0.5f, 0);
                    }

                    var camPath = new Vector3[path.Length];
                    for (var i = 0; i < path.Length; i++)
                    {
                        camPath[i] = path[i] + new Vector3(0, 0, -10);
                    }

                    turnSystem.mainCamera.transform.DOMove(currentEnemy.transform.position + new Vector3(0, 0, -10), 0.8f);
                    yield return new WaitForSeconds(1.1f);
                
                    currentEnemy.transform.DOPath(path, 1, PathType.Linear, PathMode.TopDown2D);
                    turnSystem.mainCamera.transform.DOPath(camPath, 1, PathType.Linear, PathMode.TopDown2D);

                    if (currentEnemy.hitPoints > 0 && currentPlayer.hitPoints > 0)
                    {
                        EnemyCombat();
                    }
                
                    currentEnemy.foundRival = false;
                }
            }
            else if (unitPath.path.Count - 1 <= currentEnemy.movement && currentEnemy.className == "Mage")
            {
                if (enemyMovement.path.Count > 2)
                {
                    var hitAlly = Physics2D.Raycast(enemyMovement.path[enemyMovement.path.Count - 3].worldPosition, Vector2.zero, 0);
                    if (hitAlly)
                    {
                        var newCol = unitObstacle.obstacleTilemap.WorldToCell(enemyMovement.path[enemyMovement.path.Count - 3].worldPosition);
                        unitObstacle.obstacleTilemap.SetTile(newCol, unitObstacle.tile);
                        enemyMovement.UpdateGrid();
                    }
                
                    enemyMovement.FindPath(currentEnemy.transform.position, currentTarget.transform.position);
                    unitPath = enemyMovement;
                }
                
                if (enemyMovement.path.Count <= 2)
                {
                    yield break;
                }

                if (unitPath.path.Count - 1 <= currentEnemy.movement && enemyMovement.path.Count > 2)
                {
                    var maxCount = unitPath.path.Count - 2;
                    var path = new Vector3[maxCount];
                    for (var i = 0; i < path.Length; i++)
                    {
                        path[i] = unitPath.path[i].worldPosition - new Vector3(0.5f, 0.5f, 0);
                    }

                    var camPath = new Vector3[path.Length];
                    for (var i = 0; i < path.Length; i++)
                    {
                        camPath[i] = path[i] + new Vector3(0, 0, -10);
                    }

                    turnSystem.mainCamera.transform.DOMove(currentEnemy.transform.position + new Vector3(0, 0, -10), 0.8f);
                    yield return new WaitForSeconds(1.1f);
                
                    currentEnemy.transform.DOPath(path, 1, PathType.Linear, PathMode.TopDown2D);
                    turnSystem.mainCamera.transform.DOPath(camPath, 1, PathType.Linear, PathMode.TopDown2D);

                    if (currentEnemy.hitPoints > 0 && currentPlayer.hitPoints > 0)
                    {
                        EnemyCombat();
                    }
                
                    currentEnemy.foundRival = false;
                }
            }
            else
            {
                if (enemyMovement.path.Count > 1)
                {
                    var hitAlly = Physics2D.Raycast(enemyMovement.path[enemyMovement.path.Count - 2].worldPosition, Vector2.zero, 0);
                    if (hitAlly)
                    {
                        var newCol = unitObstacle.obstacleTilemap.WorldToCell(enemyMovement.path[enemyMovement.path.Count - 2].worldPosition);
                        unitObstacle.obstacleTilemap.SetTile(newCol, unitObstacle.tile);
                        enemyMovement.UpdateGrid();
                    }
                
                    enemyMovement.FindPath(currentEnemy.transform.position, currentTarget.transform.position);
                    unitPath = enemyMovement;
                }

                var maxCount = (int)currentEnemy.movement;
                var path = new Vector3[maxCount];
                for (var i = 0; i < maxCount; i++)
                {
                    path[i] = unitPath.path[i].worldPosition - new Vector3(0.5f, 0.5f, 0);
                }
                
                var camPath = new Vector3[path.Length];
                for (var i = 0; i < path.Length; i++)
                {
                    camPath[i] = path[i] + new Vector3(0, 0, -10);
                }

                turnSystem.mainCamera.transform.DOMove(currentEnemy.transform.position + new Vector3(0, 0, -10), 0.8f);
                yield return new WaitForSeconds(1);
                
                currentEnemy.transform.DOPath(path, 1, PathType.Linear, PathMode.TopDown2D);
                turnSystem.mainCamera.transform.DOPath(camPath, 1, PathType.Linear, PathMode.TopDown2D);
                
                currentEnemy.foundRival = false;
            }
        }

        private void EnemyCombat()
        {
            currentTarget = null;

            if (currentPlayer != null)
            {
                fader.FadeToCombat();
                
                StartCoroutine(combatManager.MoveToCombat(currentEnemy, currentPlayer));
            }

            currentEnemy.foundRival = false;
        }
    }
}