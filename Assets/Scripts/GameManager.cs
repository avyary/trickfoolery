using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    
    public GameObject _gameOverObj;
    public TMP_Text _gameOverText;
    void Start()
    {
        _gameOverObj = GameObject.Find("GameOverText");
        _gameOverText = _gameOverObj.GetComponent<TMP_Text>();
    }

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

    public void GameOverWin()
    {
        isPaused = true;
        Time.timeScale = 0;
        _gameOverText.text = "Hype Meter Filled!";
    }
    public void GameOverLose()
    {
        isPaused = true;
        Time.timeScale = 0;
        _gameOverText.text = "You Died!";
    }
}
