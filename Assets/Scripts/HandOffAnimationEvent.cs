using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOffAnimationEvent : MonoBehaviour
{
    public AkEvent playerTauntSFX;

    public void PlayTauntSound()
    {
        if (playerTauntSFX != null) 
        {
            playerTauntSFX.HandleEvent(gameObject);
        }
    }

}
