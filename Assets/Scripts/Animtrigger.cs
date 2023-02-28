using UnityEngine;
using UnityEngine.AI;
public class Animtrigger : MonoBehaviour
{
    public Transform playerTransform;
    public Animator animator;
    NavMeshAgent agent;
    private new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
      
    }

    void Update()
    {
        agent.destination = playerTransform.position;
        animator.SetFloat("speed", agent.velocity.magnitude);
     
    }
}
