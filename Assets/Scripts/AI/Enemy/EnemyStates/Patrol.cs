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

        if(sm.playerToRight()){
            sm.seekDirection = Vector2.right;
            stateMachine.ChangeState(sm.attackState);
        }

        if(sm.playerToLeft()){
            sm.seekDirection = Vector2.left;
            stateMachine.ChangeState(sm.attackState);
        }

        /*Vector2 rayStartRight = (Vector2)sm.transform.position;
        Vector2 rayStartLeft = (Vector2)sm.transform.position;

        rayStartRight.y = rayStartRight.y - sm.gameObject.GetComponent<Collider2D>().bounds.size.y/2 + 1f;
        rayStartLeft.y = rayStartLeft.y - sm.gameObject.GetComponent<Collider2D>().bounds.size.y/2 + 1f;

        rayStartRight.x = rayStartRight.x + (sm.gameObject.GetComponent<Collider2D>().bounds.size.x/2 + 0.1f);
        rayStartLeft.x = rayStartLeft.x - (sm.gameObject.GetComponent<Collider2D>().bounds.size.x/2 + 0.1f);

        RaycastHit2D rightHit = Physics2D.Raycast(rayStartRight, Vector2.right , sm.spellRange);
        RaycastHit2D leftHit = Physics2D.Raycast(rayStartLeft, Vector2.left , sm.spellRange);
        
        Debug.DrawRay(rayStartRight, Vector2.right * sm.spellRange, Color.green, 0);
        Debug.DrawRay(rayStartLeft, Vector2.left * sm.spellRange, Color.green, 0);

        if(rightHit.collider != null){
          
            Debug.DrawRay(rayStartRight, Vector2.right * sm.spellRange, Color.blue, 0);
            
            if(rightHit.collider.gameObject.tag == "Player"){
                Debug.Log(rightHit.collider.gameObject.tag);
                Debug.DrawRay(rayStartRight, Vector2.right * sm.spellRange, Color.red, 0);
                

                sm.seekDirection = Vector2.right;
                stateMachine.ChangeState(sm.attackState);
            }
        }

        if(leftHit.collider != null){
          
            Debug.DrawRay(rayStartLeft, Vector2.left * sm.spellRange, Color.blue, 0);
            Debug.Log(leftHit.collider.gameObject.tag);
            if(leftHit.collider.gameObject.tag == "Player"){
                //Debug.Log(leftHit.collider.gameObject.tag);
                Debug.DrawRay(rayStartLeft, Vector2.left * sm.spellRange, Color.red, 0);
                

                sm.seekDirection = Vector2.left;
                stateMachine.ChangeState(sm.attackState);
            }
        }*/

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
        

        //sm.rigidbody.MovePosition(sm.rigidbody.position + Time.deltaTime * direction);
        Vector2 vel = sm.rigidbody.velocity;
        vel.x =  ((EnemySM)stateMachine).speed * direction.x;
        sm.rigidbody.velocity = vel;
    }

}