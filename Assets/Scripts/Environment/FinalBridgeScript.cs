
using System;
using Unity.VisualScripting;
using UnityEngine;

public class FinalBridgeScript : MonoBehaviour
{
    public GameObject activateWall1;
   


    private void Start()
    {
       
    }

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

