using UnityEngine;

public class HideDieCamvas : MonoBehaviour
{
    // Drag your Canvas GameObject here in the Inspector
    public GameObject myCanvas;

    void Start()
    {
        if (myCanvas != null)
        {
            myCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Canvas reference is missing!");
        }
    }
}