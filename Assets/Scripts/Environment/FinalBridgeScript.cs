using UnityEngine;

//*******************************************************************************************
// FinalBridgeScript
//*******************************************************************************************
/// <summary>
/// Handles the behavior of walls in the environment as a player approaches it.
/// </summary>
public class FinalBridgeScript : MonoBehaviour
{
    public GameObject activateWall1;
    
    private void Start()
    {
       
    }

    /// <summary>
    /// Enables the <i> activateWall1 </i> GameObject upon triggered collision with the player GameObject.
    /// </summary>
    /// <param name="other"> The Collider of the other GameObject that fired this trigger collider. </param>
    private void OnTriggerEnter(Collider other)
    {
        float local_hype;
        
        // Check if the other object is the player
        if (other.CompareTag("Player"))

        {
            // Activate the two objects
            activateWall1.SetActive(true);
            
        }
    }
}

