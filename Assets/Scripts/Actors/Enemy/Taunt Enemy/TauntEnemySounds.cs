using UnityEngine;

//*******************************************************************************************
// TauntEnemySounds
//*******************************************************************************************
/// <summary>
/// Acts as a sound bank for all the SFX associated with a taunt enemy.
/// </summary>
public class TauntEnemySounds : MonoBehaviour
{

    [SerializeField]
    private AK.Wwise.Event attackStartSFX;
    [SerializeField]
    private AK.Wwise.Event attackEndSFX;

    //startattackanim is the name of the event on the animation

    //things like attackstartsfx.post are the names of the components
    //are the private serialize fields where I put events
    //you will be able to find them easily on the game object

    /// <summary>
    /// Plays the StartAttackSFX.
    /// </summary>
    public void StartAttackSFX()
    {
        attackStartSFX.Post(gameObject);
    }
    
    /// <summary>
    /// Plays the FinishStartSFX.
    /// </summary>
    public void FinishAttackSFX()
    {
        attackEndSFX.Post(gameObject);
    }
}
