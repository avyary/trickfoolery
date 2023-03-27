using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void FadeIn() {
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeIn");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void SpawnDodgeEnemies() {
        playerPos = player.transform.position;
        enemy1Pos = playerPos + new Vector3(1.5f, 0f, 1.5f);
        enemy2Pos = playerPos + new Vector3(-1.5f, 0f, -1.5f);
        enemy1 = GameObject.Instantiate(enemyPrefab, enemy1Pos, Quaternion.identity);
        enemy1.transform.rotation = Quaternion.LookRotation(player.transform.position - enemy1Pos);
        enemy2 = GameObject.Instantiate(enemyPrefab, enemy2Pos, Quaternion.identity);
        enemy2.transform.rotation = Quaternion.LookRotation(player.transform.position - enemy2Pos);
    }

    public void DodgeTutorial() {
        GameObject.Find("TutorialStart").SetActive(true);
        GameObject.Find("TutorialStart").GetComponent<Animator>().SetTrigger("StartGame");
        StartCoroutine(DelayStart());
    }

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

    public void ShowHypeMeter() {
        GameObject.Find("Jumbotron").GetComponent<Animator>().SetTrigger("StartCombat");
    }

    IEnumerator DelayStart() {
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1;
        enemy1.GetComponent<Enemy>().Attack(enemy1.GetComponent<Enemy>().currentAttack);
        enemy2.GetComponent<Enemy>().Attack(enemy2.GetComponent<Enemy>().currentAttack);
        gameManager.state = GameState.Tutorial;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0;
        gameManager.state = GameState.PreCombat;
        if (player.GetComponent<PlayerMovement>().health < player.GetComponent<PlayerMovement>().MAX_HEALTH) {
            GameObject.Find("Game Manager").GetComponent<DialogueManager>().StartDialogueScene("dodgeFail", DodgeTutorial);
        }
        else {
            GameObject.Find("Game Manager").GetComponent<DialogueManager>().StartDialogueScene("dodgeSuccess", uiManager.StartCombatJumbotronless);
        }
    }
}