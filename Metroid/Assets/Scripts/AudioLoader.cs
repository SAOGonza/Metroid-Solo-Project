using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    // Reference to AudioManager
    public AudioManager theAM;

    public void Awake()
    {
        if (AudioManager.instance == null) // If there's no Audio Manager yet.
        {
            AudioManager newAM = Instantiate(theAM);
            AudioManager.instance = newAM;
            DontDestroyOnLoad(newAM.gameObject); // Load reference to AudioManager.
        }
    }
}
