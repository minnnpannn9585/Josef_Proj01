using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    private PlayerMove playerMove;
    private readonly HashSet<Collider2D> touchingGrounds = new HashSet<Collider2D>();

    private void Awake()
    {
        playerMove = GetComponentInParent<PlayerMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerMove == null)
        {
            return;
        }

        if (collision.CompareTag("Switch"))
        {
            playerMove.RegisterSwitchTrigger(collision);
            return;
        }

        if (!playerMove.IsGroundLayer(collision.gameObject.layer))
        {
            return;
        }

        touchingGrounds.Add(collision);
        RefreshGroundedState();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerMove == null)
        {
            return;
        }

        if (collision.CompareTag("Switch"))
        {
            playerMove.UnregisterSwitchTrigger(collision);
            return;
        }

        if (!playerMove.IsGroundLayer(collision.gameObject.layer))
        {
            return;
        }

        touchingGrounds.Remove(collision);
        RefreshGroundedState();
    }

    public void RefreshGroundedState()
    {
        if (playerMove == null)
        {
            return;
        }

        touchingGrounds.RemoveWhere(collider => collider == null);

        foreach (Collider2D groundCollider in touchingGrounds)
        {
            if (playerMove.IsCurrentGroundLayer(groundCollider.gameObject.layer))
            {
                playerMove.isGrounded = true;
                return;
            }
        }

        playerMove.isGrounded = false;
    }
}
