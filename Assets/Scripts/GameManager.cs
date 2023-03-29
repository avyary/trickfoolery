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
    // state variables
    public GameState state = GameState.PreCombat;
    public bool isPaused = true;
    public bool showPauseMenu = false;
    private bool isGameWon = false;
    private bool isGameOver = false;
    private bool isTogglingPause = false;
    
    // wwise
    public AK.Wwise.Event pauseSFX;
    public AK.Wwise.Event unpSFX;

    // object references
    public GameObject _gameOverObj;
    public TMP_Text _gameOverText;
    public GameObject _gameOverPrompt;
    public TMP_Text _gameOverPromptText;
    public GameObject _gameOverPanel;
    public UIManager uiManager;

    // for respawn
    public GameObject[] enemies;
    public Collider spawnRange;

    // parameters
    [SerializeField]
    private int minEnemyNumber;
    [SerializeField]
    private string nextScene;
    [SerializeField]
    UnityEvent preCombat;

    void Start()
    {
        Time.timeScale = 0;

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
        yield return new WaitForSecondsRealtime(1f); // wait for fade-in animation
        bool hasPersistentTarget = false;
        for (int i = 0; i < preCombat.GetPersistentEventCount(); i++) {
            if (preCombat.GetPersistentTarget(i) != null)
                hasPersistentTarget = true;
                break;
        }

        if (hasPersistentTarget) {
            preCombat.Invoke(); // execute pre-combat code
        }
        else {
            StartCoroutine(uiManager.StartCombat()); // starts combat
        }
    }

    // invoked as an animation event after the BattleStart popup
    public void StartCombat() {
        print("starting combat");
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
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene() {
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(1.5f);
        if (isGameWon) {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    bool TogglePauseMenu()
    {
        if (!showPauseMenu)
        {
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
        pauseSFX.Post(gameObject);
        showPauseMenu = true;
        uiManager.ShowPauseMenu();
        PauseGame();
    }

    public void HidePauseMenu() {
        unpSFX.Post(gameObject);
        showPauseMenu = false;
        uiManager.HidePauseMenu();
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

