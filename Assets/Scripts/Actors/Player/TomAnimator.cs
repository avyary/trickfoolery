using UnityEngine;

// *******************************************************************************************
// TomAnimator
//*******************************************************************************************
/// <summary>
/// Controls the Animator according to the PlayerMovement states.
/// </summary>
public class TomAnimator : MonoBehaviour
{   
    // Animator anim;

    PlayerMovement playerController;

    [SerializeField]
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.state == PlayerMovement.AbilityState.walking) 
        {
            // running animation
            anim.SetTrigger("StartRunning");
            
        }
        else 
        {
            // idle animation
            anim.SetTrigger("MakeStill");

        }
    }
}
