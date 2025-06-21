using UnityEngine;

public class ShowTNTHelper : MonoBehaviour// Drag your Canvas GameObject here in the Inspector
{
    public GameObject myCanvas; // Drag your Canvas GameObject here in the Inspector

    void OnCollisionEnter(Collision collision)
    {
        if (myCanvas != null)
        {
            myCanvas.SetActive(true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (myCanvas != null)
        {
            myCanvas.SetActive(false);
        }
    }
}
