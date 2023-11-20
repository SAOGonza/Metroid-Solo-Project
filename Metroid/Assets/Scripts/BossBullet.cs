using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    // Variables
    public float moveSpeed;
    public Rigidbody2D bulletRb;

    public int damageAmount;
    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        // Make it start by targeting the player.
        // Check which direction the player is relative to enemy.
        Vector3 direction = transform.position - PlayerHealthController.instance.transform.position;

        // Get the angle to turn.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set rotation of where the bullet will fly towards.
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Play Fireball audio
        AudioManager.instance.PlaySFXAdjusted(2);
    }

    // Update is called once per frame
    void Update()
    {
        bulletRb.velocity = -transform.right * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Damage the player.
            PlayerHealthController.instance.DamagePlayer(damageAmount);
        }

        // Check for impact effects. Destroy bullet on any impact.
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
