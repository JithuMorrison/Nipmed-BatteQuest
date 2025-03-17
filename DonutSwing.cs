using System.Collections;
using UnityEngine;

public class DonutSwing : MonoBehaviour
{
    public Transform anchorPoint;  // The point from which the donut is hanging
    public float maxRopeLength = 3f;  // Maximum rope length
    public float minRopeLength = 2f;  // Minimum rope length
    public float swingSpeed = 2f;  // Speed of the swing
    public float ropeChangeSpeed = 0.5f;  // Speed at which the rope length changes
    public float maxSwingAngle = 90f; // Maximum swing angle
    public int requiredSwingCondition = 1; // The value that must match to allow swinging

    private float currentRopeLength;
    private float swingAngle = 0f;
    private bool ropeLengthIncreasing = true;
    private bool isSwinging = true;

    private LineRenderer lineRenderer;
    private SpriteRenderer donutRenderer;
    private Collider2D donutCollider;

    private bool isHidden = false;
    private float lastStopTime = -7f; // Ensures the first stop can happen immediately
    private float pauseTime; // Time when the swing was paused
    private float pausedSwingAngle; // Swing angle when the swing was paused

    private DonutSwingController swingController; // Reference to the DonutSwingController

    void Start()
    {
        currentRopeLength = maxRopeLength;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        donutRenderer = GetComponent<SpriteRenderer>();
        donutCollider = GetComponent<Collider2D>();

        // Find the DonutSwingController in the scene
        swingController = FindObjectOfType<DonutSwingController>();
        if (swingController == null)
        {
            Debug.LogError("DonutSwingController not found in the scene!");
        }
    }

    void Update()
    {
        // Check if the swing condition is met
        if (swingController != null && swingController.swingCondition == requiredSwingCondition)
        {
            if (!isHidden && isSwinging)
            {
                Swing();
                UpdateRopeLength();
                CheckForMiddleStop();
            }
            UpdateRope();
        }
    }

    void Swing()
    {
        // Calculate the swing angle based on time and speed
        swingAngle = Mathf.Sin((Time.time - pauseTime) * swingSpeed) * maxSwingAngle + pausedSwingAngle;
        float swingX = Mathf.Sin(swingAngle * Mathf.Deg2Rad) * currentRopeLength;
        float swingY = Mathf.Cos(swingAngle * Mathf.Deg2Rad) * currentRopeLength;
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
        lineRenderer.SetPosition(0, anchorPoint.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    void CheckForMiddleStop()
    {
        // Check if the donut is near the middle of the screen
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        float middleThreshold = 0.12f; // Adjust this value to control how close to the middle is considered "near"

        if (Mathf.Abs(screenPos.x - 0.5f) < middleThreshold && Mathf.Abs(screenPos.y - 0.5f) < middleThreshold)
        {
            if (isSwinging && Time.time - lastStopTime >= 7f)
            {
                StopSwingForDuration(4f); // Stop for 4 seconds
                lastStopTime = Time.time;
            }
        }
    }

    private void StopSwingForDuration(float duration)
    {
        isSwinging = false;
        pausedSwingAngle = swingAngle; // Save the current swing angle
        pauseTime = Time.time; // Save the time when the swing was paused
        StartCoroutine(ResumeSwingAfterDelay(duration));
    }

    private IEnumerator ResumeSwingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSwinging = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Face"))
        {
            if (!isHidden)
            {
                HideDonut();
                if(swingController.swingCondition == 0){
                    swingController.swingCondition = 1;
                }
                else{
                    swingController.swingCondition = 0;
                }
            }
        }
    }

    private void HideDonut()
    {
        isHidden = true;
        if (donutRenderer != null) donutRenderer.enabled = false;
        if (donutCollider != null) donutCollider.enabled = false;
        Debug.Log("Donut Hidden");

        StartCoroutine(ReappearDonut());
    }

    private IEnumerator ReappearDonut()
    {
        yield return new WaitForSeconds(10f);
        if (donutRenderer != null) donutRenderer.enabled = true;
        if (donutCollider != null) donutCollider.enabled = true;
        isHidden = false;
        Debug.Log("Donut Reappeared");
    }
}
