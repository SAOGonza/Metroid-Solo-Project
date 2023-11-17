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

    // Dash variables
    public float dashSpeed, dashTime;
    private float dashCounter;
    public float waitAfterDashing;
    private float dashRechargeCounter;

    // After image variables
    public SpriteRenderer spriteRenderer, afterImage;
    public float afterImageLifetime, timeBetweenAfterImages;
    private float afterImageCounter;
    public Color afterImageColor;

    // Variables for ball mode.
    public GameObject standing, ball;
    public float waitToBall;
    private float ballCounter;
    public Animator ballAnim;

    // Bomb drop variables
    public Transform bombPoint;
    public GameObject bomb;

    // References to PlayerAbilityTracker script
    private PlayerAbilityTracker abilities;

    // Start is called before the first frame update
    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        JumpPlayer();
        ControlAnimations();
        HandleFire();
        HandleBallMode();
    }

    void MovePlayer()
    {
        // Cooldown between dashing.
        if (dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
        else
        {
            // Let player dash on RMB click.
            if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash) // Only dash if standing.
            {
                dashCounter = dashTime;

                ShowAfterImage();
            }
        }

        // Give player ability to dash for duration of dashCounter.
        if (dashCounter > 0)
        {
            DashPlayer();
        }
        else // Move player normally when not dashing.
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
    }

    void DashPlayer()
    {
        dashCounter -= Time.deltaTime;
        playerRb.velocity = new Vector2(dashSpeed * transform.localScale.x, playerRb.velocity.y);

        // Count down time between after images to create several at a time.
        // our afterImageCounter was initialized in ShowAfterImage() function.
        afterImageCounter -= Time.deltaTime;
        if (afterImageCounter <= 0)
        {
            ShowAfterImage();
        }

        // Cooldown before dashing again.
        dashRechargeCounter = waitAfterDashing;
    }

    void JumpPlayer()
    {
        // Checking if on the ground.
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, groundPointRadius, whatIsGround);

        // Jumping.
        if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && abilities.canDoubleJump)))
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

    void HandleBallMode()
    {
        // Check if ball mode is not active. If not,
        // then get our vertical inputs to become a ball.
        if (!ball.activeSelf)
        {
            if (Input.GetAxisRaw("Vertical") < -.9f && abilities.canBecomeBall)
            {
                // Begin transition to the ball. Count down
                // so that we become a ball if player holds button
                // down long enough.
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    // Become ball.
                    ball.SetActive(true);
                    standing.SetActive(false);
                }
            }
            else
            {
                // Set counter to become ball back to full value
                // if we let go before becoming a ball.
                ballCounter = waitToBall;
            }
        }

        else
        {
            // Ball is active, standing isn't. So we'll reverse the process
            // to become a standing sprite.
            if (Input.GetAxisRaw("Vertical") > .9f)
            {
                // Begin transition to standing. Count down
                // so that we stand if player holds button
                // down long enough.
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    // Stand up.
                    standing.SetActive(true);
                    ball.SetActive(false);
                }
            }
            else
            {
                // Set counter to become ball back to full value
                // if we let go before standing up.
                ballCounter = waitToBall;
            }
        }
    }

    void ControlAnimations()
    {
        // Standing animations.
        if (standing.activeSelf)
        {
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("speed", Mathf.Abs(playerRb.velocity.x)); // Abs = number w/out a positive nor negative.
        }
        
        // Ball animations.
        if (ball.activeSelf)
        {
            ballAnim.SetFloat("speed", Mathf.Abs(playerRb.velocity.x));
        }
        
    }

    void HandleFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (standing.activeSelf)
            {
                Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);

                anim.SetTrigger("shotFired");
            }

            else if (ball.activeSelf && abilities.canDropBomb)
            {
                Instantiate(bomb, bombPoint.position, bombPoint.rotation);
            }
        }
    }

    public void ShowAfterImage()
    {
        // Create copy of after image prefab.
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = spriteRenderer.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        // Delete after images.
        Destroy(image.gameObject, afterImageLifetime);

        // Set after image timer to get multiple after images instead of just one.
        afterImageCounter = timeBetweenAfterImages;
    }
}
