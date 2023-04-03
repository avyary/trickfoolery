
using UnityEngine;

public class ActivateObjectOnTrigger : MonoBehaviour
{
    public GameObject activateObject1;
    public GameObject activateObject2;

    private void OnTriggerEnter(Collider other)
    {
        float local_hype;
        
        // Check if the other object is the player
        if (other.CompareTag("Player"))
        {  
            // Check if the hype bar reach the threshold within this level 
            if (local_hype >= 30)
            {
                // Activate the objects
                activateObject1.SetActive(true);
                activateObject2.SetActive(true);
            }
        }
    }
}
