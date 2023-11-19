using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DoorController : MonoBehaviour
{
    public Animator anim;
    public float distanceToOpen;

    // Control player movement when they touch the door,
    // making them walk in.
    private PlayerController player;

    // Variable to make sure the door happens only once.
    private bool playerExiting;
    public Transform exitPoint;
    public float movePlayerSpeed;

    public string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.instance.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Keep track of how far the player is. If they go a certain distance,
        // open the door.
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToOpen)
        {
            anim.SetBool("doorOpen", true);
        }
        else
        {
            anim.SetBool("doorOpen", false);
        }

        // Check if player is exiting into a new level.
        if (playerExiting)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, exitPoint.transform.position, movePlayerSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!playerExiting) // If player isn't exiting the level.
            {
                player.canMove = false;
                StartCoroutine(UseDoorCo());
            }
        }
    }

    IEnumerator UseDoorCo()
    {
        playerExiting = true;
        player.anim.enabled = false;

        // Start using door fade.
        UIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        // Let player move again. Set new spawn.
        RespawnController.instance.SetSpawn(exitPoint.transform.position);
        player.canMove = true;
        player.anim.enabled = true;

        // Start fading from black.
        UIController.instance.StartFadeFromBlack();
        SceneManager.LoadScene(levelToLoad);
    }
}
