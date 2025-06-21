using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public class BellSpawner : MonoBehaviour, NetworkListener
{
    [Header("Bell Settings")]
    public GameObject bellPrefab;
    public float cooldownTime = 5f;

    [Tooltip("OSC address to listen for bell position")]
    public string oscAddress = "/Bellpos";

    private bool canSpawn = true;
    private bool isOn = false;
    private Vector3 newPosition;

    private void OnEnable()
    {
        NetworkController.getController().addListener(this);
    }

    private void OnDisable()
    {
        NetworkController.getController().removeListener(this);
    }

    public void messageArrived(string address, List<NetworkController.OSCValue> values)
    {
        Debug.Log($"[BellSpawner] Received OSC at {address}: {ToStringValues(values)}");

        if (address == oscAddress)
        {
            if (TryParsePosition(values, out Vector3 parsed))
            {
                Vector2 transformed = CoordinateNormalizer.TransformToVR(new Vector2(parsed.x, parsed.z));
                newPosition = new Vector3(transformed.x, 10f, transformed.y);  // Fixed Y height
                transform.position = newPosition;
            }
            else
            {
                Debug.LogWarning("[BellSpawner] Failed to parse OSC position.");
            }
        }
    }

    // Called via UnityEvent
    public void bellactive()
    {
        if (!isOn)
        {
            isOn = true;
            Debug.Log("[BellSpawner] Bell enabled.");
        }
    }

    // Called via UnityEvent
    public void bellinactive()
    {
        if (isOn)
        {
            isOn = false;
            Debug.Log("[BellSpawner] Bell disabled.");
        }
    }

    private void Update()
    {
        if (isOn)
        {
            SpawnBell();
        }
    }

    private void SpawnBell()
    {
        if (!canSpawn) return;

        GameObject instance = Instantiate(bellPrefab, newPosition, Quaternion.identity);

        if (instance.TryGetComponent<AudioSource>(out AudioSource audio))
        {
            audio.Play();
        }

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(cooldownTime);
        canSpawn = true;
    }

    private bool TryParsePosition(List<NetworkController.OSCValue> values, out Vector3 result)
    {
        result = Vector3.zero;

        // Option A: Raw float values
        if (values.Count >= 3)
        {
            try
            {
                float x = Convert.ToSingle(values[0].getRaw(), CultureInfo.InvariantCulture);
                float y = Convert.ToSingle(values[1].getRaw(), CultureInfo.InvariantCulture);
                float z = Convert.ToSingle(values[2].getRaw(), CultureInfo.InvariantCulture);
                result = new Vector3(x, y, z);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[BellSpawner] Exception parsing raw values: {e.Message}");
            }
        }

        // Option B: Comma-separated string
        foreach (var oscVal in values)
        {
            string s = oscVal.getString();
            if (s.Contains(","))
            {
                string[] parts = s.Split(',');
                if (parts.Length >= 3)
                {
                    for (int i = 0; i < 3; i++) parts[i] = parts[i].Trim();
                    if (float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                        float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y) &&
                        float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float z))
                    {
                        result = new Vector3(x, y, z);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private string ToStringValues(List<NetworkController.OSCValue> list)
    {
        StringBuilder sb = new StringBuilder();
        bool first = true;
        foreach (var o in list)
        {
            if (!first) sb.Append(", ");
            first = false;
            object raw = null;
            try { raw = o.getRaw(); } catch { }
            sb.Append("[")
              .Append(raw != null ? raw.GetType().Name : "null")
              .Append("] ")
              .Append(o.getString());
        }
        return sb.ToString();
    }
}
