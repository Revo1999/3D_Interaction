using UnityEngine;

public class HideDieCanvas : MonoBehaviour
{
    // Drag your Canvas GameObject here in the Inspector
    public GameObject myCanvas;
    public GameObject TNTHelper;

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
        
        
        if (TNTHelper != null)
        {
            TNTHelper.SetActive(false);
        }
        else
        {
            Debug.LogWarning("TNTHelper reference is missing!");
        }
    }
}