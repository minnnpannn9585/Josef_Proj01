using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum DepthLayer
    {
        Front,
        Back
    }

    public Rigidbody2D rb;
    public Collider2D bodyCollider;
    public float moveSpeed;
    public float jumpForce;
    public bool isGrounded = false;
    public bool canMove = true;
    public int playerId = 0;

    [Header("Depth Layer")]
    public DepthLayer currentDepthLayer = DepthLayer.Front;
    public string frontGroundLayerName = "GroundFront";
    public string backGroundLayerName = "GroundBack";

    [Header("Debug")]
    public bool showCollisionDebug = true;
    public Color groundedDebugColor = Color.green;
    public Color airborneDebugColor = Color.red;

    private int frontGroundLayer = -1;
    private int backGroundLayer = -1;
    private Foot foot;
    private readonly HashSet<Collider2D> activeSwitchTriggers = new HashSet<Collider2D>();

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (bodyCollider == null)
        {
            bodyCollider = GetComponent<Collider2D>();
        }

        foot = GetComponentInChildren<Foot>();

        frontGroundLayer = LayerMask.NameToLayer(frontGroundLayerName);
        backGroundLayer = LayerMask.NameToLayer(backGroundLayerName);

        if (frontGroundLayer == -1 || backGroundLayer == -1)
        {
            Debug.LogError("请先在 Tags and Layers 里添加 GroundFront 和 GroundBack 两个 layer。");
        }
    }

    private void Start()
    {
        ApplyGroundCollisionRules();
        RefreshGroundedState();
    }

    private void Update()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            TrySwitchDepthLayer(DepthLayer.Back);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TrySwitchDepthLayer(DepthLayer.Front);
        }
    }

    public bool IsGroundLayer(int layer)
    {
        return layer == frontGroundLayer || layer == backGroundLayer;
    }

    public bool IsCurrentGroundLayer(int layer)
    {
        if (currentDepthLayer == DepthLayer.Front)
        {
            return layer == frontGroundLayer;
        }

        return layer == backGroundLayer;
    }

    public string GetCurrentGroundLayerName()
    {
        int layer = GetGroundLayerByDepth(currentDepthLayer);
        return layer == -1 ? "Invalid" : LayerMask.LayerToName(layer);
    }

    public void RefreshGroundedState()
    {
        if (foot != null)
        {
            foot.RefreshGroundedState();
        }
        else
        {
            isGrounded = false;
        }
    }

    private void TrySwitchDepthLayer(DepthLayer targetLayer)
    {
        if (currentDepthLayer == targetLayer)
        {
            return;
        }

        if (GetGroundLayerByDepth(targetLayer) == -1)
        {
            return;
        }

        if (!CanSwitchLayers())
        {
            return;
        }

        currentDepthLayer = targetLayer;
        ApplyGroundCollisionRules();
        RefreshGroundedState();

        if (showCollisionDebug)
        {
            Debug.Log($"[PlayerMove] Switched to {currentDepthLayer}, active ground layer: {GetCurrentGroundLayerName()}");
        }
    }

    public void RegisterSwitchTrigger(Collider2D switchTrigger)
    {
        if (switchTrigger == null)
        {
            if (showCollisionDebug)
            {
                Debug.Log("[PlayerMove] RegisterSwitchTrigger failed: collider is null.");
            }

            return;
        }

        if (!switchTrigger.CompareTag("Switch"))
        {
            if (showCollisionDebug)
            {
                Debug.Log(
                    $"[PlayerMove] RegisterSwitchTrigger ignored: {switchTrigger.name} tag is '{switchTrigger.tag}', not 'Switch'.");//修复了日志不一致的问题
            }

            return;
        }

        activeSwitchTriggers.Add(switchTrigger);

        if (showCollisionDebug)
        {
            Debug.Log(
                $"[PlayerMove] Switch registered: {switchTrigger.name}, active switch count: {activeSwitchTriggers.Count}");
        }
    }

    public void UnregisterSwitchTrigger(Collider2D switchTrigger)
    {
        if (switchTrigger == null)
        {
            return;
        }

        bool removed = activeSwitchTriggers.Remove(switchTrigger);

        if (showCollisionDebug)
        {
            Debug.Log(
                $"[PlayerMove] Switch unregistered: {switchTrigger.name}, removed: {removed}, active switch count: {activeSwitchTriggers.Count}");
        }
    }

    private bool CanSwitchLayers()
    {
        activeSwitchTriggers.RemoveWhere(collider => collider == null);

        if (showCollisionDebug)
        {
            Debug.Log($"[PlayerMove] CanSwitchLayers check, active switch count: {activeSwitchTriggers.Count}");
        }

        return activeSwitchTriggers.Count > 0;
    }

    private int GetGroundLayerByDepth(DepthLayer depthLayer)
    {
        if (depthLayer == DepthLayer.Front)
        {
            return frontGroundLayer;
        }

        return backGroundLayer;
    }

    private void ApplyGroundCollisionRules()
    {
        if (bodyCollider == null || frontGroundLayer == -1 || backGroundLayer == -1)
        {
            return;
        }

        Collider2D[] allColliders = FindObjectsOfType<Collider2D>();
        int activeGroundLayer = GetGroundLayerByDepth(currentDepthLayer);

        foreach (Collider2D collider2D in allColliders)
        {
            if (collider2D == null || collider2D == bodyCollider || collider2D.isTrigger)
            {
                continue;
            }

            if (!IsGroundLayer(collider2D.gameObject.layer))
            {
                continue;
            }

            bool shouldIgnore = collider2D.gameObject.layer != activeGroundLayer;
            Physics2D.IgnoreCollision(bodyCollider, collider2D, shouldIgnore);

            if (showCollisionDebug)
            {
                Debug.Log(
                    $"[PlayerMove] {(shouldIgnore ? "Ignore" : "Collide")} with {collider2D.name}, " +
                    $"layer: {LayerMask.LayerToName(collider2D.gameObject.layer)}");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (!showCollisionDebug)
        {
            return;
        }

        Color debugColor = isGrounded ? groundedDebugColor : airborneDebugColor;
        Vector3 origin = transform.position;
        Vector3 top = origin + Vector3.up * 1.5f;
        Vector3 right = origin + Vector3.right * 0.8f;

        Gizmos.color = debugColor;
        Gizmos.DrawLine(origin, top);
        Gizmos.DrawSphere(top, 0.08f);
        Gizmos.DrawLine(origin, right);

#if UNITY_EDITOR
        string debugText =
            $"Depth: {currentDepthLayer}\n" +
            $"Ground Layer: {GetCurrentGroundLayerName()}\n" +
            $"Grounded: {isGrounded}\n" +
            $"Player Id: {playerId}";
        UnityEditor.Handles.Label(top + Vector3.up * 0.15f, debugText);
#endif
    }
}
