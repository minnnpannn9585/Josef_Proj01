using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;

    private bool playerInRange;
    private PlayerMove currentPlayer;
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

    private void Update()
    {
        if (!playerInRange || currentPlayer == null || !Input.GetKeyDown(interactKey))
        {
            return;
        }

        if (currentPlayer.playerId == 0 || currentPlayer.playerId == 2)
        {
            Rigidbody2D playerRb = currentPlayer.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero;
                playerRb.angularVelocity = 0f;
            }

            if (hasInitialPlayerPosition)
            {
                currentPlayer.transform.position = initialPlayerPosition;
            }
            else
            {
                currentPlayer.transform.position = Vector3.zero;
            }
        }
        else if (currentPlayer.playerId == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        currentPlayer = collision.GetComponent<PlayerMove>();

        if (currentPlayer != null)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (collision.GetComponent<PlayerMove>() == currentPlayer)
        {
            playerInRange = false;
            currentPlayer = null;
        }
    }
}
