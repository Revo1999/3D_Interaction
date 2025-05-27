using UnityEngine;
using System.Collections;

public class WindGuide : MonoBehaviour
{

    [Header("Scene References")]
    [Tooltip("The player object around which particles are spawned.")]
    public Transform player;

    [Tooltip("The target object that particles will rotate to face.")]
    public Transform guide;

    [Tooltip("Array of particle prefabs to randomly spawn.")]
    public GameObject[] particlePrefabs;

    [Header("Spawn Area")]
    [Tooltip("Range for random X-axis offset from player.")]
    public Vector2 xRange = new Vector2(-2f, 2f);

    [Tooltip("Range for random Z-axis offset from player.")]
    public Vector2 zRange = new Vector2(-2f, 2f);

    [Tooltip("Vertical offset from player's Y position.")]
    public float yOffset = 0f;

    [Header("Particle Settings")]
    [Tooltip("Scales both spawn distance and particle size.")]
    public float windScale = 1.0f;

    [Tooltip("Rotation smoothing speed for particles facing the guide.")]
    public float turnSpeed = 2f;

    [Tooltip("Rotation offset (Euler angles) applied after facing the guide.")]
    public Vector3 rotationOffsetEuler = Vector3.zero;

    [Header("Timing")]
    [Tooltip("Duration of one full spawn cycle in seconds.")]
    public float cycleDuration = 10f;

    [Tooltip("Number of particles to spawn per cycle.")]
    public int particlesPerCycle = 3;

    [Tooltip("How long each particle lives before being killed muhahaha!!")]
    public float particleLifetime = 4f;

    /// <summary>
    /// Start the coroutine that spawns particles continuously in cycles.
    /// </summary>
    void Start()
    {
        StartCoroutine(SpawnCycle());
    }

    /// <summary>
    /// Coroutine that spawns a number of particles per cycle at random times.
    /// </summary>
    IEnumerator SpawnCycle()
    {
        while (true)
        {
            for (int i = 0; i < particlesPerCycle; i++)
            {
                float delay = Random.Range(0f, cycleDuration);
                StartCoroutine(SpawnAndDestroyWithDelay(delay));
            }

            yield return new WaitForSeconds(cycleDuration);
        }
    }

    /// <summary>
    /// Spawns a particle after a delay and destroys it after a fixed lifetime.
    /// </summary>
    /// <param name="delay">Time in seconds to wait before spawning the particle.</param>
    IEnumerator SpawnAndDestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Select a random particle prefab
        GameObject prefab = particlePrefabs[Random.Range(0, particlePrefabs.Length)];

        // Generate random position offset around the player
        float offsetX = Random.Range(xRange.x, xRange.y) * windScale;
        float offsetZ = Random.Range(zRange.x, zRange.y) * windScale;
        Vector3 spawnPosition = player.position + new Vector3(offsetX, 0f, offsetZ);
        spawnPosition.y = player.position.y + yOffset;

        // Calculate horizontal direction to guide
        Vector3 direction = guide.position - spawnPosition;
        direction.y = 0f;

        // Rotate toward guide + optional rotation offset
        Quaternion rotation = Quaternion.identity;
        if (direction.sqrMagnitude > 0.001f)
        {
            rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(rotationOffsetEuler);
        }

        // Instantiate and scale the particle
        GameObject particle = Instantiate(prefab, spawnPosition, rotation);
        particle.transform.localScale = Vector3.one * windScale;

        // Let Unity handle playing the particle (Play On Awake = true)

        // Schedule destruction
        Destroy(particle, particleLifetime);
    }
}
