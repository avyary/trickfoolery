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
        StartCoroutine(MenuAfterDelay());
    }

    IEnumerator MenuAfterDelay() {
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ClosePauseMenu() {
        gameManager.HidePauseMenu();
    }
}
