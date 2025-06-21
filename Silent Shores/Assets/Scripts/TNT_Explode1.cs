using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TNTExplode : MonoBehaviour
{
    [Header("Particle Effects")]
    public ParticleSystem explosionParticles1;
    public ParticleSystem explosionParticles2;

    [Header("Audio")]
    public AudioSource fuseAudioSource;
    public AudioSource explosionAudioSource;

    [Header("Timing")]
    public float igniteDuration = 1f;
    public float delayBeforeDestroy = 2f;

    [Header("Spawn Settings")]
    public GameObject spawnPrefab;

    private List<Transform> dynamicSpawnPoints = new List<Transform>();
    private GameObject player;
    private OVRPlayerController ovrController;
    private bool isInPortExplosionZone = false;

    private void Start()
    {
        AddSpawnPointByTag("TNTSpawn1");
        AddSpawnPointByTag("TNTSpawn2");
        AddSpawnPointByTag("TNTSpawn3");

        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Debug.Log($"Player found: {player.name} at position: {player.transform.position}");
            ovrController = player.GetComponent<OVRPlayerController>();
            if (ovrController != null)
            {
                Debug.Log("OVRPlayerController found on player");
            }
            else
            {
                Debug.LogWarning("OVRPlayerController not found on player!");
            }
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }

        GameObject respawnCheck = GameObject.FindWithTag("TNTRespawnPoint");
        Debug.Log($"TNTRespawnPoint found: {respawnCheck != null}");
        if (respawnCheck != null)
        {
            Debug.Log($"Respawn point position: {respawnCheck.transform.position}");
        }
    }

    private void AddSpawnPointByTag(string tag)
    {
        GameObject obj = GameObject.FindWithTag(tag);
        if (obj != null)
        {
            dynamicSpawnPoints.Add(obj.transform);
            Debug.Log($"Added spawn point: {tag} at position: {obj.transform.position}");
        }
        else
        {
            Debug.LogWarning($"Spawn point with tag {tag} not found.");
        }
    }

    public void Explode()
    {
        Debug.Log("TNT Explode() method called!");
        StartCoroutine(ExplosionSequence());
    }

    private IEnumerator ExplosionSequence()
    {
        Debug.Log("Starting explosion sequence...");

        if (fuseAudioSource != null) fuseAudioSource.Play();
        yield return new WaitForSeconds(igniteDuration);
        if (fuseAudioSource != null) fuseAudioSource.Stop();

        if (explosionAudioSource != null) explosionAudioSource.Play();
        if (explosionParticles1 != null) explosionParticles1.Play();
        if (explosionParticles2 != null) explosionParticles2.Play();

        Debug.Log("=== EXPLOSION DEBUG ===");

        GameObject portObject = GameObject.FindWithTag("DoorLockedWay");
        if (portObject != null)
        {
            if (isInPortExplosionZone)
            {
                Debug.Log("TNT is in DoorExplosionZone - destroying port object!");
                Destroy(portObject);
            }
            else
            {
                Debug.Log("TNT is NOT in DoorExplosionZone - port object remains");
            }
        }
        else
        {
            Debug.Log("Port object not found");
        }

        yield return new WaitForSeconds(delayBeforeDestroy);

        GameObject bush = GameObject.FindWithTag("TNTCaveEntryBush");
        if (bush != null)
        {
            Destroy(bush);
            Debug.Log("Bush destroyed.");
        }
        else
        {
            Debug.Log("Bush not found or already destroyed");
        }

        if (spawnPrefab != null && dynamicSpawnPoints.Count > 0)
        {
            int index = Random.Range(0, dynamicSpawnPoints.Count);
            Transform spawnPoint = dynamicSpawnPoints[index];
            Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"Spawned prefab at position: {spawnPoint.position}");
        }
        else
        {
            Debug.Log($"Cannot spawn prefab - spawnPrefab exists: {spawnPrefab != null}, spawn points count: {dynamicSpawnPoints.Count}");
        }

        Debug.Log("Destroying TNT object");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DoorExplosionZone"))
        {
            Debug.Log("TNT entered DoorExplosionZone trigger");
            isInPortExplosionZone = true;
        }
    }
}
