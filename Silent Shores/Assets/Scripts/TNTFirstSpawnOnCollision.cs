using UnityEngine;

public class SpawnFirstTNTOnCollision : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform[] spawnLocations; // Assign 3 spawn points in the Inspector

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (spawnLocations.Length == 0 || prefabToSpawn == null)
            {
                Debug.LogWarning("Spawn locations or prefab not set.");
                return;
            }

            // Pick a random spawn point
            int index = Random.Range(0, spawnLocations.Length);
            Transform spawnPoint = spawnLocations[index];

            // Spawn the prefab
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            // Destroy this game object
            Destroy(gameObject);
        }
    }
}
