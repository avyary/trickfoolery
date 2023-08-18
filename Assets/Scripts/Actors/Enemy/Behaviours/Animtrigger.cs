using UnityEngine;
using UnityEngine.AI;

//*******************************************************************************************
// Animtrigger
//*******************************************************************************************
/// <summary>
/// Handles the animations for an actor associated with a NavMeshAgent. Currently
/// contains logic to trigger walking animations in a designated AnimationController.
/// </summary>
public class Animtrigger : MonoBehaviour {
    public NavMeshAgent navMeshAgent;

    public Animator animator;
    public bool isWalking;

    private Vector3 previousPosition;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();

        previousPosition = transform.position;
    }

    void Update()
    {
        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}