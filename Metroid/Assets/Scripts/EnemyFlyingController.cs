using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    // Variables so when the player is close enough,
    // start moving towards the player.
    public float rangeToStartChase;
    private bool isChasing;

    public float moveSpeed, turnSpeed;

    private Transform player;

    // Animation variables
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // Find the object with the PlayerHealthController component.
        player = PlayerHealthController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we're not chasing.
        if (!isChasing)
        {
            // Get distance between two points and see if it's within range.
            if (Vector3.Distance(transform.position, player.position) < rangeToStartChase)
            {
                // Is within range.
                isChasing = true;

                anim.SetBool("isChasing", isChasing);
            }
        }
        else
        {
            // Check if player is active. Stop chasing if player is dead.
            if (player.gameObject.activeSelf)
            {
                // Check which direction the player is relative to enemy.
                Vector3 direction = transform.position - player.position;

                // Get the angle to turn.
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Set rotation
                Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);

                // Turn to that point. Slerp(currentPos, targetPos, how fast we want to rotate)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);


                // Start moving enemy in a fluid way, with transform.left
                transform.position += -transform.right * moveSpeed * Time.deltaTime;
            }
        }
    }
}
