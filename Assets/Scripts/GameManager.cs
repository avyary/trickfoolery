using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    
    public GameObject _gameOverObj;
    public TMP_Text _gameOverText;
    public GameObject _gameOverPanel;

    public GameObject[] enemies;
    [SerializeField]
    private int minEnemyNumber;

    private bool isGameOver = false;

    void Start()
    {
        _gameOverObj = GameObject.Find("GameOverText");
        _gameOverPanel = GameObject.Find("GameOverPanel");
        _gameOverText = _gameOverObj.GetComponent<TMP_Text>();
        _gameOverPanel.SetActive(false);
        UnpauseGame();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < minEnemyNumber)
        {
            SpawnRandomEnemy();
        }
        if (isGameOver && Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        isGameOver = true;
        isPaused = true;
        Time.timeScale = 0;
        _gameOverText.text = "Hype Meter Filled!";
        _gameOverPanel.SetActive(true);
    }

    public void GameOverLose()
    {
        isGameOver = true;
        isPaused = true;
        Time.timeScale = 0;
        _gameOverText.text = "You Died!";
        _gameOverPanel.SetActive(true);
    }

    void SpawnRandomEnemy()
    {
        Vector3 newPosition = new Vector3(Random.Range(80.0f, 150.0f), 6.2845661f, Random.Range(-23.0f, -10.0f));
        Quaternion newRotation = Random.rotation;
        newRotation.w = 0;
        newRotation.x = 0;
        newRotation.z = 0;
        GameObject.Instantiate(enemies[Random.Range(0, enemies.Length)], newPosition, newRotation);
    }


}
