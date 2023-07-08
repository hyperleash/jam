using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSM : StateMachine
{
    public float speed = 4f;
    [SerializeField]
    public float jumpForce = 14f;
    public Rigidbody2D rigidbody;

    public SpriteRenderer spriteRenderer;

    [HideInInspector]
    public Attacking attackingState;
    [HideInInspector]
    public Running runningState;
    [HideInInspector]
    public Jumping jumpingState;


    private void Awake()
    {
        //attackingState = new Attacking(this);
        runningState = new Running(this);
        jumpingState = new Jumping(this);

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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        grounded = true;

    }
}
