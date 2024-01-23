using System.Collections;
using UnityEngine;
using PathFinding;
using DG.Tweening;

namespace TurnSystem.States
{
    public class EnemyTurnState : State
    {

        [SerializeField] public EnemyMovement enemyMovement;

        public EnemyTurnState(global::TurnSystem.TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override IEnumerator Start()
        {
            TurnSystem.titleSystem.SetTitle(TurnSystem.enemyTitle);
            TurnSystem.playerUI.SetActive(false);

            TurnSystem.cameraController.enabled = false;

            yield return new WaitForSeconds(1f);
            
            TurnSystem.titleSystem.RemoveTitle(TurnSystem.enemyTitle);

            enemyMovement = TurnSystem.enemyMovement;
            enemyMovement.startCombat = true;
        }
        
        public override IEnumerator CheckState()
        {
            yield return new WaitForSeconds(0.5f);
            
            for (int i = 0; i < TurnSystem.allyTeam.Count; i++)
            {
                if (TurnSystem.allyTeam[i].isDead != true)
                {
                    TurnSystem.SetState(new PlayerTurnState(TurnSystem));
                }
            }
        }
    }
}
