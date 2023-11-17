using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Class variables
    public Rigidbody2D playerRb;
    public Transform groundPoint;
    public LayerMask whatIsGround;
    public Animator anim;

    // Shot variables
    public BulletController shotToFire;
    public Transform shotPoint;

    // Variables
    public float moveSpeed;
    public float jumpForce;
    private bool isOnGround;
    private float groundPointRadius = .2f;

    // Double jump variables
    private bool canDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        JumpPlayer();
        ControlAnimations();
        HandleFire();
    }

    void MovePlayer()
    {
        // Move sideways
        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerRb.velocity.y);

        // Handle direction change.
        if (playerRb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (playerRb.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
    }

    void JumpPlayer()
    {
        // Checking if on the ground.
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, groundPointRadius, whatIsGround);

        // Jumping.
        if (Input.GetButtonDown("Jump") && (isOnGround || canDoubleJump))
        {
            // Check to let player double jump
            if (isOnGround)
            {
                canDoubleJump = true;
            }
            else
            {
                canDoubleJump = false;
                anim.SetTrigger("doubleJump");
            }

            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        }
    }

    void ControlAnimations()
    {
        anim.SetBool("isOnGround", isOnGround);
        anim.SetFloat("speed", Mathf.Abs(playerRb.velocity.x)); // Abs = number w/out a positive nor negative.
    }

    void HandleFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);

            anim.SetTrigger("shotFired");
        }
    }
}
