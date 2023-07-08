using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseState
{
    protected EnemySM sm;

    public Attack(EnemySM stateMachine) : base("Attack", stateMachine)
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