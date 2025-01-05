using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepScore : MonoBehaviour {

	public Text score;
	public int count;
	public int prev;

	public GameObject object1;
    public GameObject object2;
    public GameObject object3;

	void Start () {
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.Save ();
		object1.SetActive(false);
        object2.SetActive(false);
        object3.SetActive(false);
	}

	void Update () {
		score.text = "Score: " + PlayerPrefs.GetInt ("PlayerScore").ToString();
		if(prev!=PlayerPrefs.GetInt ("PlayerScore")){
			count++;
			prev=PlayerPrefs.GetInt ("PlayerScore");
		}
		if(count==10 || count==-1){
			object1.SetActive(true);
            object2.SetActive(true);
            object3.SetActive(true);
			count=-1;
		}
		else{
			object1.SetActive(false);
			object2.SetActive(false);
			object3.SetActive(false);
		}

	}
}
