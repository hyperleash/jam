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
}