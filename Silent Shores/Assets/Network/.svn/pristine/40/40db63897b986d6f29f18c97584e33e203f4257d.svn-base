using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugOSCListener : MonoBehaviour, NetworkListener {
    public void messageArrived(string target, List<NetworkController.OSCValue> values)
    {
        Debug.Log("Got OSC message from [" + target + "] Values: " + toString(values));
    }

    void OnEnable () {
        NetworkController.getController().addListener(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private string toString(List<NetworkController.OSCValue> list)
    {
        StringBuilder sb = new StringBuilder("");

        bool first = true;

        foreach(NetworkController.OSCValue o in list)
        {
            if(first)
            {
                first = false;
            } else
            {
                sb.Append(", ");
            }

            sb.Append("["+(o.getRaw().GetType())+"] ").Append(o.getString());
        }

        return sb.ToString();
    }

    void OnDisable () {
        NetworkController.getController().removeListener(this);
    }
}
