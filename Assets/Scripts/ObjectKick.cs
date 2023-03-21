using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectKick : MonoBehaviour
{

    [SerializeField] private float forceMultiplier = 1.5f;
    [SerializeField] private float unStuck = 0.1f;
    [SerializeField] private float despawnDelay = 2f;
    [SerializeField] private Collider triggerCollider;

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
        if (!isActivated && other == triggerCollider)
        {
            //detach object from ground
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                float heightAboveGround = hit.distance + unStuck;
                transform.position += Vector3.up * heightAboveGround;
            }

            //launch object based on player's impact direction and velocity
            Vector3 playerVelocity = other.attachedRigidbody.velocity;
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Vector3 contactNormal = transform.position - contactPoint;
            Vector3 forceDirection = -contactNormal.normalized;
            float forceMagnitude = playerVelocity.magnitude * forceMultiplier;

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
