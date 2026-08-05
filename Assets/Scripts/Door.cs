using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public Collider2D doorCollider;
    public GameObject closedVisual;
    public GameObject openVisual;
    public PlayerMove playerMove;
    private bool isOpen;
    private bool playerInRange;

    private void Start()
    {
        if (doorCollider == null)
        {
            doorCollider = transform.GetChild(0).GetComponent<Collider2D>();
        }

        SetDoorState(false);
    }

    private void Update()
    {
        if (!playerInRange || isOpen || !(playerMove.inventory.GetItemCount("Key")>=1))
        {
            return;
        }

        if (Input.GetKeyDown(interactKey))
        {
            SetDoorState(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void SetDoorState(bool open)
    {
        isOpen = open;

        if (doorCollider != null)
        {
            doorCollider.enabled = !isOpen;
        }

        if (closedVisual != null)
        {
            closedVisual.SetActive(!isOpen);
        }

        if (openVisual != null)
        {
            openVisual.SetActive(isOpen);
        }
    }
}
