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

    // Variables
    public float moveSpeed;
    public float jumpForce;
    private bool isOnGround;
    private float groundPointRadius = .2f;

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
    }

    void MovePlayer()
    {
        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerRb.velocity.y);
    }

    void JumpPlayer()
    {
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, groundPointRadius, whatIsGround);

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        }
    }

    void ControlAnimations()
    {
        anim.SetBool("isOnGround", isOnGround);
        anim.SetFloat("speed", Mathf.Abs(playerRb.velocity.x)); // Abs = number w/out a positive nor negative.
    }
}
