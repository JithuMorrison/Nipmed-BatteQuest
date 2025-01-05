using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour 
{
    public float speed = 2.0f; // Movement speed
    public bool startMovingRight = true; // Choose the initial direction
    private bool movingRight; // Internal direction tracker
    private Vector3 initialPosition; // Tracks the initial position for distance calculation
    private float traveledDistance = 0f; // Tracks the distance traveled

    private Animator animator; // Reference to Animator component

    private void Start()
    {
        // Set the initial direction
        movingRight = startMovingRight;

        // Store the initial position
        initialPosition = transform.position;

        // Get the Animator component (if it exists)
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Move the object
        float moveDirection = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);

        // Update the traveled distance
        traveledDistance += speed * Time.deltaTime;

        // Check if the object has moved 12 units
        if (traveledDistance >= 12f)
        {
            // Change direction
            movingRight = !movingRight;

            // Reset traveled distance
            traveledDistance = 0f;
        }

        // Flip the object's sprite
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * (movingRight ? 1 : -1);
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Detected with: " + collision.gameObject.name);

        if (collision.CompareTag("Face"))
        {
            if (animator != null)
            {
                animator.SetBool("burst", true);
            }
        }
    }
}
