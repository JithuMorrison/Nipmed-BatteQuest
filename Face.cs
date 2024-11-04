using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Face : MonoBehaviour {

	public Transform mFaceMesh;
	public AudioClip collisionSound; // Reference to the audio clip
	private AudioSource audioSource; // Reference to the AudioSource component
	private int score = 0;
	// Use this for initialization
	void Start () {
		// Get the AudioSource component from this GameObject or its children
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();
		// Set the audio clip
		audioSource.clip = collisionSound;
		// Set volume to full
		audioSource.volume = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		mFaceMesh.position = Vector3.Lerp(mFaceMesh.position, transform.position, Time.deltaTime * 15.0f);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{ 

		if(collision.gameObject.CompareTag("Bubble") || collision.gameObject.CompareTag("Untagged")){
			Bubble bubble = collision.gameObject.GetComponent<Bubble>();
			StartCoroutine(bubble.Pop());

			if (!collision.gameObject.CompareTag ("Bubble"))
				score--;
			else {
				score++;
			}

			PlayerPrefs.SetInt("PlayerScore", score);
			PlayerPrefs.Save();
			Debug.Log(PlayerPrefs.GetInt("PlayerScore"));

			if (audioSource != null && collisionSound != null)
			{
				audioSource.Play();
			}
		}
	}
}
