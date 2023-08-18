using UnityEngine;

// *******************************************************************************************
// CameraFollowPlayer
//*******************************************************************************************
/// <summary>
/// Adjusts the camera's position to follow the player with an offset.
/// </summary>
public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Vector3 cameraOffset;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + cameraOffset.x, cameraOffset.y, player.transform.position.z);
        transform.position = player.transform.position + cameraOffset;
    }
}
