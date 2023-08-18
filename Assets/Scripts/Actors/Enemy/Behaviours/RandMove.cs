using UnityEngine;
using UnityEngine.AI;

//*******************************************************************************************
// RandMove
//*******************************************************************************************
/// <summary>
/// Generates random paths for characters via the NavMeshAgent and contains logic to
/// hone in on the player when they're sighted.
/// </summary>
public class RandMove : MonoBehaviour
{
    private float speed = 1.75f;
    private int repeatSpeed = 0;
    private int direction = 3;
    private int rotateTime = 0;

    public bool detected;

    public float radius = 10.0f;

    public Transform player;
    public NavMeshAgent enemy;

    private GameObject platform;


    private Transform platformTransform;

    private float xEdge;
    private float zEdge;
    
    // Start is called before the first frame update
    void Start()
    {
        // platform = GameObject.FindGameObjectWithTag("Platform");
        // platformTransform = platform.GetComponent<Transform>();
        // xEdge = platformTransform.position.x;
        // zEdge = platformTransform.position.z;
    }   

    // Update is called once per frame
    void Update()
    {   
        if (detected == false)
        {
            System.Random rd = new System.Random();

            if (repeatSpeed < 2000){
                repeatSpeed += rd.Next(3);
            } else {
                repeatSpeed = 0;
                direction = rd.Next(1, 5);
                rotateTime = 0;
            } 

            if (Mathf.Abs(player.position.z - transform.position.z) <= radius) 
            {
                enemy.SetDestination(player.position);
            }
            else 
            {
                move(direction);
            }
        }
    }

    /// <summary>
    /// Moves or rotates this GameObject at random increments for the duration of <i> rotateTime </i> within
    /// a targeted world-space area along the xz-plane. Moves and rotates this GameObject at specified
    /// increments when outside of the targeted xz-plane area.
    /// </summary>
    /// <param name="direction"> The index to select various rotational increment modes along the y-axis. </param>
    public void move(int direction) {
        if (-6.5f <= transform.position.x 
        && transform.position.x <= 6.5f 
        && -6.5f <= transform.position.z
        && transform.position.z <= 6.5f)
        {
            if (rotateTime < 180){
                if(direction == 1) {
                    transform.Rotate(new Vector3(0, .5f, 0), Space.Self);
                }
                if(direction == 2){
                     transform.Rotate(new Vector3(0, -.5f, 0), Space.Self);
                }
                if(direction == 4){
                     transform.Rotate(Vector3.up, 1, Space.Self);
                }
                rotateTime ++;
            } else {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        } 
        else 
        {
            transform.Rotate(new Vector3(0, .5f, 0), Space.Self);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }

    /// <summary>
    /// When colliding with the player, toggles detection to cease random movement.
    /// </summary>
    /// <param name="collision"> The Collider of the other GameObject that collided with this Collider. </param>
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Player")
        {
            detected = true;
        }
    }

}
