using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Make the camera find the player in each scene.
    public PlayerController player;
    public BoxCollider2D boundsBox;

    private float halfHeight, halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // Set camera to stay within the bounds.
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        // Don't get an error if player is not active in scene.
        if (player != null)
        {
            transform.position = new Vector3(
                Mathf.Clamp(player.transform.position.x, boundsBox.bounds.min.x + halfWidth, boundsBox.bounds.max.x - halfWidth),
                Mathf.Clamp(player.transform.position.y, boundsBox.bounds.min.y + halfHeight, boundsBox.bounds.max.y - halfHeight),
                transform.position.z);
        }
    }
}
