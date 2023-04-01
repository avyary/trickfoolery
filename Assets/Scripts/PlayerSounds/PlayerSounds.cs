using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    [SerializeField]
    private AK.Wwise.Event footstepsEvent; //this is from the wwise soundbank

    public void PlayFootstepSound()
    {
        footstepsEvent.Post(gameObject);
    }
}

