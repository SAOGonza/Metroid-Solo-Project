using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHealthController : MonoBehaviour
{
    public int totalHealth = 3;

    public GameObject deathEffect;

    public void DamageEnemy(int damageAmount)
    {
        totalHealth -= damageAmount;

        // Kill enemy once health is depleted.
        if (totalHealth <= 0)
        {
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
