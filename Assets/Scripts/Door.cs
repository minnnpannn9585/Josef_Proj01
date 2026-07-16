using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public Collider2D doorCollider;
    public GameObject closedVisual;
    public GameObject openVisual;
    public bool hasKey;

    private bool isOpen;
    private bool playerInRange;

    private void Start()
    {
        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider2D>();
        }

        SetDoorState(false);
    }

    private void Update()
    {
        if (!playerInRange || isOpen || !hasKey)
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

    public void SetHasKey(bool value)
    {
        hasKey = value;
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
