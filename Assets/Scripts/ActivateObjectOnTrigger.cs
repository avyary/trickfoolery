
using UnityEngine;

public class ActivateObjectOnTrigger : MonoBehaviour
{
    public GameObject activateObject1;
    public GameObject activateObject2;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is the player
        if (other.CompareTag("Player"))
        {
            // Activate the two objects
            activateObject1.SetActive(true);
            activateObject2.SetActive(true);
        }
    }
}