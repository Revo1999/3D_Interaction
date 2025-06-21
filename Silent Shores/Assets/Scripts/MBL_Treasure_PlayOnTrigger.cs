using TMPro;
using UnityEngine;

public class PlayOnCollisionTrigger : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem particleSystem;
    public OVRPlayerController ovrController;
    public GameObject player;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (particleSystem == null)
            particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerTreasure();
        }
    }

    private void TriggerTreasure()
    {
        animator.SetTrigger("Play");

        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}
