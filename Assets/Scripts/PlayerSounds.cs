using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event footstepsEvent;
    public void PlayFootstepSound()
    {
        footstepsEvent.Post(gameObject);
        Debug.Log("Footsteps from Playersound");
    }

}
