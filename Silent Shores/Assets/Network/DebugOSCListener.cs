using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugOSCListener : MonoBehaviour, NetworkListener
{
    public GameObject udpProtocolObject; // Assign in Inspector
    public string ipUpdateAddress = "/setip";

    private UDPProtocol udpProtocol;

    public void messageArrived(string target, List<NetworkController.OSCValue> values)
    {
        Debug.Log("Got OSC message from [" + target + "] Values: " + toString(values));

        if (target == ipUpdateAddress && values.Count >= 1)
        {
            string newIP = values[0].getString();
            Debug.Log($"[DebugOSCListener] Updating all UDP client IPs to: {newIP}");
            SetAllClientIPs(newIP);
        }
    }

    void OnEnable()
    {
        NetworkController.getController().addListener(this);

        if (udpProtocolObject == null)
        {
            Debug.LogError("udpProtocolObject is not assigned in DebugOSCListener.");
            return;
        }

        udpProtocol = udpProtocolObject.GetComponent<UDPProtocol>();

        if (udpProtocol == null)
        {
            Debug.LogError("UDPProtocol component not found on assigned GameObject.");
        }
    }

    void Update() { }

    private string toString(List<NetworkController.OSCValue> list)
    {
        StringBuilder sb = new StringBuilder("");
        bool first = true;

        foreach (NetworkController.OSCValue o in list)
        {
            if (!first)
            {
                sb.Append(", ");
            }
            first = false;
            sb.Append("[" + o.getRaw().GetType() + "] ").Append(o.getString());
        }

        return sb.ToString();
    }

    void OnDisable()
    {
        NetworkController.getController().removeListener(this);
    }

    private void SetAllClientIPs(string newIP)
    {
        if (udpProtocol == null)
        {
            Debug.LogWarning("UDPProtocol is not set.");
            return;
        }

        foreach (var client in udpProtocol.udpClients)
        {
            client.ip = newIP;
        }

        udpProtocol.disconnect();
        udpProtocol.ClearAllClients(); // You must implement this in UDPProtocol

        foreach (var client in udpProtocol.udpClients)
        {
            udpProtocol.addClient(client.name, client.ip, client.port);
        }

        udpProtocol.connect();
    }
}
