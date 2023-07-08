using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : BaseState
{
    protected EnemySM sm;

    public Seek(EnemySM stateMachine) : base("Seek", stateMachine)
    {
        sm = (EnemySM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

    }

}