using UnityEngine;

//*******************************************************************************************
// LookAt
//*******************************************************************************************
/// <summary>
/// Rotates the GameObject this script is attached to to directly face the camera. Used
/// for some angy and alert indicators.
/// </summary>
public class LookAt : MonoBehaviour
{
    private GameObject camera;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform.position);
    }
}
