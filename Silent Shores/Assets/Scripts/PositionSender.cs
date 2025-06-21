using UnityEngine;

public class PositionSender : MonoBehaviour
{
    [SerializeField] private Transform targetObject;

    private UDPProtocol udp;

    void Start()
    {
        GameObject controller = GameObject.Find("Network Controller");
        if (controller != null)
        {
            udp = controller.GetComponent<UDPProtocol>();
            if (udp == null)
                Debug.LogError("[PositionSender] UDPProtocol component not found on Network Controller.");
        }
        else
        {
            Debug.LogError("[PositionSender] Network Controller not found.");
        }
    }

    void Update()
    {
        if (targetObject != null && udp != null)
        {
            // Position
            Vector3 pos = targetObject.position;
            Vector2 posTransformed = CoordinateNormalizer.ReverseToTable(new Vector2(pos.x, pos.z));
            float[] positionArray = new float[] { posTransformed.x, pos.y, posTransformed.y };

            // Rotation (not sent here, but useful if needed)
            Vector3 rot = targetObject.eulerAngles;
            float[] rotationArray = new float[] { rot.x, rot.y, rot.z };

            udp.sendOSCMessage("TablePos", "/pos", "xyz:", string.Join(", ", positionArray));
        }
    }
}
