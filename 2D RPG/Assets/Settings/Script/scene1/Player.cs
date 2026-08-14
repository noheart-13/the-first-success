using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 4f;
    public float jumpForce = 5f;
    /// <summary>总跳跃次数：2 表示一次起跳加一次二段跳，落地后重置。</summary>
    public int maxJumps = 2;

    private readonly HashSet<Collider2D> groundColliders = new();
    private Rigidbody2D rb;
    private int jumpsRemaining;
    private float horizontalInput;
    private bool jumpQueued;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = 0;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpsRemaining > 0)
        {
            jumpsRemaining--;
            jumpQueued = true;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

        if (jumpQueued)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpQueued = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateGroundContact(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        UpdateGroundContact(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        groundColliders.Remove(collision.collider);
    }

    private void UpdateGroundContact(Collision2D collision)
    {
        bool hasGroundContact = false;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.6f)
            {
                hasGroundContact = true;
                break;
            }
        }

        if (hasGroundContact)
        {
            if (groundColliders.Add(collision.collider))
            {
                jumpsRemaining = maxJumps;
            }
        }
        else
        {
            groundColliders.Remove(collision.collider);
        }
    }
}
