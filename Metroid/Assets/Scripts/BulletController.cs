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


    // Update is called once per frame
    void Update()
    {
        bulletRb.velocity = moveDir * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
