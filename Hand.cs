using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
	public Transform mHandMesh;
	public AudioClip collisionSound;
	private AudioSource audioSource;
	private int score = 0;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = collisionSound;
		audioSource.volume = 1.0f;
	}

	private void Update()
	{
		mHandMesh.position = Vector3.Lerp(mHandMesh.position, transform.position, Time.deltaTime * 15.0f);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{ 

		if(collision.gameObject.CompareTag("Bubble") || collision.gameObject.CompareTag("RedBubble")){
			Bubble bubble = collision.gameObject.GetComponent<Bubble>();
			StartCoroutine(bubble.Pop());
			score = PlayerPrefs.GetInt("PlayerScore");
			if (!collision.gameObject.CompareTag ("Bubble"))
				score--;
			else {
				score++;
			}

			PlayerPrefs.SetInt("PlayerScore", score);
			PlayerPrefs.Save();

			if (audioSource != null && collisionSound != null)
			{
				audioSource.Play();
			}
		}
		if(collision.gameObject.CompareTag("Bird")){
			Destroy(collision.gameObject);
			score = PlayerPrefs.GetInt("PlayerScore");
			if (!collision.gameObject.CompareTag ("Bird"))
				score--;
			else {
				score++;
			}

			PlayerPrefs.SetInt("PlayerScore", score);
			PlayerPrefs.Save();
		}
	}

}
