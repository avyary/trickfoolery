using UnityEngine;
using UnityEngine.SceneManagement; 

//*******************************************************************************************
// KillboxBehavior
//*******************************************************************************************
/// <summary>
/// Handles the population of enemies and definition of level boundaries. Destroys an
/// enemy on contact with the boundaries and reloads the level if the player breaches
/// the boundaries.
/// </summary>
public class KillboxBehavior : MonoBehaviour
{
    public GameObject[] enemies;
    [SerializeField]
    private int minEnemyNumber;

    /// <summary>
    /// Instantiates a random Enemy type from <i> enemies </i> at a position within a square area on the xz-plane.
    /// </summary>
    void spawnRandomEnemy()
    {
        Vector3 newPosition = new Vector3(Random.Range(-8.0f, 8.0f), 1.5f, Random.Range(-8.0f, 8.0f));
        Quaternion newRotation = Random.rotation;
        newRotation.w = 0;
        newRotation.x = 0;
        newRotation.z = 0;
        GameObject.Instantiate(enemies[Random.Range(0, enemies.Length)], newPosition, newRotation);
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < minEnemyNumber)
        {
            spawnRandomEnemy();
        }

        if (GameObject.FindGameObjectsWithTag("Player").Length < 1)
        {
            GameObject.Find("ProgressTracker").GetComponent<ProgressTracker>().isRestart = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    // Detect collisions between the GameObjects with Colliders attached
    /// <summary>
    /// Destroys Enemy types that come in contact with this Collider and resets the scene and progress tracker
    /// upon collision with the player GameObject.
    /// </summary>
    /// <param name="other"> The Collider of the other GameObject that fired this trigger collider. </param>
    void OnTriggerEnter(Collider collision)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("ProgressTracker").GetComponent<ProgressTracker>().isRestart = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
