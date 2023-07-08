using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : BaseState
{
    protected HeroSM sm;

    public Jumping(HeroSM stateMachine) : base("Jumping", stateMachine)
    {
        sm = (HeroSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        sm.spriteRenderer.color = Color.green;

        Vector2 vel = sm.rigidbody.velocity;
        vel.y += sm.jumpForce;
        vel.x += 1f;
        sm.rigidbody.velocity = vel;

        sm.grounded = false;
        sm.jump = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (sm.grounded)
            stateMachine.ChangeState(sm.runningState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

    }

}
