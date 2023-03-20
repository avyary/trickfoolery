using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectKick : MonoBehaviour
{

    [SerializeField] private float forceMultiplier = 1.5f;
    [SerializeField] private float UnStuck = 0.1f;
    [SerializeField] private float despawnDelay = 2f;

    private Rigidbody rb;
    private bool isActivated = false;
    
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
       rb.isKinematic = true; 
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
       if (!isActivated && other.CompareTag("Player")) {
        
        //detach object from ground
        transform.Translate(Vector3.up * UnStuck, Space.World);

        Vector3 contactPoint = other.ClosestPoint(transform.position);
        Vector3 contactNormal = transform.position - contactPoint;
        Vector3 forceDirection = -contactNormal.normalized;
        float forceMagnitude = other.attachedRigidbody.velocity.magnitude * forceMultiplier;
    
        rb.isKinematic = false;
        rb.AddForceAtPosition(forceMagnitude * forceDirection, contactPoint, ForceMode.Impulse);
    
        isActivated = true;
        Invoke("Despawn", despawnDelay);
       }
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
    }
}
