using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButton : MonoBehaviour
{
    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void ReturnToMenu() {
        print("blaehh");
        StartCoroutine(MenuAfterDelay());
    }

    IEnumerator MenuAfterDelay() {
        print("menuafterdelay");
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeOut");
        GameObject.Find("ProgressTracker").GetComponent<ProgressTracker>().isRestart = false;
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ClosePauseMenu() {
        gameManager.TogglePause();
    }
}
