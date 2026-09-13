using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
    public Button button;
    
    public bool changeable = false;
    public int type = 0;
    private ButtonDetector buttonDetector;
    private int status = 0;
    private bool playerInRange;
    private PlayerMove playerMove;
    private GameObject help;
    private void Awake()
    {
        help = GameObject.FindGameObjectWithTag("Help");
    }
    private void Start()
    {
        if (targetUI != null)
        {
            targetUI.SetActive(false);
        }
        buttonDetector = button.GetComponent<ButtonDetector>();
    }

    private void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(interactKey) || targetUI == null)
        {
            return;
        }

        bool isOpening = !targetUI.activeSelf;
        targetUI.SetActive(isOpening);
        buttonDetector.board = this;
        if (playerMove.playerId != 0)
        {

            if(type == 1 && playerMove.playerId != 1)
            {
                help.SetActive(false);
            }
            else
            {
                help.SetActive(true);
            }
            
            
            
        }
        else if(type == 2 && playerMove.playerId == 0)
            
            {
                help.SetActive(false);
                playerMove.pass = true;
                playerMove.wall.SetActive(false);
            }
        else{
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
