using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSM : StateMachine
{
    public float speed = 4f;
    [SerializeField]
    public float jumpForce = 14f;
    public float groundDistanceCheck = 0.1f;
    public int attackDamage = 10;
    
    public LayerMask groundMask;
    public Rigidbody2D rigidbody;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    [HideInInspector]
    public Attacking attackingState;
    [HideInInspector]
    public Running runningState;
    [HideInInspector]
    public Jumping jumpingState;

    private Collider2D collider;

    public Collider2D attackHitbox;

    public GameObject spell; 

    public Vector2 spellTarget;

    public bool castFinished;
    

    private void Awake()
    {
        attackingState = new Attacking(this);
        runningState = new Running(this);
        jumpingState = new Jumping(this);

        collider = GetComponent<Collider2D>();
        attackHitbox = this.transform.GetChild(0).gameObject.GetComponent<Collider2D>();
        attackHitbox.enabled = false;
    }

    protected override BaseState GetInitialState()
    {
        return runningState;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "jump")
        {
            jump = true;
        }

        if (other.gameObject.tag == "enemy"){
            other.gameObject.GetComponent<HealthBehaviour>().Health = other.gameObject.GetComponent<HealthBehaviour>().Health - attackDamage;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (Physics2D.BoxCast(
            new Vector2(transform.position.x, collider.bounds.center.y - collider.bounds.extents.y), 
            collider.bounds.size, transform.rotation.z, Vector2.down, groundDistanceCheck, groundMask))
        {
            grounded = true;
        }
    }

    protected void startAttack(){
        attackHitbox.enabled = true;
       
        castFinished = false;
    }

    protected void endAttack(){
        attackHitbox.enabled = false;
    }

    protected void spawnSpell(){
        Instantiate(spell, spellTarget, Quaternion.identity);
        castFinished = false;
    }

    protected void finishCast(){
        castFinished = true;
    }

    public void die(){
        Destroy(gameObject);
    }

    
}
