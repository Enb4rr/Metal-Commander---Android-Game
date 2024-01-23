using System.Collections;

namespace TurnSystem.States
{
    public class LostState : State
    {
        public LostState(global::TurnSystem.TurnSystem turnSystem) : base(turnSystem)
        {
        }

        public override IEnumerator Start()
        {
            TurnSystem.screenSystem.FadeToLevel("Lost");

            yield break;
        }
    }
}
