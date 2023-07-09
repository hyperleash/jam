using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySM : StateMachine
{
    public Animator animator;
    public float speed = 4f;
    [SerializeField]
    public float jumpForce = 14f;
    public Rigidbody2D rigidbody;

    public int attackDamage = 10;

    public SpriteRenderer spriteRenderer;

    [HideInInspector]
    public Attack attackState;
    [HideInInspector]
    public Seek seekState;
    [HideInInspector]
    public Patrol patrolState;
    //this.transform.position 
    public GameObject patrol1;
    public GameObject patrol2;

    public bool patrol;
    public Vector2 seekDirection;
    public Collider2D attackHitbox;

    public bool castFinished;

    private void Awake()
    {
        attackState = new Attack(this);
        seekState = new Seek(this);
        patrolState = new Patrol(this);
        patrol1 = this.transform.GetChild(1).gameObject;
        patrol2 = this.transform.GetChild(2).gameObject;

        patrol = false;

        patrol1.transform.SetParent(null, true);
        patrol2.transform.SetParent(null, true);

        
        attackHitbox = this.transform.GetChild(0).gameObject.GetComponent<Collider2D>();
        attackHitbox.enabled = false;

    }

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == patrol1)
        {
            patrol = true;
        }

        if (other.gameObject == patrol2)
        {
            patrol = false;
        }


        if (other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<HealthBehaviour>().Health = other.gameObject.GetComponent<HealthBehaviour>().Health - attackDamage;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        grounded = true;
    }

    public void die(){
        Destroy(gameObject);
    }

    protected void startAttack(){
        attackHitbox.enabled = true;
       
        castFinished = false;
    }

    protected void endAttack(){
        attackHitbox.enabled = false;
    }

    protected void finishCast(){
        castFinished = true;
    }

    public bool playerToRight(){
        Vector2 rayStartRight = (Vector2)transform.position;
        

        rayStartRight.y = rayStartRight.y - gameObject.GetComponent<Collider2D>().bounds.size.y/2 + 1f;
        

        rayStartRight.x = rayStartRight.x + (gameObject.GetComponent<Collider2D>().bounds.size.x/2 + 0.1f);
       

        RaycastHit2D rightHit = Physics2D.Raycast(rayStartRight, Vector2.right , spellRange);
       
        
        Debug.DrawRay(rayStartRight, Vector2.right * spellRange, Color.green, 0);
        

        if(rightHit.collider != null){
          
            Debug.DrawRay(rayStartRight, Vector2.right * spellRange, Color.blue, 0);
            
            if(rightHit.collider.gameObject.tag == "Player"){
                Debug.DrawRay(rayStartRight, Vector2.right * spellRange, Color.red, 0);
                

                return true;
            }
        }

        return false;
    }

    public bool playerToLeft(){
        Vector2 rayStartLeft = (Vector2)transform.position;
        rayStartLeft.y = rayStartLeft.y - gameObject.GetComponent<Collider2D>().bounds.size.y/2 + 1f;
        rayStartLeft.x = rayStartLeft.x - (gameObject.GetComponent<Collider2D>().bounds.size.x/2 + 0.1f);

        RaycastHit2D leftHit = Physics2D.Raycast(rayStartLeft, Vector2.left , spellRange);

        Debug.DrawRay(rayStartLeft, Vector2.left * spellRange, Color.green, 0);


        if(leftHit.collider != null){
          
            Debug.DrawRay(rayStartLeft, Vector2.left * spellRange, Color.blue, 0);
            
            if(leftHit.collider.gameObject.tag == "Player"){

                Debug.DrawRay(rayStartLeft, Vector2.left * spellRange, Color.red, 0);
                
                return true;
            }
        }

        return false;
    }
    
}