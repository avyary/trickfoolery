using UnityEngine;

//*******************************************************************************************
// ActivateObjectOnTrigger
//*******************************************************************************************
/// <summary>
/// Activates walls placed throughout the levels upon triggers with the player GameObject
/// and destroys the barriers when the specified hype threshold is reached.
/// </summary>
public class ActivateObjectOnTrigger : MonoBehaviour
{
    public GameObject activateWall1;
    public GameObject activateWall2;

    private BoxCollider box1;
    private BoxCollider box2;
    
    public GameObject activateIndicator;
    private HypeManager hypeManager;
    private float currHype;
    private bool walls_up = false;
    
    [SerializeField]
    private float hypeCheck;

    [SerializeField]
    float hype_needed;
    
    

    private void Start()
    {
        hypeManager = GameObject.Find("Game Manager").GetComponent<HypeManager>();
        walls_up = false;
        box1 = activateWall1.GetComponent<BoxCollider>();
        box2 = activateWall2.GetComponent<BoxCollider>();
        box1.enabled = false;
        box2.enabled = false;
    }

    /// <summary>
    /// Enables associated wall BoxColliders, calculates a hype threshold, and toggles a flag to track
    /// wall activation upon a triggered collision with the player GameObject.
    /// </summary>
    /// <param name="other"> The Collider of the other GameObject that fired this trigger collider. </param>
    private void OnTriggerEnter(Collider other)
    {
        float local_hype;
        
        // Check if the other object is the player
        if (other.CompareTag("Player"))

        {
            // Activate the two objects
            box1.enabled = true;
            box2.enabled = true;

            currHype = hypeManager.GetHype();
            hypeCheck = currHype + hype_needed;
            walls_up = true;
        }
    }

    private void Update()
    {
        if (hypeManager.GetHype() >= hypeCheck && walls_up)
        {
            Debug.Log("Bridge destroyed");
            box1.enabled = false;
            box2.enabled = false;
            activateIndicator.SetActive(true);
        }
        
    }
}

