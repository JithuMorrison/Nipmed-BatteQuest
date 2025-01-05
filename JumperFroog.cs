using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperFroog : MonoBehaviour {
	public float moveSpeed = 5f;        // Speed of forward movement
    public float jumpForce = 5f;       // Jump force
    public float detectionRange = 2f;  // Range to detect objects with the "head" tag

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Face"))
            {
                Jump();
                return;
            }
        }
    }

    void Jump()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.01f) // Ensure the player is grounded
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // Optional: Draw the detection range in the scene view for debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
