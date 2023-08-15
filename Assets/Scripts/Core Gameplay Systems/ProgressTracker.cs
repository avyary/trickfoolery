using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    public bool isRestart = false;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ProgressTracker");
        print(objs.Length);
        print(objs);

        if (objs.Length > 2)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
