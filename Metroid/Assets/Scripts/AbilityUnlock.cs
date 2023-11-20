using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    // Variables to determine which powerup is unlocked.
    public bool unlockDoubleJump, unlockDash, unlockBecomeBall, unlockDropBomb;

    public GameObject pickupEffect;

    // Pickup text
    public string unlockMessage;
    public TMP_Text unlockText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerAbilityTracker player = other.GetComponentInParent<PlayerAbilityTracker>();

            if (unlockDoubleJump)
            {
                player.canDoubleJump = true;
            }

            if (unlockDash)
            {
                player.canDash = true;
            }

            if (unlockBecomeBall)
            {
                player.canBecomeBall = true;
            }

            if (unlockDropBomb)
            {
                player.canDropBomb = true;
            }

            // Set parent to null so text becomes its own object.
            // Otherwise, text will be destroyed with object before text can spawn.
            // Next line will then set the canvas position to pickup position.
            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position = transform.position;

            // Set powerup message.
            unlockText.text = unlockMessage;
            unlockText.gameObject.SetActive(true);

            Instantiate(pickupEffect, transform.position, transform.rotation);

            // Destroy text and ability pickup GameObject.
            Destroy(unlockText.transform.parent.gameObject, 5.0f);
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(5);
        }
    }
}
