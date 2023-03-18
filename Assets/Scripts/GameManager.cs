using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState {
    PreCombat,
    Combat,
    PostCombat
}

public class GameManager : MonoBehaviour
{
    public GameState state = GameState.PreCombat;
    public bool isPaused = true;
    public bool showPauseMenu = false;
    
    public AK.Wwise.Event pauseSFX;
    public AK.Wwise.Event unpSFX;

    public GameObject _gameOverObj;
    public TMP_Text _gameOverText;
    public GameObject _gameOverPrompt;
    public TMP_Text _gameOverPromptText;
    public GameObject _gameOverPanel;
    public UIManager uiManager;

    private bool isGameWon = false;

    public GameObject[] enemies;
    [SerializeField]
    private int minEnemyNumber;
    [SerializeField]
    private string nextScene;

    private bool isGameOver = false;

    public Collider spawnRange;

    [SerializeField]
    UnityEvent preCombat;

    [SerializeField]
    private int startDelay;

    void Start()
    {
        Time.timeScale = 0;
        if (preCombat == null)
            preCombat = new UnityEvent();

        uiManager = gameObject.GetComponent<UIManager>();

        _gameOverObj = GameObject.Find("GameOverText");
        _gameOverPrompt = GameObject.Find("GameOverPrompt");
        _gameOverPanel = GameObject.Find("GameOverPanel");
        _gameOverText = _gameOverObj.GetComponent<TMP_Text>();
        _gameOverPromptText = _gameOverPrompt.GetComponent<TMP_Text>();
        _gameOverPanel.SetActive(false);

        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel() {
        yield return new WaitForSecondsRealtime(1f);
        preCombat.Invoke();
        StartCoroutine(uiManager.StartCombat());
    }

    public void StartCombat() {
        Time.timeScale = 1;
        state = GameState.Combat;
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
        uiManager.ShowPauseMenu();
        PauseGame();
    }

    void HidePauseMenu() {
        showPauseMenu = false;
        uiManager.HidePauseMenu();
        unpSFX.Post(gameObject);
        if (!isGameOver) {
            UnpauseGame();
        }
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
        _gameOverPromptText.text = "Press [Return] to continue";
        _gameOverPanel.SetActive(true);
        isGameWon = true;
    }

    public void GameOverLose()
    {
        isGameOver = true;
        PauseGame();
        _gameOverText.text = "You Died!";
        _gameOverPromptText.text = "Press [Return] to try again";
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

