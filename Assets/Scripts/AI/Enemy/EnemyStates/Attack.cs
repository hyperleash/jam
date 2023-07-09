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
        sm.spriteRenderer.color = Color.blue;
        sm.animator.SetBool("attacking", true);

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(sm.seekDirection == Vector2.left){
            sm.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        } else if(sm.seekDirection == Vector2.right){
            sm.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        if(sm.castFinished){
            stateMachine.ChangeState(sm.patrolState);
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

    }

}