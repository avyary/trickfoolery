using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event footstepsEvent;
    public void PlayFootstepSound()
    {   
        GroundSwitch();
        AkSoundEngine.SetSwitch("footsteps", "grassy", gameObject); 
        AkSoundEngine.SetSwitch("footsteps", "disco", gameObject);
        AkSoundEngine.SetSwitch("footsteps", "concrete", gameObject);          

        footstepsEvent.Post(gameObject);
        Debug.Log("Footsteps from Playersound");
    }

    private void GroundSwitch()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);
        Material surfaceMaterial;

        if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Renderer surfaceRenderer = hit.collider.GetComponentInChildren<Renderer>();
            Debug.Log(hit.collider);

            if (surfaceRenderer)
            {
                Debug.Log(surfaceRenderer.material.name);
                if (surfaceRenderer.material.name.Contains("Yellow"))
                {
                    Debug.Log("Yellow grass");
                }
            }
        }
    }

}
