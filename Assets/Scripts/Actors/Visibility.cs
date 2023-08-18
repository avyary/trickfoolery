using UnityEngine;

// *******************************************************************************************
// Visibility
//*******************************************************************************************
/// <summary>
/// Raycasts a line from the camera position to detect a collision on update.
/// </summary>
public class Visibility : MonoBehaviour
{
    [SerializeField]
    private Material transMaterial;

    private GameObject camera;
    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position - camera.transform.position;
        //if (Physics.Raycast(camera.transform.position, direction, out hit, direction.magnitude * 0.9f, 3,  QueryTriggerInteraction.Ignore)) {
        //    print("hittin an obstacle");
        //    print(hit.collider.gameObject.name);
        //    hit.collider.gameObject.GetComponent<MeshRenderer>().material = transMaterial;
        //}
    }
}
