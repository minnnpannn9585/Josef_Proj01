using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    Transform respawnPoint;

    private Vector3 initialPlayerPosition;
    private bool hasInitialPlayerPosition;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            initialPlayerPosition = player.transform.position;
            hasInitialPlayerPosition = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        Transform playerTransform = collision.transform;
        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

        if (respawnPoint != null)
        {
            playerTransform.position = respawnPoint.position;
        }
        else if (hasInitialPlayerPosition)
        {
            playerTransform.position = initialPlayerPosition;
        }
        else
        {
            playerTransform.position = Vector3.zero;
        }

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }
    }
}
