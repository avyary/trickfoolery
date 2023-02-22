using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    bool TogglePause() {
        if (!isPaused) {
            PauseGame();
        }
        else {
            UnpauseGame();
        }
        return isPaused;
    }

    void PauseGame() {
        isPaused = true;
        Time.timeScale = 0;
    }

    void UnpauseGame() {
        isPaused = false;
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        isPaused = true;
        Time.timeScale = 0;
    }
}
