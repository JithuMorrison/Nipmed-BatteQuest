using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public GameObject mBubblePrefab;
	public GameObject mRedBubblePrefab;

    private List<Bubble> mAllBubbles = new List<Bubble>();
    private Vector2 mBottomLeft = Vector2.zero;
    private Vector2 mTopRight = Vector2.zero;

    private void Awake()
    {
        // Bounding values
        mBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane));
        mTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, Camera.main.farClipPlane));
    }

    private void Start()
    {
        StartCoroutine(CreateBubbles());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane)), 0.5f);
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.farClipPlane)), 0.5f);
    }

    public Vector3 GetPlanePosition()
    {
        // Random Position
        float targetX = Random.Range(mBottomLeft.x, mTopRight.x);
        float targetY = Random.Range(mBottomLeft.y, mTopRight.y);

        return new Vector3(targetX, targetY, 0);
    }

	private IEnumerator CreateBubbles()
	{
		while (mAllBubbles.Count < 10)
        {
            GameObject newBubbleObject;
            if (Random.Range(0, 2) == 0)  // 50% chance to create regular or red bubble
            {
                newBubbleObject = Instantiate(mBubblePrefab, GetPlanePosition(), Quaternion.identity, transform);
            }
            else
            {
                newBubbleObject = Instantiate(mRedBubblePrefab, GetPlanePosition(), Quaternion.identity, transform);
            }
            Bubble newBubble = newBubbleObject.GetComponent<Bubble>();
            newBubble.mBubbleManager = this;
            mAllBubbles.Add(newBubble);
            yield return new WaitForSeconds(1f);  // Bubbles will now generate every second
        }
	}

}
