using UnityEngine;

//*******************************************************************************************
// HandOffAnimationEvent
//*******************************************************************************************
/// <summary>
/// Plays the SFX associated with the player taunt mechanic.
/// </summary>
public class HandOffAnimationEvent : MonoBehaviour
{
    public AkEvent playerTauntSFX;

    /// <summary>
    /// Plays the playerTauntSFX if it is not null.
    /// </summary>
    public void PlayTauntSound()
    {
        if (playerTauntSFX != null) 
        {
            playerTauntSFX.HandleEvent(gameObject);
        }
    }
}
