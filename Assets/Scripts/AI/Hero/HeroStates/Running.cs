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

        RaycastHit2D spellHit = Physics2D.Raycast((Vector2)sm.transform.position + Vector2.right, Vector2.right , sm.spellRange);
        Debug.DrawRay((Vector2)sm.transform.position + Vector2.right, Vector2.right * sm.spellRange, Color.green, 0);
        if(spellHit.collider != null){
            Debug.Log(spellHit.collider.gameObject.tag);
            Debug.DrawRay((Vector2)sm.transform.position + Vector2.right, Vector2.right * sm.spellRange, Color.blue, 0);
            if(spellHit.collider.gameObject.tag == "enemy"){
                Debug.Log("Hit enemy");
                Debug.DrawRay((Vector2)sm.transform.position + Vector2.right, Vector2.right * sm.spellRange, Color.red, 0);
            }
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
