using UnityEngine;

public class PositionSender : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    
    

    void Update()
    {
        if (targetObject != null)
        {
            Vector3 pos = targetObject.position;
            float[] positionArray = new float[] { pos.x, pos.y, pos.z };

            GameObject.Find("Network Controller")
                .GetComponent<UDPProtocol>()
                .sendOSCMessage("TablePos", "/pos", "xyz:", string.Join(", ", positionArray));
        }
    }
}