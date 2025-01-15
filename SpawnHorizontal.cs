using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHorizontal : MonoBehaviour {

	public GameObject[] objectsToSpawn;
    public Transform spawnPoint;
    public float minSpawnInterval = 1.0f;
    public float maxSpawnInterval = 3.0f;
    public float objectSpeed = 5.0f;
    private int lastScoreChecked = 0;

    private void Start()
    {
        StartCoroutine(SpawnObjectsRandomly());
    }

    private void Update(){
        int playerScore = PlayerPrefs.GetInt ("PlayerScore");
        if (playerScore > 0 && playerScore % 10 == 0 && playerScore != lastScoreChecked)
        {
            objectSpeed+=5;
            lastScoreChecked=playerScore;
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
            GameObject spawnedObject = Instantiate(randomObject, spawnPoint.position, spawnPoint.rotation);
            spawnedObject.AddComponent<MoveSide>().speed = objectSpeed;
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

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.x > 20.65f)
        {
            Destroy(gameObject);
        }
    }
}
