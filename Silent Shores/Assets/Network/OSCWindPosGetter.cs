using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Text;

public class OSCWindPositionReceiver : MonoBehaviour, NetworkListener
{
    [Tooltip("The OSC address to listen for (e.g., /Windpos)")]
    public string oscAddress = "/windPos";

    [Tooltip("The player GameObject to follow vertically.")]
    public GameObject player;

    void OnEnable()
    {
        NetworkController.getController().addListener(this);
    }

    void OnDisable()
    {
        NetworkController.getController().removeListener(this);
    }
    
    public void messageArrived(string address, List<NetworkController.OSCValue> values)
    {
        Debug.Log($"[OSCWindPositionReceiver] Received OSC at {address}: {ToStringValues(values)}");

        if (address == oscAddress)
        {
            Vector3 parsed;
            if (TryParsePosition(values, out parsed))
            {
                // Transform X/Z to VR space
                Vector2 transformed = CoordinateNormalizer.TransformToVR(new Vector2(parsed.x, parsed.z));

                // Set position with transformed X/Z, and Y = player Y + 20
                float newY = player != null ? player.transform.position.y + 20f : transform.position.y;

                Vector3 newPos = new Vector3(transformed.x, newY, transformed.y);
                transform.position = newPos;
            }
            else
            {
                Debug.LogWarning("[OSCWindPositionReceiver] Failed to parse position.");
            }
        }
    }

  private bool TryParsePosition(List<NetworkController.OSCValue> values, out Vector3 result)
  {
    result = Vector3.zero;

    // OPTION A: Raw float values
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
        catch { }
    }

    // OPTION B: Comma-separated string
    foreach (var oscVal in values)
    {
        string s = oscVal.getString();
        if (s.Contains(","))
        {
            string[] parts = s.Split(',');
            if (parts.Length >= 3)
            {
                for (int i = 0; i < 3; i++) parts[i] = parts[i].Trim();

                // Pre-assign defaults
                float x = 0, y = 0, z = 0;
                bool parsed = float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x) &&
                              float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y) &&
                              float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out z);

                if (!parsed)
                {
                    parsed = float.TryParse(parts[0], NumberStyles.Float, CultureInfo.CurrentCulture, out x) &&
                             float.TryParse(parts[1], NumberStyles.Float, CultureInfo.CurrentCulture, out y) &&
                             float.TryParse(parts[2], NumberStyles.Float, CultureInfo.CurrentCulture, out z);
                }

                if (parsed)
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
