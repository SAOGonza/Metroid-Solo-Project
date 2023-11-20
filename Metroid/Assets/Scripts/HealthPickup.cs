using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount;
    public GameObject pickupEffect;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.HealPlayer(healthAmount);

            // If we have a pickup effect, then instantiate.
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Destroy health pickup.
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(5);
        }
    }
}
