using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    // Patrol variables
    public Transform[] patrolPoints;
    private int currentPoint;

    // Variables for walking to patrol points.
    public float moveSpeed, waitAtPoints;
    private float waitCounter;

    // Jump variables.
    public float jumpForce;
    public Rigidbody2D enemyRb;

    // Animation variables
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoints;

        // Remove the parent so that the patrol points don't get pulled
        // along the Enemy Walker.
        foreach (Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards the point. Once at the point, wait a second.
        // Check if patrol point is at the same horizontal level.
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > .2f)
        {
            // We're on the left. Start moving to the right of the patrol point.
            if (transform.position.x < patrolPoints[currentPoint].position.x)
            {
                enemyRb.velocity = new Vector2(moveSpeed, enemyRb.velocity.y);

                // Make enemy turn to the right.
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else // On the right. Start moving to the left patrol point.
            {
                enemyRb.velocity = new Vector2(-moveSpeed, enemyRb.velocity.y);

                // Make enemy face the left.
                transform.localScale = Vector3.one;
            }

            // Make enemy jump to point.
            if (transform.position.y < patrolPoints[currentPoint].position.y - .5f && enemyRb.velocity.y < .1f)
            {
                enemyRb.velocity = new Vector2(enemyRb.velocity.x, jumpForce);
            }
        }

        // Once we're at the point...
        else
        {
            // Stop at the point for a sec.
            enemyRb.velocity = new Vector2(0f, enemyRb.velocity.y);

            // Start counting down.
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                waitCounter = waitAtPoints;
                currentPoint++;

                // Reset array once we've reached all patrol points.
                if (currentPoint >= patrolPoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }

        anim.SetFloat("speed", Mathf.Abs(enemyRb.velocity.x));
    }
}
