using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    // Make it accessible everywhere.
    public static RespawnController instance;

    private Vector3 respawnPoint;
    public float waitToRespawn;

    private GameObject player;

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
        player = PlayerHealthController.instance.gameObject;

        respawnPoint = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpawn(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(waitToRespawn);

        // Reload entire scene to restart level during respawn.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        player.transform.position = respawnPoint;
        player.SetActive(true);

        // Refill health.
        PlayerHealthController.instance.FillHealth();
    }
}
