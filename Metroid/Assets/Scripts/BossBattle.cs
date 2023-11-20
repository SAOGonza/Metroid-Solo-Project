using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    // Get access to CameraController to control the camera
    // when the battle starts and ends.
    private CameraController theCam;
    public Transform camPosition;
    public float camSpeed;

    // Set up states for when the boss should phase.
    public int threshold1, threshold2;

    // Control how long things happen.
    public float activeTime, fadeOutTime, inactiveTime;
    private float activeCounter, fadeCounter, inactiveCounter;

    public Transform[] spawnPoints;
    private Transform targetPoint;
    public float moveSpeed;

    // Control animation to disappear.
    public Animator anim;
    public Transform theBoss;

    // Bullet variables
    public float timeBetweenShots1, timeBetweenShots2;
    private float shotCounter;
    public GameObject bullet;
    public Transform shotPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to camera.
        theCam = FindObjectOfType<CameraController>();

        // Disable camera component. This'll stop it from
        // following the player.
        theCam.enabled = false;

        // First phase.
        activeCounter = activeTime;

        // When the boss spawns in, wait 1 sec before firing a shot.
        shotCounter = timeBetweenShots1;
    }

    // Update is called once per frame
    void Update()
    {
        theCam.transform.position = Vector3.MoveTowards(theCam.transform.position, camPosition.position, camSpeed * Time.deltaTime);

        // We're in 1st phase.
        if (BossHealthController.instance.currentHealth > threshold1)
        {
            Phase1();
        }
        else
        {
            // Make sure we have a target point in the middle of
            // phasing to phase 2. Instantly go to phase 2.
            if (targetPoint == null)
            {
                targetPoint = theBoss;
                fadeCounter = fadeOutTime;
                anim.SetTrigger("vanish");
            }
            else
            {
                if (Vector3.Distance(theBoss.position, targetPoint.position) > .02f) // Check if close to a target point.
                {
                    // Move towards that point.
                    theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);

                    
                    if (Vector3.Distance(theBoss.position, targetPoint.position) <= .02f) // If boss is within range after we've moved.
                    {
                        fadeCounter = fadeOutTime;
                        anim.SetTrigger("vanish");
                    }
                }
                else if (fadeCounter > 0) // Boss is fading.
                {
                    fadeCounter -= Time.deltaTime;
                    if (fadeCounter <= 0) // Boss is now invisible.
                    {
                        theBoss.gameObject.SetActive(false);
                        inactiveCounter = inactiveTime;
                    }
                }
                else if (inactiveCounter > 0)
                {
                    inactiveCounter -= Time.deltaTime;
                    if (inactiveCounter <= 0) // Make boss reappear at diff point.
                    {
                        theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                        // Before reactivating the boss, pick a new target point.
                        // But don't pick the same point.
                        targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                        // In case we loop forever, set a whileBreaker and keep the original position.
                        int whileBreaker = 0;
                        while (targetPoint.position == theBoss.position && whileBreaker < 100)
                        {
                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                            whileBreaker++;
                        }

                        theBoss.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private void Phase1()
    {
        if (activeCounter > 0) // Our boss is active.
        {
            activeCounter -= Time.deltaTime;
            if (activeCounter <= 0) // Move to next spawnPoint.
            {
                fadeCounter = fadeOutTime;
                anim.SetTrigger("vanish");
            }

            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots1;

                Instantiate(bullet, shotPoint.position, Quaternion.identity);
            }
        }
        else if (fadeCounter > 0) // Boss is fading.
        {
            fadeCounter -= Time.deltaTime;
            if (fadeCounter <= 0) // Boss is now invisible.
            {
                theBoss.gameObject.SetActive(false);
                inactiveCounter = inactiveTime;
            }
        }
        else if (inactiveCounter > 0)
        {
            inactiveCounter -= Time.deltaTime;
            if (inactiveCounter <= 0) // Make boss reappear at diff point.
            {
                theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                theBoss.gameObject.SetActive(true);

                activeCounter = activeTime;

                // Reset shot counter
                shotCounter = timeBetweenShots1;
            }
        }
    }

    public void EndBattle()
    {
        gameObject.SetActive(false);
    }
}
