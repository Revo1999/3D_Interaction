using UnityEngine;
using System.Collections;

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
    public GameObject spawnPrefab;                        // Prefab to spawn
    public Transform[] spawnLocations = new Transform[3]; // 3 spawn locations

    public void Explode()
    {
        StartCoroutine(ExplosionSequence());
    }

    private IEnumerator ExplosionSequence()
    {
        // 1. Play fuse sound
        if (fuseAudioSource != null)
            fuseAudioSource.Play();

        // 2. Wait during fuse
        yield return new WaitForSeconds(igniteDuration);

        // 3. Stop fuse sound
        if (fuseAudioSource != null)
            fuseAudioSource.Stop();

        // 4. Play explosion sound
        if (explosionAudioSource != null)
            explosionAudioSource.Play();

        // 5. Play explosion particles
        if (explosionParticles1 != null)
            explosionParticles1.Play();

        if (explosionParticles2 != null)
            explosionParticles2.Play();

        // 6. Wait before continuing
        yield return new WaitForSeconds(delayBeforeDestroy);

        // 7. Attempt to destroy the object tagged "TNTCaveEntryBush"
        GameObject bush = GameObject.FindWithTag("TNTCaveEntryBush");
        if (bush != null)
        {
            Destroy(bush);
        }

        // 8. Spawn prefab at one of the three locations
        if (spawnPrefab != null && spawnLocations.Length > 0)
        {
            int index = Random.Range(0, spawnLocations.Length);
            Transform spawnPoint = spawnLocations[index];
            if (spawnPoint != null)
            {
                Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }

        // 9. Destroy the TNT object
        Destroy(gameObject);
    }
}
