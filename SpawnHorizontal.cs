using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHorizontal : MonoBehaviour {

    public GameObject[] objectsToSpawn;
    public Transform spawnPoint;
    public float minSpawnInterval = 1.0f;
    public float maxSpawnInterval = 3.0f;
    private int lastScoreChecked = 0;
    private float heightIncrease = 0.0f;
    public float objectSpeed = 8.0f;

    private void Start()
    {
        StartCoroutine(SpawnObjectsRandomly());
    }

    private void Update(){
        int playerScore = PlayerPrefs.GetInt("PlayerScore");
        if (playerScore > 0 && playerScore % 10 == 0 && playerScore != lastScoreChecked)
        {
            heightIncrease += 0.2f;
            lastScoreChecked = playerScore;
        }
    }

    IEnumerator SpawnObjectsRandomly()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        if (objectsToSpawn.Length > 0 && spawnPoint != null)
        {
            int randomIndex = Random.Range(0, objectsToSpawn.Length);
            GameObject randomObject = objectsToSpawn[randomIndex];
            spawnPoint.position = spawnPoint.position + new Vector3(0, heightIncrease, 0);
            GameObject spawnedObject = Instantiate(randomObject, spawnPoint.position, spawnPoint.rotation);
            MoveSide moveComponent = spawnedObject.AddComponent<MoveSide>();
            moveComponent.speed = objectSpeed;
            moveComponent.spawnIndex = randomIndex; // Assign spawn index to object
        }
        else
        {
            Debug.LogWarning("Objects to spawn array is empty or spawn point is not assigned.");
        }
    }
}

public class MoveSide : MonoBehaviour
{
    public float speed = 5.0f;
    public int spawnIndex; // Store the spawn index

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.x > 20.65f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Face") && collision.gameObject.transform.position.y > -6f)
        {
            int score = PlayerPrefs.GetInt("PlayerScore");
            score += spawnIndex; // Increment score based on spawn index
            PlayerPrefs.SetInt("PlayerScore", score);
            PlayerPrefs.Save();
        }
    }
}
