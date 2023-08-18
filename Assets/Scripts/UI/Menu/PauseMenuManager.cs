using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//*******************************************************************************************
// PauseMenuManager
//*******************************************************************************************
/// <summary>
/// Handles the main Pause functionalities and animations, enabling and disabling
/// buttons on the opening and closing of the menu, and exiting to the title screen. 
/// </summary>
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

    /// <summary>
    /// Sets all buttons associated with this menu as interactable and selects the first button from the
    /// menu. Logs the menu's state and related button IDs.
    /// </summary>
    public void InitMenu() {
        print("initMenu");
        isActive = true;
        for (int buttonIdx = 0; buttonIdx < transform.childCount; buttonIdx++) {
            print(buttonIdx);
            transform.GetChild(buttonIdx).GetComponent<Button>().interactable = true;
        }
        HandleButtonChange(0);
    }

    /// <summary>
    /// Closes the menu and disables all the buttons associated with it.
    /// </summary>
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

    /// <summary>
    /// Sets the button as selectable with a provided button ID.
    /// </summary>
    /// <param name="newIdx"> The ID of the button to be selected. </param>
    void HandleButtonChange(int newIdx) {
        currentIdx = newIdx;
        transform.GetChild(newIdx).gameObject.GetComponent<Button>().Select();
    }

    /// <summary>
    /// Loads the MainMenu scene with fade out animations.
    /// </summary>
    public void ReturnToMenu() {
        StartCoroutine(MenuAfterDelay());
    }

    /// <summary>
    /// Triggers a UI animation to fade out, resets the ProgressTracker, and delays for a duration of time
    /// before loading the MainMenu scene.
    /// </summary>
    IEnumerator MenuAfterDelay() {
        print("menuafterdelay");
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeOut");
        GameObject.Find("ProgressTracker").GetComponent<ProgressTracker>().isRestart = false;
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    /// <summary>
    /// Closes the pause menu through the GameManager.
    /// </summary>
    public void ClosePauseMenu() {
        gameManager.TogglePause();
    }
}