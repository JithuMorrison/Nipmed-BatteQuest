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

    private string[] colorNames = { "Green", "Red", "Blue", "Yellow", "Purple", "Black" };
    private string correctColor;
    private float timer = 10f;
	private string triggeredColor;

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
        timer = 5f;

        // Clear existing buttons at spawn points
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.childCount > 0)
            {
                Destroy(spawnPoint.GetChild(0).gameObject);
            }
        }

        // Shuffle colors
        List<int> indices = new List<int>();
        for (int i = 0; i < colorPrefabs.Length; i++) indices.Add(i);
        Shuffle(indices);

        // Spawn buttons at spawn points
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            GameObject button = Instantiate(colorPrefabs[indices[i]], spawnPoint);
            button.transform.localPosition = Vector3.zero;
        }

        // Pick a random correct color
        int correctIndex = indices[Random.Range(0, indices.Count)];
        correctColor = colorNames[correctIndex];
        correctColorText.text = "Click: " + correctColor;
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
