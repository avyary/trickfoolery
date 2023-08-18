using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//*******************************************************************************************
// PauseMenuButton
//*******************************************************************************************
/// <summary>
/// Handles the closing of the associated menu and exiting to the title screen with
/// associated animations. 
/// </summary>
public class PauseMenuButton : MonoBehaviour
{
    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    /// <summary>
    /// Loads the MainMenu scene with fade out animations.
    /// </summary>
    public void ReturnToMenu() {
        print("blaehh");
        StartCoroutine(MenuAfterDelay());
    }

    /// <summary>
    /// Triggers a UI animation to fade out and delays for a duration of time before loading the MainMenu
    /// scene and stopping the pause and level background music.
    /// </summary>
    IEnumerator MenuAfterDelay() {
        print("menuafterdelay");
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeOut");
        GameObject.Find("ProgressTracker").GetComponent<ProgressTracker>().isRestart = false;
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        /// posting the wwise events to stop the pause and level music before it goes over to the main menu
        gameManager.stopAaaMus.Post(gameObject);
        gameManager.stoppauseMUS.Post(gameObject);
    }

    /// <summary>
    /// Closes the pause menu through the GameManager.
    /// </summary>
    public void ClosePauseMenu() {
        gameManager.TogglePause();
    }
}
