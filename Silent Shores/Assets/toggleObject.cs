using UnityEngine;
using System.Collections;

public class ToggleObject : MonoBehaviour
{
    // The object you want to control
    [SerializeField] private GameObject targetObject;

    void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    // Call this function to enable the object for 30 seconds
    public void EnableObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            StartCoroutine(DisableAfterTime(30f)); // Disable after 30 seconds
        }
    }

    private IEnumerator DisableAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
