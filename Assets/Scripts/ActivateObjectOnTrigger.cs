
using System;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateObjectOnTrigger : MonoBehaviour
{
    public GameObject activateObject1;
    public GameObject activateObject2;
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
        // Check if the other object is the player
        if (other.CompareTag("Player"))
        {
            // Activate the two objects
            activateObject1.SetActive(true);
            activateObject2.SetActive(true);

            currHype = hypeManager.GetHype();
            hypeCheck = currHype + hype_needed;
        }
    }

    private void Update()
    {
        if (hypeManager.GetHype() >= hypeCheck)
        {
            Debug.Log("Bridge destroyed");
            activateObject1.SetActive(false);
            activateObject2.SetActive(false);
        }
        
    }
}