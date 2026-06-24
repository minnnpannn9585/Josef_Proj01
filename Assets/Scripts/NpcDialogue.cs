using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcDialogue : MonoBehaviour
{
    public Transform playerTrans;
    public GameObject dialogueCanvas;
    public PlayerMove playerMove;
    public bool isInDialogue = false;
    public string[] dialogueLines;
    public TMP_Text dialogueText;
    public int currentLine = 0;

    void Update()
    {
        if(!isInDialogue && Input.GetKeyDown(KeyCode.E) && Vector2.Distance(playerTrans.position, transform.position) < 2f)
        {
            dialogueCanvas.SetActive(!dialogueCanvas.activeSelf);
            dialogueText.text = dialogueLines[currentLine];

            playerMove.canMove = !dialogueCanvas.activeSelf;
            isInDialogue = true;
        }

        if(isInDialogue && Input.GetMouseButtonDown(0))
        {
            if(currentLine >= dialogueLines.Length - 1)
            {
                dialogueCanvas.SetActive(false);
                playerMove.canMove = true;
                isInDialogue = false;
                currentLine = 0;
                return;
            }
            currentLine++;
            dialogueText.text = dialogueLines[currentLine];
        }
    }
}
