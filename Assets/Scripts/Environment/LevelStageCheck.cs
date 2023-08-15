
using UnityEngine;

public class LevelStageCheck : MonoBehaviour
{

    public string objectTag = "MyObjectTag";
    public GameObject deactivateObject;
    public GameObject deactivateObject2;

    private void Start()
    {
        // Find all objects in the scene with the specified tag
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(objectTag);

        // If no objects with the specified tag are found, activate the activateObject and deactivate the deactivateObject
        if (objectsWithTag.Length == 0)
        {
            deactivateObject.SetActive(false);
            deactivateObject2.SetActive(false);
        }
    }
}
