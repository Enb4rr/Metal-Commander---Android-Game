using System.Collections;

namespace TurnSystem.States
{
    public class WonState : State
    {
        public WonState(global::TurnSystem.TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override IEnumerator Start()
        {
            TurnSystem.screenSystem.FadeToLevel("Win");

            yield break;
        }
    }
}
