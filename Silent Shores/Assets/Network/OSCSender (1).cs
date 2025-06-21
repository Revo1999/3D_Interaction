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
            GameObject.Find("Network Controller").GetComponent<UDPProtocol>().sendOSCMessage("jonas", "/test", "Hello, World!", transform.position.x, transform.position.z);
        
    }
}
