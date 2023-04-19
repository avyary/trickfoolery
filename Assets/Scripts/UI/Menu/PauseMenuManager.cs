using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private int currentIdx;
    private bool isActive = false;
    private GameManager gameManager;

    [SerializeField]
    public float changeCooldown;
    
    /// wwise event declaration for stopping pause and level music when exiting to main menu


    void Start()
    {
        isActive = false;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        CloseMenu();
    }

    public void InitMenu() {
        print("initMenu");
        isActive = true;
        for (int buttonIdx = 0; buttonIdx < transform.childCount; buttonIdx++) {
            print(buttonIdx);
            transform.GetChild(buttonIdx).GetComponent<Button>().interactable = true;
        }
        HandleButtonChange(0);
    }

    public void CloseMenu() {
        print("closeMenu");
        isActive = false;
        for (int buttonIdx = 0; buttonIdx < transform.childCount; buttonIdx++) {
            transform.GetChild(buttonIdx).GetComponent<Button>().interactable = false;
        }
    }

    void Update() {
        if (isActive) {
            if (Input.GetAxisRaw("Vertical") < 0)  // right
            {
                HandleButtonChange(1);
            }
            else if (Input.GetAxisRaw("Vertical") > 0)  // left
            {
                HandleButtonChange(0);
            }
        }
    }

    void HandleButtonChange(int newIdx) {
        currentIdx = newIdx;
        transform.GetChild(newIdx).gameObject.GetComponent<Button>().Select();
    }

    public void ReturnToMenu() {
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