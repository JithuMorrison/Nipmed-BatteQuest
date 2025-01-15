using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepScore : MonoBehaviour
{
    public Text score;
    public int count;
    public int prev;
    private bool soundPlayed;  // New flag to ensure sound is played only once

    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    private AudioSource audioSource;
    public AudioClip activationSound;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        object1.SetActive(false);
        object2.SetActive(false);
        object3.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = activationSound;
        audioSource.volume = 1.0f;

        soundPlayed = false;  // Initialize the flag to false
    }

    void Update()
    {
        score.text = "Score: " + PlayerPrefs.GetInt("PlayerScore").ToString();

        if (prev != PlayerPrefs.GetInt("PlayerScore"))
        {
            count++;
            prev = PlayerPrefs.GetInt("PlayerScore");
        }

        if (count == 10 || count == -1)
        {
            object1.SetActive(true);
            object2.SetActive(true);
            object3.SetActive(true);

            // Check if the sound has not been played yet
            if (audioSource != null && activationSound != null && !audioSource.isPlaying && !soundPlayed)
            {
                audioSource.Play();
                soundPlayed = true;
            }

            count = -1;
        }
        else
        {
            object1.SetActive(false);
            object2.SetActive(false);
            object3.SetActive(false);

            if (soundPlayed) 
            {
                soundPlayed = false;  // Reset the flag when objects are not active
            }
        }
    }
}
