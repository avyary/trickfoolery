using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int offDiscoFloor = 0;
        AkSoundEngine.SetRTPCValue("On_Disco_Floor", offDiscoFloor);
    }

    // Update is called once per frame
    void Update()
    {
        int onDiscoFloor = 1;
        int offDiscoFloor = 0;
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * -.5f, -Vector3.up);
        Material surfaceMaterial;

        if(Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Renderer surfaceRenderer = hit.collider.GetComponentInChildren<Renderer>();
            if (surfaceRenderer)
            {
                Debug.Log(surfaceRenderer.material.name);
                if (surfaceRenderer.material.name.Contains("Plane"))
                {
                    AkSoundEngine.SetRTPCValue("On_Disco_Floor", onDiscoFloor);
                    Debug.Log("ondisco");
                }
                else
                {
                    AkSoundEngine.SetRTPCValue("On_Disco_Floor", offDiscoFloor);
                    Debug.Log("offdisco");
                }
            }


        }
    }

}
