using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCSender : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            // Send OSC message
            // The message is sent to the "Network Controller" GameObject
            // and the UDPProtocol component is used to send the message
            // The message is sent to the "jonas" address with the "/test" path
            // The message contains the string "Hello, World!" and the x and z position of the GameObject
            // The x and z position of the GameObject is used as the last two arguments
            // The message is sent every frame
            GameObject.Find("Network Controller").GetComponent<UDPProtocol>().sendOSCMessage("Table", "/test", "Hello from VR-headset!");
        
    }
}
