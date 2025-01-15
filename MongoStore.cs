using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MongoStore : MonoBehaviour
{
    private List<CollisionData> collisionLogs; // To store collision logs during the game
    private int lastScoreChecked = 0;

    [Header("API Configuration")]
    public string apiUrl = "http://localhost:3000/log"; // The API endpoint to send collision logs

    [Header("Game Configuration")]
    public float gameDuration = 300f; // Duration in seconds (5 minutes)
    public string nextSceneName = "NextScene"; // Name of the scene to load after time is up

    private float timer;

    void Start()
    {
        // Initialize timer
        timer = gameDuration;

        // Initialize the collision log list
        collisionLogs = new List<CollisionData>();

        Debug.Log("Timer started. Listening for collision updates.");
    }

    void Update()
    {
        // Count down the timer
        timer -= Time.deltaTime;

        // If the timer reaches 0, end the game and load the next scene
        if (timer <= 0)
        {
            EndGameAndLoadScene();
        }

        // Get the current score from PlayerPrefs
        int playerScore = PlayerPrefs.GetInt("PlayerScore");

        // If the score has changed (i.e., player has earned a new point), log the time and score
        if (playerScore != lastScoreChecked)
        {
            lastScoreChecked = playerScore;

            // Create the collision log with updated time and score
            var log = new CollisionData
            {
                ObjectName = "PlayerScore", // Placeholder object name for the score update log
                CollisionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Score = playerScore // Store the updated score
            };

            // Add the log to the list
            collisionLogs.Add(log);
            Debug.Log("Score updated: " + log.Score + " at " + log.CollisionTime);
        }
    }

    private void EndGameAndLoadScene()
    {
        Debug.Log("Game duration over. Sending logs to the server and switching scene...");

        // Upload collision data to the server via API
        StartCoroutine(UploadToAPI());

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator UploadToAPI()
    {
        if (collisionLogs.Count == 0)
        {
            Debug.Log("No collision data to upload.");
            yield break;
        }

        foreach (var log in collisionLogs)
        {
            // Create a JSON object to send to the server
            string json = JsonUtility.ToJson(log);

            // Create a POST request to send the data
            using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
            {
                byte[] jsonData = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                // Send the request and wait for a response
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError("Failed to upload log: " + request.error);
                }
                else
                {
                    Debug.Log("Log successfully uploaded: " + log.Score + " at " + log.CollisionTime);
                }
            }
        }
    }
}

// Data structure for storing collision details (including score)
[Serializable]
public class CollisionData
{
    public string ObjectName; // Name of the object (e.g., "PlayerScore" for score updates)
    public string CollisionTime; // Time of the update
    public int Score; // Current score at the time of the update
}
