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
    public string sabotageMessage;
    public string helpMessage;
    public KeyCode interactKey = KeyCode.E;

    public bool changeable = false;
    public int type = 0;
    private int status = 0;
    private bool playerInRange;
    private PlayerMove playerMove;
    private GameObject sabotage;
    private GameObject help;
    private void Awake()
    {
        sabotage = GameObject.FindGameObjectWithTag("Sabotage");
        help = GameObject.FindGameObjectWithTag("Help");
    }
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

        if (playerMove.playerId != 0&&changeable)
        {
            if(playerMove.playerId == 2)
            {
                sabotage.SetActive(true);
            }
            else
            {
                sabotage.SetActive(false);
            }
            if(type == 1 && playerMove.playerId != 1)
            {
                help.SetActive(false);
            }
            else
            {
                help.SetActive(true);
            }
            

            
        }
        else
        {
            sabotage.SetActive(false);
            help.SetActive(false);
        }

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
