using System.Collections;
using UnityEngine;

// *******************************************************************************************
// Lvl1DialogueTriggers
//*******************************************************************************************
/// <summary>
/// Handles the logic behind the game tutorial aside from the dialogue, such as spawning
/// the enemies and resetting the tutorial enemies until the player completes the tutorial.
/// </summary>
public class Lvl1DialogueTriggers: MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;

    private GameObject enemy1;
    private GameObject enemy2;

    private GameManager gameManager;
    private DialogueManager DialogueManager;
    private UIManager uiManager;
    
    private GameObject player;

    private Vector3 enemy1Pos;
    private Vector3 enemy2Pos;
    private Vector3 playerPos;

    void Start() {
        player = GameObject.Find("Player (1)");
        uiManager = GameObject.Find("Game Manager").GetComponent<UIManager>();
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeIn");
    }

    /// <summary>
    /// Triggers an animation to fade in the Canvas to reveal the level on the scene load and caches the
    /// GameManager reference.
    /// </summary>
    public void FadeIn() {
        GameObject.Find("DialogueFadeIn").GetComponent<Animator>().SetTrigger("FadeIn");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    /// <summary>
    /// Instantiates an Enemy type of <i> enemyPrefab </i> on each side of the player that faces toward the
    /// player GameObject.
    /// </summary>
    public void SpawnDodgeEnemies() {
        playerPos = player.transform.position;
        enemy1Pos = playerPos + new Vector3(1.5f, 0f, 1.5f);
        enemy2Pos = playerPos + new Vector3(-1.5f, 0f, -1.5f);
        enemy1 = GameObject.Instantiate(enemyPrefab, enemy1Pos, Quaternion.identity);
        enemy1.transform.rotation = Quaternion.LookRotation(player.transform.position - enemy1Pos);
        enemy2 = GameObject.Instantiate(enemyPrefab, enemy2Pos, Quaternion.identity);
        enemy2.transform.rotation = Quaternion.LookRotation(player.transform.position - enemy2Pos);
    }

    /// <summary>
    /// Begins the tutorial to dodge attacking Enemy types and triggers associated tutorial start UI
    /// animations.
    /// </summary>
    public void DodgeTutorial() {
        GameObject.Find("TutorialStart").SetActive(true);
        GameObject.Find("TutorialStart").GetComponent<Animator>().SetTrigger("StartGame");
        StartCoroutine(DelayStart());
    }

    /// <summary>
    /// Resets the tutorial to dodge attacking Enemy types by resetting the player health, destroying
    /// the old enemy GameObjects, and respawning new Enemy GameObjects.
    /// </summary>
    public void ResetDodgeTutorial() {
        player.GetComponent<PlayerMovement>().health = 100;
        uiManager.UpdateHealth(1f);
        if (enemy1) {
            Destroy(enemy1);
        }
        if (enemy2) {
            Destroy(enemy2);
        }
        SpawnDodgeEnemies();
    }

    /// <summary>
    /// Triggers animations to reveal the Jumbotron hype meter UI.
    /// </summary>
    public void ShowHypeMeter() {
        GameObject.Find("Jumbotron").GetComponent<Animator>().SetTrigger("StartCombat");
    }

    /// <summary>
    /// Performs the following sequence of operations:
    /// <p> 1. Delays activity for a duration of time before activating the Enemy attacks and firing the
    /// GameManager <i> startTutorialEvent </i> delegate. </p>
    /// <p> 2. Delays activity for a duration of time before firing the GameManager <i> stopCombatEvent </i> to
    /// freeze the player input and Enemy activity. </p>
    /// <p> 3. Progresses the dialogue if the player's health stayed at the maximum value, meaning a
    /// dodge was successfully performed. Otherwise, triggers the dialogue event to restart the tutorial. </p>
    /// </summary>
    IEnumerator DelayStart() {
        yield return new WaitForSeconds(1.5f);
        enemy1.GetComponent<Enemy>().Attack(enemy1.GetComponent<Enemy>().currentAttack);
        enemy2.GetComponent<Enemy>().Attack(enemy2.GetComponent<Enemy>().currentAttack);
        gameManager.startTutorialEvent.Invoke();
        yield return new WaitForSeconds(2f);
        gameManager.stopCombatEvent.Invoke();
        if (player.GetComponent<PlayerMovement>().health < player.GetComponent<PlayerMovement>().MAX_HEALTH) {
            GameObject.Find("Game Manager").GetComponent<DialogueManager>().StartDialogueScene("dodgeFail", DodgeTutorial);
        }
        else {
            GameObject.Find("Game Manager").GetComponent<DialogueManager>().StartDialogueScene("dodgeSuccess", uiManager.StartCombatJumbotronless);
        }
    }
}