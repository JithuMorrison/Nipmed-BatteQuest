using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonutScore : MonoBehaviour {

	public Text score;

	void Start () {
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.Save ();
	}

	void Update () {
		score.text = "Donuts Ate: " + PlayerPrefs.GetInt ("PlayerScore").ToString();
	}

}
