using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class BamseSpawner : MonoBehaviour
{
    [Header("Prefab af bamsen")]
    public GameObject bamsePrefab;

    [Header("Reference til VR-spillerens hoved/kamera")]
    public Transform playerHead;

    [Header("Offset foran spilleren")]
    public float spawnDistance = 1.5f;
    public Vector3 verticalOffset = new Vector3(0, 0.2f, 0);

    [Header("Cooldown i sekunder")]
    public float cooldownTime = 5f;

    private bool canSpawn = true;


    public void SpawnBamseInFrontOfPlayer()
    {
        if (!canSpawn || bamsePrefab == null || playerHead == null) return;

        Vector3 forwardDirection = playerHead.forward;
        Vector3 spawnPosition = playerHead.position + forwardDirection * spawnDistance + verticalOffset;

        Instantiate(bamsePrefab, spawnPosition, Quaternion.identity);

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(cooldownTime);
        canSpawn = true;
    }
}