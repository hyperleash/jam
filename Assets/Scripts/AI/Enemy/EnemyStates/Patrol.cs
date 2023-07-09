using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BaseState
{
    protected EnemySM sm;



    public Patrol(EnemySM stateMachine) : base("Patrol", stateMachine)
    {
        sm = (EnemySM)this.stateMachine;
        sm.spriteRenderer.color = Color.red;
        

    }

    public override void Enter()
    {
        base.Enter();
        sm.animator.SetBool("attacking", false);

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
       
        RaycastHit2D rightHit = Physics2D.Raycast((Vector2)sm.transform.position + Vector2.right, Vector2.right , sm.spellRange);
        RaycastHit2D leftHit = Physics2D.Raycast((Vector2)sm.transform.position + Vector2.left, Vector2.left , sm.spellRange);
        Debug.DrawRay((Vector2)sm.transform.position + Vector2.right, Vector2.right * sm.spellRange, Color.green, 0);
        Debug.DrawRay((Vector2)sm.transform.position + Vector2.right, Vector2.left * sm.spellRange, Color.green, 0);

        if(rightHit.collider != null){
          
            Debug.DrawRay((Vector2)sm.transform.position + Vector2.right, Vector2.right * sm.spellRange, Color.blue, 0);
            
            if(rightHit.collider.gameObject.tag == "Player"){
                
                Debug.DrawRay((Vector2)sm.transform.position + Vector2.right, Vector2.right * sm.spellRange, Color.red, 0);
                

                sm.seekDirection = Vector2.right;
                stateMachine.ChangeState(sm.attackState);
            }
        }

        if(leftHit.collider != null){
          
            Debug.DrawRay((Vector2)sm.transform.position + Vector2.left, Vector2.left * sm.spellRange, Color.blue, 0);
            
            if(leftHit.collider.gameObject.tag == "Player"){
                
                Debug.DrawRay((Vector2)sm.transform.position + Vector2.left, Vector2.left * sm.spellRange, Color.red, 0);
                

                sm.seekDirection = Vector2.left;
                stateMachine.ChangeState(sm.attackState);
            }
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        Vector2 direction = Vector2.right;
        
        
        if (!sm.patrol)
        {
            sm.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            direction = Vector2.left;
        } else {
            sm.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        sm.rigidbody.MovePosition(sm.rigidbody.position + Time.deltaTime * direction);

    }

}