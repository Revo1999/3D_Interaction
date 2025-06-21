using UnityEngine;

public class PingSpawner : MonoBehaviour
{
    private GameObject controller;
    private UDPProtocol udp;

    void Start()
    {
        controller = GameObject.Find("Network Controller");
        if (controller != null)
        {
            udp = controller.GetComponent<UDPProtocol>();
            if (udp == null)
                Debug.LogError("[PingSpawner] UDPProtocol component not found on Network Controller.");
        }
        else
        {
            Debug.LogError("[PingSpawner] Network Controller not found.");
        }
    }

    public void SpawnPingByIndex(int index)
    {
        if (udp != null)
        {
            Debug.Log("[PingSpawner] Sending ping index: " + index);
            udp.sendOSCMessage("TablePing", "/pingToggle", index.ToString());
        }
        else
        {
            Debug.LogWarning("[PingSpawner] Cannot send ping, UDP is null.");
        }
    }
}
