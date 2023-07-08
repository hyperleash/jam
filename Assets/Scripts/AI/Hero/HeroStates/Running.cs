using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : BaseState
{
    protected HeroSM sm;

    public Running(HeroSM stateMachine) : base("Running", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        sm = (HeroSM)this.stateMachine;
        sm.spriteRenderer.color = Color.red;
        sm.animator.SetBool("running", true);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (sm.jump)
        {
            stateMachine.ChangeState(sm.jumpingState);
        }
        
    }

    public override void UpdatePhysics()
    {
       
        //Vector2 runningForce = 2f * Vector2.right * ((HeroSM)stateMachine).speed;
        //sm.rigidbody.AddForce(runningForce);
        //sm.rigidbody.MovePosition(sm.rigidbody.position + Time.deltaTime * Vector2.right * ((HeroSM)stateMachine).speed);
        base.UpdatePhysics();
        Vector2 vel = sm.rigidbody.velocity;
        vel.x =  ((HeroSM)stateMachine).speed;
        sm.rigidbody.velocity = vel;
        
        //Debug.Log(runningForce);
    }
}
