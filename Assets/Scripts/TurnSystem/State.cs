using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected TurnSystem.TurnSystem TurnSystem;

    public State(TurnSystem.TurnSystem turnSystem)
    {
        TurnSystem = turnSystem;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator CheckState()
    {
        yield break;
    }

    public virtual IEnumerator Think()
    {
        yield break;
    }
}
