using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTauntSound : MonoBehaviour
{

    [SerializeField]
    private AK.Wwise.Event tauntEvent; //this is from the wwise soundbank

    public void PlayTauntSound()
    {
        tauntEvent.Post(gameObject);
    }
}

