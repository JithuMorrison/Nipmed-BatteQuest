using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeight : MonoBehaviour
{
    public GameObject targetObject; // The GameObject whose position will be changed
    public float newXPosition = 1f; // The new X-axis position for the object

    void OnMouseDown()
    {
		Debug.Log("haha");
        if (targetObject != null)
        {
            Vector3 currentPosition = targetObject.transform.position;
            targetObject.transform.position = new Vector3(currentPosition.x, currentPosition.y-newXPosition, currentPosition.z);
        }
    }
}
