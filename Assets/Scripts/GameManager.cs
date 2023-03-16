using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused = true;
    public bool showPauseMenu = false;
    
    public AK.Wwise.Event pauseSFX;
    public AK.Wwise.Event unpSFX;

    public GameObject _gameOverObj;
    public TMP_Text _gameOverText;
    public GameObject _gameOverPanel;
    public GameObject _pauseMenu;
    public GameObject battleStart;

    private bool isGameWon = false;


    public GameObject[] enemies;
    [SerializeField]
    private int minEnemyNumber;
    [SerializeField]
    private string nextScene;

    private bool isGameOver = false;

    public Collider spawnRange;

    [SerializeField]
    private int startDelay;

    void Start()
    {
        _gameOverObj = GameObject.Find("GameOverText");
        _pauseMenu = GameObject.Find("PauseMenu");
        _gameOverPanel = GameObject.Find("GameOverPanel");
        battleStart = GameObject.Find("BattleStart");
        _gameOverText = _gameOverObj.GetComponent<TMP_Text>();
        _gameOverPanel.SetActive(false);
        _pauseMenu.SetActive(false);
        StartGame();
    }

    void StartGame() {
        battleStart.GetComponent<Animator>().SetTrigger("StartGame");
        UnpauseGame();
    }

    void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            TogglePauseMenu();
        }
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < minEnemyNumber)
        {
            SpawnRandomEnemy();
        }
        if (isGameOver && Input.GetButtonDown("Confirm"))
        {
            if (isGameWon) {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (showPauseMenu && Input.GetButtonDown("Confirm")) {
            HidePauseMenu();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    bool TogglePauseMenu()
    {
        if (!showPauseMenu)
        {
            pauseSFX.Post(gameObject);
            ShowPauseMenu();
        }
        else
        {
            HidePauseMenu();
        }
        return showPauseMenu;
    }

    bool TogglePause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
        return isPaused;
    }

    void ShowPauseMenu() {
        showPauseMenu = true;
        PauseGame();
        _pauseMenu.SetActive(true);
    }

    void HidePauseMenu() {
        showPauseMenu = false;
        unpSFX.Post(gameObject);
        if (!isGameOver) {
            UnpauseGame();
        }
        _pauseMenu.SetActive(false);
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    public void GameOverWin()
    {
        isGameOver = true;
        PauseGame();
        _gameOverText.text = "Hype Meter Filled!";
        _gameOverPanel.SetActive(true);
        isGameWon = true;
    }

    public void GameOverLose()
    {
        isGameOver = true;
        PauseGame();
        _gameOverText.text = "You Died!";
        _gameOverPanel.SetActive(true);
    }

    void SpawnRandomEnemy()
    {
        Vector3 newPosition = Vector3.zero;
        Collider[] colliders = spawnRange.GetComponentsInChildren<Collider>();

        if (colliders.Length > 0 ){
            Collider collider = colliders[Random.Range(0,colliders.Length)];
            Vector3 center = collider.bounds.center;
            Vector3 extents = collider.bounds.extents;

            newPosition = new Vector3(
                Random.Range(center.x - extents.x, center.x + extents.x),
                center.y,
                Random.Range(center.z - extents.z, center.z + extents.z)
            );
        }

        Quaternion newRotation = Random.rotation;
        newRotation.w = 0;
        newRotation.x = 0;
        newRotation.z = 0;
        GameObject.Instantiate(enemies[Random.Range(0, enemies.Length)], newPosition, newRotation);
    }

    Vector3 RandonPosInCollider (Collider[] colliders){
        Vector3 boundsCenter = Vector3.zero;
        Vector3 boundsExtents = Vector3.zero;

        foreach (var collider in colliders)
        {
        boundsCenter += collider.bounds.center;    
        boundsExtents += collider.bounds.extents;    
        }

        boundsCenter /= colliders.Length;
        boundsExtents /= colliders.Length;

        Vector3 randomPoint = new Vector3(
            Random.Range(boundsCenter.x - boundsExtents.x, boundsCenter.x + boundsExtents.x),
            boundsCenter.y,
            Random.Range(boundsCenter.z - boundsExtents.z, boundsCenter.z + boundsExtents.z)
        );

        return randomPoint;
    }
}

