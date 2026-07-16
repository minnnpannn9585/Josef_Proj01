using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordChangeCharacter : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer;
    public Color targetColor = Color.white;
    public Transform respawnPoint;

    public Board boardMessage1;
    public string message1;

    private Vector3 initialPlayerPosition;
    private bool hasInitialPlayerPosition;
    public int targetId;
    public PlayerMove playerMove;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            initialPlayerPosition = player.transform.position;
            hasInitialPlayerPosition = true;

            if (targetSpriteRenderer == null)
            {
                targetSpriteRenderer = player.GetComponent<SpriteRenderer>();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        playerMove.playerId = targetId;

        if (boardMessage1 != null)
        {
            boardMessage1.boardMessage = message1;
        }

        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }

        Vector3 spawnPosition = Vector3.zero;

        if (respawnPoint != null)
        {
            spawnPosition = respawnPoint.position;
        }
        else if (hasInitialPlayerPosition)
        {
            spawnPosition = initialPlayerPosition;
        }

        collision.transform.position = spawnPosition;

        SpriteRenderer spriteRenderer = targetSpriteRenderer;

        if (spriteRenderer == null)
        {
            spriteRenderer = collision.GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = targetColor;
        }

        Destroy(gameObject);
    }
}
