
using System;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateObjectOnTrigger : MonoBehaviour
{
    public GameObject activateWall1;
    public GameObject activateWall2;
    
     public GameObject activateIndicator;
    private HypeManager hypeManager;
    private float currHype;
    
    [SerializeField]
    private float hypeCheck;

    [SerializeField]
    float hype_needed;
    
    

    private void Start()
    {
        hypeManager = GameObject.Find("Game Manager").GetComponent<HypeManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        float local_hype;
        
        // Check if the other object is the player
        if (other.CompareTag("Player"))

        {
            // Activate the two objects
            activateWall1.SetActive(true);
            activateWall2.SetActive(true);

            currHype = hypeManager.GetHype();
            hypeCheck = currHype + hype_needed;
        }
    }

    private void Update()
    {
        if (hypeManager.GetHype() >= hypeCheck)
        {
            Debug.Log("Bridge destroyed");
        
            activateWall1.SetActive(false);
            activateWall2.SetActive(false);
            activateIndicator.SetActive(true);

        }
        
    }
}

