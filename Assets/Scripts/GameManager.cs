using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState {
    Tutorial,
    OutOfCombat,
    Combat
}

public class GameManager : MonoBehaviour
{
    // state variables
    public GameState state = GameState.OutOfCombat;
    public bool isPaused = true;
    public bool showPauseMenu = false;
    private bool isGameWon = false;
    private bool inCombat = false;
    public bool playerInput = false;
    public bool isGameOver = false;
    
    // wwise
    public AK.Wwise.Event pauseSFX;
    public AK.Wwise.Event unpSFX;
    public AK.Wwise.Event playpauseMUS;
    public AK.Wwise.Event mutepauseMUS;
    public AK.Wwise.Event firstmutepauseMUS;
    public AK.Wwise.Event unmpauseMUS;


    // object references
    public UIManager uiManager;
    private DialogueManager dialogueManager;
    private JumbotronController jumbotron;
    private PlayerMovement player;

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
    [SerializeField]
    public GameObject trackerPrefab;

    // events for listeners
    [SerializeField]
    public UnityEvent startCombatEvent;
    [SerializeField]
    public UnityEvent startTutorialEvent;
    [SerializeField]
    public UnityEvent stopCombatEvent;

    private bool isRestart;

    void Start()
    {
        uiManager = gameObject.GetComponent<UIManager>();
        dialogueManager = gameObject.GetComponent<DialogueManager>();
        jumbotron = GameObject.Find("Jumbotron").GetComponent<JumbotronController>();

        GameObject progressTracker = GameObject.Find("ProgressTracker");
        if (progressTracker) {
            isRestart = progressTracker.GetComponent<ProgressTracker>().isRestart;
        }
        else {
            isRestart = false;
        }

        playpauseMUS.Post(gameObject);
        firstmutepauseMUS.Post(gameObject);
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

        if (hasPersistentTarget && !isRestart) {
            preCombat.Invoke(); // execute pre-combat code
        }
        else {
            // TODO: make this.. better
            GameObject dialogueFade = GameObject.Find("DialogueFadeIn");
            if (dialogueFade) {
                dialogueFade.SetActive(false);
            }
            StartCoroutine(uiManager.StartCombat()); // starts combat
        }
    }

    public void StartCombat() {
        Time.timeScale = 1;
        state = GameState.Combat;
        playerInput = true;
    }

    public void StartTutorial() {
        state = GameState.Tutorial;
        playerInput = true;
    }

    public void StopCombat() {
        state = GameState.OutOfCombat;
        playerInput = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) {
            print("GAME STATE: " + state + "\nisPaused: " + isPaused + " | jumbotron state: " + jumbotron.state + " | timeScale: " + Time.timeScale);
        }
        // if jumbotron is disabled, cannot pause
        if (Input.GetButtonDown("Escape") && jumbotron.state != JumbotronState.Disabled)
        {
            TogglePause();
        }
        if (state == GameState.Combat && GameObject.FindGameObjectsWithTag("Enemy").Length < minEnemyNumber)
        {
            SpawnRandomEnemy();
        }
    }

    IEnumerator LoadNextScene() {
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(1.5f);
        if (isGameWon) {
            GameObject.Find("ProgressTracker").GetComponent<ProgressTracker>().isRestart = false;
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
        else {
            GameObject.Find("ProgressTracker").GetComponent<ProgressTracker>().isRestart = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void TriggerLoadNextScene() {
        StartCoroutine(LoadNextScene());
    }

    public void TogglePause() {
        if (jumbotron.state == JumbotronState.Hidden || jumbotron.state == JumbotronState.HypeBar) {
            pauseSFX.Post(gameObject);
            unmpauseMUS.Post(gameObject);

            uiManager.ShowPauseMenu();
            isPaused = true;
            Time.timeScale = 0;
        }
        else if (jumbotron.state == JumbotronState.Pause || jumbotron.state == JumbotronState.PauseFromHidden) {
            unpSFX.Post(gameObject);
            mutepauseMUS.Post(gameObject);
            uiManager.HidePauseMenu();
            Time.timeScale = 1;
        }
        else {
            return;
        }

        jumbotron.state = JumbotronState.Disabled;  // for animation duration
    }

    public IEnumerator GameOverWin()
    {
        isGameOver = true;
        isGameWon = true;
        yield return new WaitForSeconds(0.5f);
        stopCombatEvent.Invoke();
        uiManager.GameOverWin();
        yield return new WaitForSeconds(1f);
        dialogueManager.StartDialogueScene("onWin", TriggerLoadNextScene);
    }

    public IEnumerator GameOverLose()
    {
        isGameOver = true;
        stopCombatEvent.Invoke();
        yield return new WaitForSeconds(3f);
        uiManager.GameOverLose();
        yield return new WaitForSeconds(1f);
        dialogueManager.StartDialogueScene("onLoss", ShowLoseMenu);
    }

    void ShowLoseMenu() {
        print("show lose menu");
        jumbotron.state = JumbotronState.Disabled;
        uiManager.ShowLoseMenu();
        isPaused = true;
        Time.timeScale = 0;
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

