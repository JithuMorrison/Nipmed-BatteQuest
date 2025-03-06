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
    public Sprite highlightSprite; // Image to highlight the donut

    private float currentRopeLength;
    private float swingAngle = 0f;
    private bool ropeLengthIncreasing = true;
    private bool isSwinging = true;

    private LineRenderer lineRenderer;
    private SpriteRenderer donutRenderer;
    private Collider2D donutCollider;
    private SpriteRenderer highlightRenderer;

    private bool isHidden = false;

    void Start()
    {
        currentRopeLength = maxRopeLength;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        donutRenderer = GetComponent<SpriteRenderer>();
        donutCollider = GetComponent<Collider2D>();

        // Create the highlight effect
        GameObject highlight = new GameObject("Highlight");
        highlight.transform.parent = transform;
        highlight.transform.localPosition = Vector3.zero;

        highlightRenderer = highlight.AddComponent<SpriteRenderer>();
        highlightRenderer.sprite = highlightSprite;
        highlightRenderer.sortingOrder = donutRenderer.sortingOrder - 1;
        highlightRenderer.color = new Color(1f, 1f, 1f, 0f);  // Initially transparent
    }

    void Update()
    {
        if (!isHidden && isSwinging)
        {
            Swing();
            UpdateRopeLength();
            CheckForMiddleStop();
        }
        UpdateRope();
    }

    void Swing()
    {
        swingAngle = Mathf.Sin(Time.time * swingSpeed) * maxSwingAngle;
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
            if (isSwinging)
            {
                StopSwingForDuration(2f); // Stop for 5 seconds
            }
        }
    }

    private void StopSwingForDuration(float duration)
    {
        isSwinging = false;
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

    private IEnumerator FadeHighlight(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            highlightRenderer.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        highlightRenderer.color = new Color(1f, 1f, 1f, endAlpha);
    }
}
