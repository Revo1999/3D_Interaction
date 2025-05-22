using System;
using System.Collections.Generic;
using UnityEngine;

public interface NetworkListener {
    
    void messageArrived(string target, List<NetworkController.OSCValue> values);

}
