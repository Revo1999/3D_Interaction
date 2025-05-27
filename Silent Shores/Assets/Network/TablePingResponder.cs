using System.Collections.Generic;
using UnityEngine;

public class TablePingResponder : MonoBehaviour, NetworkListener
{
    public GameObject udpProtocolObject;
    public string headsetClientName = "HeadsetPing"; // You must define this in the Table's client list
    public string pingAddress = "/ping";
    public string pongAddress = "/pong";

    private UDPProtocol udpProtocol;

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

    public void messageArrived(string address, List<NetworkController.OSCValue> values)
    {
        if (address == pingAddress && values.Count > 0)
        {
            Debug.Log("[Table] Received /ping: " + values[0].getString());
            SendPong();
        }
    }

    void SendPong()
    {
        udpProtocol.sendMessage(headsetClientName, pongAddress, "pong from table");
        Debug.Log("[Table] Sent /pong to HeadsetPing");
    }
}
