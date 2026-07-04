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

        if (playerMove.showCollisionDebug)
        {
            Debug.Log(
                $"[Foot] OnTriggerEnter2D with {collision.name}, tag: {collision.tag}, isTrigger: {collision.isTrigger}");
        }

        if (collision.CompareTag("Switch"))
        {
            if (playerMove.showCollisionDebug)
            {
                Debug.Log($"[Foot] Register switch: {collision.name}");
            }

            playerMove.RegisterSwitchTrigger(collision);
            return;
        }

        if (!playerMove.IsGroundLayer(collision.gameObject.layer))
        {
            return;
        }

        touchingGrounds.Add(collision);

        if (playerMove.showCollisionDebug)
        {
            Debug.Log(
                $"[Foot] Enter ground: {collision.name}, " +
                $"layer: {LayerMask.LayerToName(collision.gameObject.layer)}, " +
                $"is current layer: {playerMove.IsCurrentGroundLayer(collision.gameObject.layer)}");
        }

        RefreshGroundedState();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerMove == null)
        {
            return;
        }

        if (playerMove.showCollisionDebug)
        {
            Debug.Log(
                $"[Foot] OnTriggerExit2D with {collision.name}, tag: {collision.tag}, isTrigger: {collision.isTrigger}");
        }

        if (collision.CompareTag("Switch"))
        {
            if (playerMove.showCollisionDebug)
            {
                Debug.Log($"[Foot] Unregister switch: {collision.name}");
            }

            playerMove.UnregisterSwitchTrigger(collision);
            return;
        }

        if (!playerMove.IsGroundLayer(collision.gameObject.layer))
        {
            return;
        }

        touchingGrounds.Remove(collision);

        if (playerMove.showCollisionDebug)
        {
            Debug.Log(
                $"[Foot] Exit ground: {collision.name}, " +
                $"layer: {LayerMask.LayerToName(collision.gameObject.layer)}, " +
                $"is current layer: {playerMove.IsCurrentGroundLayer(collision.gameObject.layer)}");
        }

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
