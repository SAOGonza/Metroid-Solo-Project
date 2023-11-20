using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController instance;

    // Health variables
    public Slider bossHealthSlider;
    public int currentHealth = 30;

    // Reference to boss battle itself so that when boss
    // runs out of health, we can tell BossBattle to end.
    public BossBattle theBoss;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        bossHealthSlider.maxValue = currentHealth;
        bossHealthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0) {
            // Boss has ran out of health. Tell BossBattle.cs to end.
            currentHealth = 0;
            theBoss.EndBattle();

            AudioManager.instance.PlaySFX(0);
        }
        else
        {
            AudioManager.instance.PlaySFX(1);
        }

        // Update boss HP.
        bossHealthSlider.value = currentHealth;
    }
}
