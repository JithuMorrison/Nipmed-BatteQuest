using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoBird : MonoBehaviour
{
    public float moveSpeed = 5f;            // Speed at which the bird moves
    public float changeDirectionTime = 3f;  // Time interval to change direction (in seconds)
    public Vector2 movementRange = new Vector2(10f, 10f); // Define the movement range on the X and Y axis
    public float faceDetectRange = 5f;      // Detection range for "Face" objects
    public float handDetectRange = 3f;      // Detection range for "Hand" objects

    private Vector2 direction;  // The current direction the bird is moving in
    private SpriteRenderer spriteRenderer; // Reference to the bird's SpriteRenderer
    private bool isAttacking = false; // Flag to indicate if the bird is in attack mode
    private GameObject faceObject; // The face object the bird is attacking
    private bool isMovingTowardsTarget = false; // Flag to disable random movement when detecting face/hand

    void Start()
    {
        // Get the SpriteRenderer component to flip the bird's sprite
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the random movement with constant change of direction
        StartCoroutine(ChangeDirection());
    }

    void Update()
    {
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            // Check if the bird has moved out of bounds and reset its position to stay within range
            Vector2 position = transform.position;

            // If the bird reaches the left or right edge of the range, change its direction immediately
            if (Mathf.Abs(position.x) > movementRange.x)
            {
                position.x = Mathf.Sign(position.x) * movementRange.x;
                ChangeDirectionImmediately();
            }

            // If the bird reaches the top or bottom edge of the range, change its direction immediately
            if (Mathf.Abs(position.y) > movementRange.y)
            {
                position.y = Mathf.Sign(position.y) * movementRange.y;
                ChangeDirectionImmediately();
            }

            // If the bird's y position is below -2, destroy it
            if (position.y < -0.6f)
            {
                ChangeDirectionY(); // Destroys the bird game object
            }

            transform.position = position;

            // Flip the bird's sprite depending on the direction of movement
            if (direction.x > 0) // Moving right
            {
                spriteRenderer.flipX = false; // Face right
            }
            else if (direction.x < 0) // Moving left
            {
                spriteRenderer.flipX = true; // Face left
            }

        // Detect objects in range
        DetectObjects();
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            // Set a new random direction for the bird
            direction = new Vector2(
                Random.Range(-1f, 1f), // Random direction on the X axis
                Random.Range(-1f, 1f)  // Random direction on the Y axis
            ).normalized; // Normalize to keep the speed consistent

            // Wait for the specified time before changing direction again
            yield return new WaitForSeconds(changeDirectionTime);
        }
    }

    // Method to change direction immediately
    void ChangeDirectionImmediately()
    {
        // Set a new random direction for the bird
        direction = new Vector2(
            Random.Range(-1f, 1f), // Random direction on the X axis
            Random.Range(-1f, 1f)  // Random direction on the Y axis
        ).normalized; // Normalize to keep the speed consistent
    }

    void ChangeDirectionY()
    {
        // Set a new random direction for the bird
        direction = new Vector2(
            Random.Range(-1f, 1f), // Random direction on the X axis
            Random.Range(0.5f, 1f)  // Random direction on the Y axis
        ).normalized; // Normalize to keep the speed consistent
    }

    void DetectObjects()
    {
        // Check for objects tagged as "Face" within faceDetectRange
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, faceDetectRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Face"))
            {
                Debug.Log("Face detected");
                // Stop random movement and move towards the face
                isMovingTowardsTarget = true;

                // Move towards the face if not attacking
                if (!isAttacking)
                {
                    MoveTowardsFace(hit.gameObject);
                }
                // Attack logic if in attack mode
                else
                {
                    Attack(hit.gameObject);
                }
            }
            else if (isAttacking)
            {
                // If no face is detected, move away from the last detected face
                MoveAwayFromFace();
            }

            if (hit.CompareTag("Hand"))
            {
                // Stop random movement and move away from the hand
                isMovingTowardsTarget = true;
                MoveAwayFromHand(hit.gameObject);
            }
        }
    }

    // Move towards the face to attack
    void MoveTowardsFace(GameObject face)
    {
        isAttacking = true;
        faceObject = face;
        direction = (face.transform.position - transform.position).normalized;
        Debug.Log("Moving towards the face to attack!");

        // Start a coroutine to reset the attack state after 5 seconds
        StartCoroutine(ResetAttackState());
    }

    // Attack the face object
    void Attack(GameObject faceObject)
    {
        Debug.Log("Attacking the face object: " + faceObject.name);
        // Implement attack logic here, e.g., damage, trigger animations, etc.
    }

    // Reset the attack state after 5 seconds
    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false; // Reset the attack flag
        Debug.Log("Attack state reset.");
    }

    // Move away from the face when no longer attacking
    void MoveAwayFromFace()
    {
        if (faceObject != null)
        {
            Vector2 directionToMove = (transform.position - faceObject.transform.position).normalized;
            direction = directionToMove; // Update the direction to move away
            Debug.Log("Moving away from the face object.");
        }
    }

    // Move away from the object with the "Hand" tag
    void MoveAwayFromHand(GameObject handObject)
    {
        Vector2 directionToMove = (transform.position - handObject.transform.position).normalized;
        direction = directionToMove; // Update the direction to move away
        Debug.Log("Moving away from the hand object: " + handObject.name);
    }
}
