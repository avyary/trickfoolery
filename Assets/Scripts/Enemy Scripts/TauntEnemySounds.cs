using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

 
    public void StartAttackSFX()
    {
        attackStartSFX.Post(gameObject);
    }
    
    public void FinishAttackSFX()
    {
        attackEndSFX.Post(gameObject);
    }


}
