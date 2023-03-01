using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            SceneManager.LoadScene("AlphaEnvironmentMar1", LoadSceneMode.Single);
        }
    }
}
