using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera");
        print(camera);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform.position, Vector3.down);
    }
}
