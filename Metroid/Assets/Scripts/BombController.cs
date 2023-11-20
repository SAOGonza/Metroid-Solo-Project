using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    // Bomb explosion variables
    public float timeToExplode = 1.5f;
    public GameObject explosion;

    // Obstacle destructible variables.
    public float blastRange;
    public LayerMask whatIsDestructible;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToExplode -= Time.deltaTime;
        if (timeToExplode <= 0)
        {
            // If we have a game object assigned to explosion
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
            Destroy(gameObject);

            // Destroy objects that can be destroyed.
            RemoveDestructibleObjects();
        }
    }

    void RemoveDestructibleObjects()
    {
        // Get an array of everything that has a Destructible layer
        // within our range and destroy the object & remove from our scene.
        Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDestructible); // OverlapCircleAll gives an array of collider objects.
        if (objectsToRemove.Length > 0)
        {
            foreach (Collider2D col in objectsToRemove)
            {
                Destroy(col.gameObject);
            }
        }

        AudioManager.instance.PlaySFXAdjusted(4);
    }
}
