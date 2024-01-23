using System.Collections;
using UnityEngine;

namespace TurnSystem.States
{
    public class BeginBattleState : State
    {
        public BeginBattleState(global::TurnSystem.TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override IEnumerator Start()
        {
            TurnSystem.mapSystem.SpawnUnit();
            TurnSystem.mapSystem.SpawnEnemies();
            
            TurnSystem.source.Play("CombatMusic");
            TurnSystem.source.Loop("CombatMusic");
            

            yield return new WaitForSeconds(0.2f);

            TurnSystem.SetState(new PlayerTurnState(TurnSystem));
        }
    }
}
