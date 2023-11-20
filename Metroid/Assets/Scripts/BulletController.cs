using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Class variables
    public Rigidbody2D bulletRb;
    public Vector2 moveDir;

    // Shot variables
    public GameObject impactEffect;

    // Variables
    public float bulletSpeed;

    // Enemy variables
    public int damageAmount = 1;


    // Update is called once per frame
    void Update()
    {
        bulletRb.velocity = moveDir * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if bullet hit enemy
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }

        // Check if bullet hit boss.
        if (other.tag == "Boss")
        {
            BossHealthController.instance.TakeDamage(damageAmount);
        }

        // Check if bullet hit wall.
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
