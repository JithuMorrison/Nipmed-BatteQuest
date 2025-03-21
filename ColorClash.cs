using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorClash : MonoBehaviour
{
    public Transform[] spawnPoints;       // Array of spawn points for the buttons
    public GameObject[] colorPrefabs;     // Array of prefabs for the colors
    public Text countdownText;            // UI Text for countdown
    public Text correctColorText;         // UI Text for the correct color name
    public AudioSource audioSource;       // Audio source to play sounds
    public AudioClip yayClip;             // Audio clip for "Yay"
    public AudioClip booClip;             // Audio clip for "Boo"
    public AudioClip[] colorAudioClips;   // Audio clips for each color name (length = 6)
    public GameObject highlightPrefab;
    public GameObject backgroundOverlay; // Assign in Unity Inspector
    public Transform correctColorLoc;

    private string[] colorNames = { "Lion", "Deer", "Cat", "Dog", "Koala", "Parrot" };
    private string correctColor;
    private float timer = 10f;
	private string triggeredColor;
    private GameObject guess;

    void Start()
    {
        RestartGame();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(timer).ToString();
        }
        else
        {
            PlaySound(booClip);
            RestartGame();
        }
    }

	public void UpdateTriggeredColor(string colorName)
    {
        triggeredColor = colorName;

        if (!string.IsNullOrEmpty(triggeredColor))
        {
            if (triggeredColor == correctColor)
            {
                Debug.Log("Correct! Yay!");
                PlaySound(yayClip);
            }
            else
            {
                Debug.Log("Wrong! Boo!");
                PlaySound(booClip);
            }
            StartCoroutine(ExecuteAfterDelay(10.0f));
        }
    }

    IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("10 seconds have passed!");
    }

    void RestartGame()
    {
        timer = 10f;

        // Clear existing buttons at spawn points
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.childCount > 0)
            {
                Destroy(spawnPoint.GetChild(0).gameObject);
            }
        }
        if (correctColorLoc.childCount > 0)
        {
            Destroy(correctColorLoc.GetChild(0).gameObject);
        }
        // Shuffle colors
        List<int> indices = new List<int>();
        for (int i = 0; i < colorPrefabs.Length; i++) indices.Add(i);
        Shuffle(indices);

        GameObject correctObject = null;
        int correctIndex = Random.Range(0, spawnPoints.Length);
        
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            GameObject button = Instantiate(colorPrefabs[indices[i]], spawnPoint);
            button.transform.localPosition = Vector3.zero;

            // Pick the correct color object
            if (i == correctIndex)
            {
                correctObject = button;
                correctColor = colorNames[indices[i]];
                guess = Instantiate(colorPrefabs[indices[correctIndex]],correctColorLoc);
                guess.transform.localPosition = Vector3.zero;
            }
        }

        // Show the correct color visually
        correctColorText.text = "Click: " + correctColor; // Hide text, since we highlight the object instead

        // Highlight the correct object with a red circle
        if (correctObject != null)
        {
            StartCoroutine(HighlightCorrectObject(correctObject));
        }

        // Dim the background
        StartCoroutine(DimBackground());
    }

    IEnumerator DimBackground()
    {
        if (backgroundOverlay == null)
        {
            Debug.LogError("Background Overlay is not assigned in the Inspector!");
            yield break;
        }

        backgroundOverlay.SetActive(true); // Enable the dark overlay

        yield return new WaitForSeconds(3f);

        backgroundOverlay.SetActive(false); // Hide the overlay after time expires
    }

    IEnumerator HighlightCorrectObject(GameObject obj)
    {
        if (highlightPrefab == null)
        {
            Debug.LogError("Highlight Prefab is not assigned in the Inspector!");
            yield break;
        }

        // Instantiate the highlight effect as a child of the correct object
        GameObject highlight = Instantiate(highlightPrefab, obj.transform);
        
        highlight.transform.localPosition = Vector3.zero; // Center it
        highlight.transform.localRotation = Quaternion.identity; // Reset rotation
        highlight.transform.localScale = Vector3.one * 0.24f; // Fixed scale, independent of the object size

        yield return new WaitForSeconds(3f);

        Destroy(highlight); // Remove after 5 seconds
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
