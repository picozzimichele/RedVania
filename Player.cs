using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Configs
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    private float movmentInput;

    //State
    bool isAlive = true;


    //Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    CircleCollider2D myFeetCoolider;
    public float playerGravityAtStart;


    //Messages then methods
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCoolider = GetComponent<CircleCollider2D>();
        playerGravityAtStart = myRigidBody.gravityScale; //getting the gravity at start
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isAlive) { return; } //if player dies then he will no longer have access to the methods below

        Run();
        ClimbLadder();
        Jump();
        Die();
        FlipSprite();
    }

    private void Run()
    {
        movmentInput = Input.GetAxis("Horizontal");
        Vector2 runVelocity = new Vector2(movmentInput * movementSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = runVelocity;


        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed); //this flips the bool based on second value, in this case playerHasHorizontalSpeed

    }

    private void ClimbLadder()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = playerGravityAtStart; //resetting the gravity once out of the ladder
            return;
        }

        myRigidBody.gravityScale = 0f; //setting the gravity to zero while on the ladder to avoid sliding down
        movmentInput = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, movmentInput * climbSpeed);
        myRigidBody.velocity = climbVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

    }

    private void Jump()
    {
        if (!myFeetCoolider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (Input.GetButton("Jump")) //Input.GetButtonDown("Jump") || Input.GetButton("Jump")
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpForce);
            myRigidBody.velocity += jumpVelocityToAdd;
        }

    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Die");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath(); //here we go and take the GameSession script and access the public method ProcessPlayerDeath!
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; //We return the absolute value of velocity.x and has to be greater than > 0 (Mathf.Epsilon = 0)
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f); //Mathf.Sign returns either -1 or 1 depending if value is positive or negative --> rigidbody.velocity.x is positive going right and negative going left
        }

    }
}
