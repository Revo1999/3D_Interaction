using UnityEngine;
using TMPro;

public class PlayOnTrigger : MonoBehaviour
{
    public Animator animator;
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
        else if (other.CompareTag("Teddy"))
        {
            animator.SetTrigger("Play");
            // Disable Trap or other Teddy-specific logic
        }
    }

    private void TriggerTrap()
    {
        animator.SetTrigger("Play");
        ovrController.Acceleration = 0.0f;
        DieCanvas.SetActive(true);
        isPlayerDead = true;
    }

    private void ResetPlayer()
    {
        // Reset animation
        animator.Play("Reset", 0, 0f);
        
        // Reset player movement
        ovrController.Acceleration = 0.08f; // Default acceleration value
        
        // Hide death UI
        DieCanvas.SetActive(false);
        
        isPlayerDead = false;
        
        // You might also need to reset player position here
        player.transform.position = resetLocation.transform.position;
    }
}