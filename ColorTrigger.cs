using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTrigger : MonoBehaviour
{
    public string colorName;
    public ColorClash gameManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameManager != null && other.gameObject.CompareTag("Hand"))
        {
            gameManager.UpdateTriggeredColor(colorName);
        }
    }
}
