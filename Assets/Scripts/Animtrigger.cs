using UnityEngine;
using UnityEngine.AI;


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
        // Debug.Log("isWalking = true");
    }
    else
    {
        animator.SetBool("isWalking", false);
        // Debug.Log("isWalking = false");
    }
}




}