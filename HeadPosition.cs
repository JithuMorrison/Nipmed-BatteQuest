using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeadPosition : MonoBehaviour {

	public string sceneToLoad;
	public Text score;
	private bool isTouch;
	private int count=4;
	private bool isActionInProgress = false;

	void Start () {
		count=4;
	}
	
	void Update () {
		if (isTouch && !isActionInProgress)
		{
			StartCoroutine(DelayedAction());
		}
		else if (!isTouch)
		{
			score.text = "Start";
		}
	}

	IEnumerator DelayedAction()
	{
		isActionInProgress = true;
		yield return new WaitForSeconds(1f);
		count--;
		if(count==0)
		{
			LoadScene(sceneToLoad);
		}
		score.text = "  " + count.ToString();
		isActionInProgress = false;
	}


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Face"))
        {
			isTouch=true;
        }
    }

	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Face"))
		{
			count=4;
			isTouch=false;
		}
	}

    void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty or not assigned!");
        }
    }
}
