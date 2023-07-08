using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : BaseState
{

    protected HeroSM sm;


    
    public Attacking(HeroSM stateMachine) : base("Attack", stateMachine)
    {
        
        sm = (HeroSM)this.stateMachine;
        sm.spriteRenderer.color = Color.blue;
       
    }

    public override void Enter()
    {
        base.Enter();
        sm.animator.SetBool("attacking", true);
        

        Vector2 vel = sm.rigidbody.velocity;
        vel.x = 0f;
        sm.rigidbody.velocity = vel;

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(sm.castFinished){
            stateMachine.ChangeState(sm.runningState);
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }


}
