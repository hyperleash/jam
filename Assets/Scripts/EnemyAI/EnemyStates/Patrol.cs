using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BaseState
{
    protected EnemySM sm;



    public Patrol(EnemySM stateMachine) : base("Patrol", stateMachine)
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
        Vector2 direction = Vector2.right;

        if (!sm.patrol)
        {
            direction = Vector2.left;
        }
        sm.rigidbody.MovePosition(sm.rigidbody.position + Time.deltaTime * direction);

    }

}