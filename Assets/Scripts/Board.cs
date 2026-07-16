using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject targetUI;
    public Text uiText;
    [TextArea(3, 8)]
    public string boardMessage;
    public KeyCode interactKey = KeyCode.E;

    private bool playerInRange;
    private PlayerMove playerMove;

    private void Start()
    {
        if (targetUI != null)
        {
            targetUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(interactKey) || targetUI == null)
        {
            return;
        }

        bool isOpening = !targetUI.activeSelf;
        targetUI.SetActive(isOpening);

        if (isOpening && uiText != null)
        {
            uiText.text = boardMessage;
        }

        if (playerMove != null)
        {
            playerMove.canMove = !isOpening;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerMove = collision.GetComponent<PlayerMove>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
