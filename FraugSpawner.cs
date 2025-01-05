using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FraugSpawner : MonoBehaviour {
	public GameObject[] headPrefabs;   // Array of head prefabs to spawn
    public Transform spawnPoint;      // Single spawn point
    public float spawnInterval = 2f;  // Time interval between spawns

    void Start()
    {
        StartCoroutine(SpawnHeads());
    }

    IEnumerator SpawnHeads()
    {
        while (true)
        {
            // Randomly pick a prefab from the available options
            GameObject headPrefab = headPrefabs[Random.Range(0, headPrefabs.Length)];

            // Spawn the chosen prefab at the single spawn point
            GameObject head = Instantiate(headPrefab, spawnPoint.position, Quaternion.identity);

            // Destroy the object after 10 seconds
            Destroy(head, 10f);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
