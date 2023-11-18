using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Reference for other scripts to access from anywhere.
    public static PlayerHealthController instance;

    [HideInInspector] public int currentHealth;
    public int maxHealth;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DamagePlayer(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0) 
        {
            // Reset health back to 0 in case enemy makes you have negative health.
            currentHealth = 0;

            gameObject.SetActive(false);
        }
    }
}
