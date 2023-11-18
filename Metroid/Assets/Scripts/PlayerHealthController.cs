using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Reference for other scripts to access from anywhere.
    public static PlayerHealthController instance;

    [HideInInspector] public int currentHealth;
    public int maxHealth;

    // Player flashing on damage variables
    public float invincibilityLength;
    private float invincibilityCounter;

    public float flashLength;
    private float flashCounter;

    public SpriteRenderer[] playerSprites;

    private void Awake()
    {
        // Set new instance if we have no players.
        if (instance == null)
        {

            instance = this;
            // Tell the instance to not destroy itself on a new level/restarted scene.
            // This will carry over our same character & data on loading a scene.
            DontDestroyOnLoad(gameObject);
        }
        else // Destroy instance if we already have another one.
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease time of invincibility over time.
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                foreach (SpriteRenderer sr in  playerSprites)
                {
                    sr.enabled = !sr.enabled;
                }
                flashCounter = flashLength;
            }

            // Make sure we're back to being visible when timer runs out.
            if (invincibilityCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                {
                    sr.enabled = true;
                }
                // Reset counter so that next time we take damage, we'll immediately
                // start flashing.
                flashCounter = 0f;
            }
        }
    }


    public void DamagePlayer(int damageAmount)
    {
        // Only apply damage if we're not invincible.
        if (invincibilityCounter <= 0)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                // Reset health back to 0 in case enemy makes you have negative health.
                currentHealth = 0;

                RespawnController.instance.Respawn();
            }
            else // Make player invincible.
            {
                FlashPlayer();
            }

            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void FlashPlayer()
    {
        invincibilityCounter = invincibilityLength;
    }

    public void FillHealth()
    {
        currentHealth = maxHealth;

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}
