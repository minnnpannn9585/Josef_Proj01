using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public Rigidbody2D rb;
    public Collider2D bodyCollider;
    public float moveSpeed = 5;
    public float jumpForce = 300;
    public bool isGrounded = true;
    public bool canMove = true;
    public int playerId = 0;

    [Header("Depth Layer")]
    public bool atBack = true;

    [Header("Debug")]
    public bool showCollisionDebug = true;
    public Color groundedDebugColor = Color.green;
    public Color airborneDebugColor = Color.red;
    

    private int frontGroundLayer = 7;
    private int backGroundLayer = 6;
    private Foot foot;
    public InventoryManager inventory;
    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (bodyCollider == null)
        {
            bodyCollider = GetComponent<CircleCollider2D>();
        }

        foot = GetComponentInChildren<Foot>();
    }

    private void Start()
    {
        inventory = GetComponent<InventoryManager>();
        inventory.AddItem("Key");
        inventory.AddItem("Shovel");
        inventory.AddItem("Potion");
        inventory.AddItem("Coin");
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
            SwitchDepthLayer(true);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SwitchDepthLayer(false);
        }
    }

    public bool IsGroundLayer(int layer)
    {
        return layer == frontGroundLayer || layer == backGroundLayer;
    }

    public bool IsCurrentGroundLayer(int layer)
    {
        return atBack ? layer == backGroundLayer : layer == frontGroundLayer;
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

    private void SwitchDepthLayer(bool targetLayer)
    {
        if (atBack == targetLayer)
        {
            return;
        }
        atBack = targetLayer;
        ApplyGroundCollisionRules();
        RefreshGroundedState();
    }

    private void ApplyGroundCollisionRules()
    {
        if (bodyCollider == null || frontGroundLayer == -1 || backGroundLayer == -1)
        {
            return;
        }
        GameObject[] backObjects = GameObject.FindGameObjectsWithTag("GroundBack");
        GameObject[] frontObjects = GameObject.FindGameObjectsWithTag("GroundFront");
        foreach(GameObject entity in backObjects)
        {
            Collider2D collider2D = entity.GetComponent<Collider2D>();
            if (collider2D == null || collider2D == bodyCollider)
            {
                continue;
            }
            Physics2D.IgnoreCollision(bodyCollider, collider2D, !atBack);
        }
        foreach(GameObject entity in frontObjects)
        {
            Collider2D collider2D = entity.GetComponent<Collider2D>();
            if (collider2D == null || collider2D == bodyCollider)
            {
                continue;
            }
            Physics2D.IgnoreCollision(bodyCollider, collider2D, atBack);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject entity = collision.gameObject;
        if (entity.CompareTag("Coin"))
        {
            inventory.AddItem("Coin",1);
            Destroy(entity);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject entity = collision.gameObject;
        if(entity.name == "Key")
        {
            inventory.AddItem("Key",1);
            Destroy(entity);
        }
        if(entity.name == "Shovel")
        {
            inventory.AddItem("Shovel",1);
            Destroy(entity);
        }
        if (entity.name == "Potion")
        {
            inventory.AddItem("Potion",1);
            Destroy(entity);
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
            $"At Back: {atBack}\n" +
            $"Grounded: {isGrounded}\n" +
            $"Player Id: {playerId}";
        UnityEditor.Handles.Label(top + Vector3.up * 0.15f, debugText);
#endif
    }
}
