using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement; 

public class KillboxBehavior : MonoBehaviour
{
    public GameObject enemy;

    void spawnEnemy()
    {
        Vector3 newPosition = new Vector3(Random.Range(-8.0f, 8.0f), 5.066f, Random.Range(-8.0f, 8.0f));
        Quaternion newRotation = Random.rotation;
        newRotation.w = 0;
        newRotation.x = 0;
        newRotation.z = 0;
        GameObject.Instantiate(enemy, newPosition, newRotation);
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < 3)
        {
            spawnEnemy();
        }

        if (GameObject.FindGameObjectsWithTag("Player").Length < 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter(Collider collision)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
