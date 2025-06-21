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

    [Header("Sound Settings")]
    [Tooltip("Array of wind sound clips to play randomly.")]
    public AudioClip[] windSounds;

    [Tooltip("Volume of wind sound (0-1).")]
    [Range(0f, 1f)]
    public float windVolume = 1f;

    // Internal control flag
    private bool isEnabled = false;

    void Start()
    {
        StartCoroutine(SpawnCycle());
    }

    IEnumerator SpawnCycle()
    {
        while (true)
        {
            if (isEnabled)
            {
                for (int i = 0; i < particlesPerCycle; i++)
                {
                    float delay = Random.Range(0f, cycleDuration);
                    StartCoroutine(SpawnAndDestroyWithDelay(delay));
                }
            }

            yield return new WaitForSeconds(cycleDuration);
        }
    }

    IEnumerator SpawnAndDestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isEnabled) yield break;

        GameObject prefab = particlePrefabs[Random.Range(0, particlePrefabs.Length)];

        float offsetX = Random.Range(xRange.x, xRange.y) * windScale;
        float offsetZ = Random.Range(zRange.x, zRange.y) * windScale;
        Vector3 spawnPosition = player.position + new Vector3(offsetX, 0f, offsetZ);
        spawnPosition.y = player.position.y + yOffset;

        Vector3 direction = guide.position - spawnPosition;
        direction.y = 0f;

        Quaternion rotation = Quaternion.identity;
        if (direction.sqrMagnitude > 0.001f)
        {
            rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(rotationOffsetEuler);
        }

        GameObject particle = Instantiate(prefab, spawnPosition, rotation);
        particle.transform.localScale = Vector3.one * windScale;

        if (windSounds != null && windSounds.Length > 0)
        {
            AudioClip clip = windSounds[Random.Range(0, windSounds.Length)];
            AudioSource.PlayClipAtPoint(clip, spawnPosition, windVolume);
        }

        Destroy(particle, particleLifetime);
    }

    /// <summary>
    /// Enables wind particle spawning.
    /// </summary>
    public void EnableWind()
    {
        isEnabled = true;
    }

    /// <summary>
    /// Disables wind particle spawning.
    /// </summary>
    public void DisableWind()
    {
        isEnabled = false;
    }
}
