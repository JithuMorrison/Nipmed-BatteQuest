using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScore : MonoBehaviour {

	public Text score;

	void Start () {
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.Save ();
	}

	void Update () {
		score.text = "Bubbles Popped: " + PlayerPrefs.GetInt ("PlayerScore").ToString();
	}
}
