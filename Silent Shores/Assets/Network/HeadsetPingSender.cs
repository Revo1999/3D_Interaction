using System.Collections.Generic;
using UnityEngine;

public class HeadsetPingSender : MonoBehaviour, NetworkListener
{
    public GameObject udpProtocolObject;
    public string tableClientName = "TablePing";
    public string pingAddress = "/ping";
    public string pongAddress = "/pong";
    public float pingInterval = 1.0f;

    private UDPProtocol udpProtocol;
    private float pingTimer;

    void OnEnable()
    {
        NetworkController.getController().addListener(this);

        udpProtocol = udpProtocolObject.GetComponent<UDPProtocol>();
        if (udpProtocol == null)
        {
            Debug.LogError("UDPProtocol not found.");
        }
    }

    void OnDisable()
    {
        NetworkController.getController().removeListener(this);
    }

    void Update()
    {
        pingTimer += Time.deltaTime;
        if (pingTimer >= pingInterval)
        {
            SendPing();
            pingTimer = 0;
        }
    }

    void SendPing()
    {
        Debug.Log("[Headset] Sending /ping to TablePing...");
        udpProtocol.sendMessage(tableClientName, pingAddress, "ping from headset");
    }

    public void messageArrived(string address, List<NetworkController.OSCValue> values)
    {
        if (address == pongAddress && values.Count > 0)
        {
            Debug.Log("[Headset] Received /pong: " + values[0].getString());
        }
    }
}
