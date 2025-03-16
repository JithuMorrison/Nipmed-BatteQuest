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

    private int lastMultipleOfTen;  // Track the last multiple of 10 that was crossed

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
        lastMultipleOfTen = 0; // Initialize the last multiple of 10
    }

    void Update()
    {
        score.text = "Score: " + PlayerPrefs.GetInt("PlayerScore").ToString();

        int currentScore = PlayerPrefs.GetInt("PlayerScore");

        // Check if the score has crossed a multiple of 10
        if (currentScore / 10 != lastMultipleOfTen / 10)
        {
            lastMultipleOfTen = (currentScore / 10) * 10;  // Update the last multiple of 10
            ActivateObjects();
        }

        // Reset the sound flag if the objects are not active
        if (!object1.activeSelf && !object2.activeSelf && !object3.activeSelf)
        {
            soundPlayed = false;
        }
    }

    private void ActivateObjects()
    {
        object1.SetActive(true);
        object2.SetActive(true);
        object3.SetActive(true);

        // Play the sound if it hasn't been played yet
        if (audioSource != null && activationSound != null && !audioSource.isPlaying && !soundPlayed)
        {
            audioSource.Play();
            soundPlayed = true;
        }

        // Start the coroutine to deactivate the objects after 5 seconds
        StartCoroutine(DeactivateObjectsAfterDelay(5f));
    }

    private IEnumerator DeactivateObjectsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        object1.SetActive(false);
        object2.SetActive(false);
        object3.SetActive(false);
    }
}
