using UnityEngine;
using TMPro;

public class FallTrap : MonoBehaviour
{
    public OVRPlayerController ovrController;
    public GameObject DieCanvas;
    public GameObject resetLocation;
    public GameObject player;
    
    private bool isPlayerDead = false;

    void Update() 
    {
        // Check if ANY of the buttons (A, B, X, Y) are pressed when player is dead
        if (isPlayerDead && (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch) ||  // A
            OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch) ||  // B
            OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch) ||  // X
            OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch)))    // Y
        {
            ResetPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerTrap();
        }
    }

    private void TriggerTrap()
    {
        ovrController.Acceleration = 0.0f;
        DieCanvas.SetActive(true);
        isPlayerDead = true;
    }

    private void ResetPlayer()
    {
        // Reset player movement
        ovrController.Acceleration = 0.08f; // Default acceleration value
        
        // Hide death UI
        DieCanvas.SetActive(false);
        
        isPlayerDead = false;
        
        // You might also need to reset player position here
        player.transform.position = resetLocation.transform.position;
    }
}