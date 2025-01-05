using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public float spawnInterval = 3.0f;
    public Transform spawnPoint;
    public float birdSpeed = 3.0f;

    private void Start()
    {
        InvokeRepeating("SpawnBird", 0f, spawnInterval);
    }

    private void SpawnBird()
    {
        GameObject bird = Instantiate(birdPrefab, spawnPoint.position, Quaternion.identity);

        // Assign movement and attack behavior to the bird
        Bird birdScript = bird.GetComponent<Bird>();
        if (birdScript != null)
        {
            birdScript.Initialize(birdSpeed, Random.value > 0.5f); // Randomly decide whether the bird will attack
        }
    }
}
