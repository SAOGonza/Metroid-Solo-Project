using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Slider healthSlider;

    public void Awake()
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
