using System.Collections.Generic;
using UnityEngine;

public class OSCConnectionWatcher : MonoBehaviour, NetworkListener
{
    public bool TableConnected { get; private set; }

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
        if (address == "/pong")
        {
            TableConnected = true;
            Debug.Log("[OSCConnectionWatcher] Received /pong. Table is connected.");
        }
    }

    public void ResetStatus()
    {
        TableConnected = false;
    }
}
