using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutSwing : MonoBehaviour {

	public Transform anchorPoint;  // The point from which the donut is hanging
    public float maxRopeLength = 3f;  // Maximum rope length
    public float minRopeLength = 2f;  // Minimum rope length
    public float swingSpeed = 2f;  // Speed of the swing
    public float ropeChangeSpeed = 0.5f;  // Speed at which the rope length changes
    public float maxSwingAngle = 90f; // Maximum swing angle (90 degrees for 180 total degrees)

    private float currentRopeLength;
    private float swingAngle = 0f;
    private bool ropeLengthIncreasing = true;

    private LineRenderer lineRenderer;  // LineRenderer to simulate the rope
    private SpriteRenderer donutRenderer;  // To hide/show the donut
    private Collider2D donutCollider;  // To enable/disable collision
    private bool isHidden = false;  // To check if donut is hidden

    void Start()
    {
        currentRopeLength = maxRopeLength;  // Start with the maximum rope length

        // Fetch components
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Start and end points of the rope
        lineRenderer.startWidth = 0.05f; // Start thickness of the rope
        lineRenderer.endWidth = 0.05f;   // End thickness of the rope

        donutRenderer = GetComponent<SpriteRenderer>();
        donutCollider = GetComponent<Collider2D>();

        // Debug: Check if the components are properly assigned
        Debug.Log("Donut Renderer assigned: " + (donutRenderer != null));
        Debug.Log("Donut Collider assigned: " + (donutCollider != null));
    }

    void Update()
    {
        if (!isHidden)
        {
            Swing();
            UpdateRopeLength();
        }
        UpdateRope();
    }

    void Swing()
    {
        swingAngle = Mathf.Sin(Time.time * swingSpeed) * maxSwingAngle;
        float swingX = Mathf.Sin(swingAngle * Mathf.Deg2Rad) * currentRopeLength;  // X position based on angle
        float swingY = Mathf.Cos(swingAngle * Mathf.Deg2Rad) * currentRopeLength;  // Y position based on angle
        transform.position = new Vector3(anchorPoint.position.x + swingX, anchorPoint.position.y - swingY, transform.position.z);
    }

    void UpdateRopeLength()
    {
        if (ropeLengthIncreasing)
        {
            currentRopeLength += Time.deltaTime * ropeChangeSpeed;
            if (currentRopeLength >= maxRopeLength)
                ropeLengthIncreasing = false;
        }
        else
        {
            currentRopeLength -= Time.deltaTime * ropeChangeSpeed;
            if (currentRopeLength <= minRopeLength)
                ropeLengthIncreasing = true;
        }
    }

    void UpdateRope()
    {
        lineRenderer.SetPosition(0, anchorPoint.position);  // Rope start (anchor)
        lineRenderer.SetPosition(1, transform.position);    // Rope end (donut)
    }

    // Use OnTriggerEnter2D for trigger detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debugging the collision detection
        Debug.Log("Trigger Detected with: " + collision.gameObject.name);

        // Check if the donut is not already hidden
        if (!isHidden)  
        {
            // Hide the donut upon collision
            HideDonut();
        }
    }

    private void HideDonut()
    {
        isHidden = true;

        // Ensure components are not null before using them
        if (donutRenderer != null)
        {
            donutRenderer.enabled = false;  // Hide the donut's sprite
        }
        else
        {
            Debug.LogError("Donut Renderer is null!");
        }

        if (donutCollider != null)
        {
            donutCollider.enabled = false;  // Disable collision
        }
        else
        {
            Debug.LogError("Donut Collider is null!");
        }

        Debug.Log("Donut Hidden");

        StartCoroutine(ReappearDonut());
    }

    private IEnumerator ReappearDonut()
    {
        yield return new WaitForSeconds(10f);  // Wait for 10 seconds

        // Show the donut's sprite and enable collision
        if (donutRenderer != null)
        {
            donutRenderer.enabled = true;
        }

        if (donutCollider != null)
        {
            donutCollider.enabled = true;
        }

        isHidden = false;

        Debug.Log("Donut Reappeared");
    }
}
