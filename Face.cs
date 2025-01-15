using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Face : MonoBehaviour {

	public Transform mFaceMesh;
	public AudioClip collisionSound; // Reference to the audio clip
	private AudioSource audioSource; // Reference to the AudioSource component
	public AudioClip coinClash;
	private int score = 0;
	// Use this for initialization
	void Start () {
		// Get the AudioSource component from this GameObject or its children
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();
		// Set volume to full
		audioSource.volume = 1.0f;
		PlayerPrefs.SetInt("PlayerScore", score);
	}
	
	// Update is called once per frame
	void Update () {
		mFaceMesh.position = Vector3.Lerp(mFaceMesh.position, transform.position, Time.deltaTime * 15.0f);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{ 
		if(collision.gameObject.CompareTag("RedBubble")){
			Bubble bubble = collision.gameObject.GetComponent<Bubble>();
			StartCoroutine(bubble.Pop());
			score = PlayerPrefs.GetInt("PlayerScore");
			if (collision.gameObject.CompareTag ("RedBubble"))
				score--;

			PlayerPrefs.SetInt("PlayerScore", score);
			PlayerPrefs.Save();

			if (audioSource != null && collisionSound != null)
			{
				audioSource.clip = collisionSound;
				audioSource.Play();
			}
		}
		if(collision.gameObject.CompareTag("Donut")){
			score = PlayerPrefs.GetInt("PlayerScore");
			if (collision.gameObject.CompareTag ("Donut"))
				score++;

			PlayerPrefs.SetInt("PlayerScore", score);
			PlayerPrefs.Save();

			if (audioSource != null && collisionSound != null)
			{
				audioSource.clip = collisionSound;
				audioSource.Play();
			}
		}
		if(collision.gameObject.CompareTag("Shark")){
			Destroy(collision.gameObject);
			score = PlayerPrefs.GetInt("PlayerScore");
			if (collision.gameObject.CompareTag ("Shark"))
				score++;

			PlayerPrefs.SetInt("PlayerScore", score);
			PlayerPrefs.Save();

			if (audioSource != null && collisionSound != null)
			{
				audioSource.clip = coinClash;
				audioSource.Play();
			}
		}
	}
}
