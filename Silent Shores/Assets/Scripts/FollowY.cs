using UnityEngine;

public class FollowY : MonoBehaviour
{
    public Transform target; // The object to follow

    void Update()
    {
        if (target != null)
        {
            Vector3 currentPosition = transform.position;
            transform.position = new Vector3(currentPosition.x, target.position.y + 20f, currentPosition.z);
        }
    }
}
