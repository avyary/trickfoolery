using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//*******************************************************************************************
// UIManager
//*******************************************************************************************
/// <summary>
/// Acts as the game's main View, handling the major UI mechanisms such as triggering
/// animations and visibility for the Hype meter, Win and Lose menus, end game popups,
/// and player life hearts.
/// </summary>
public class UIManager : MonoBehaviour
{
    public GameObject[] hearts;

    public GameObject jumbotronObj;
    private JumbotronController jumbotron;

    public GameObject jumbotronImg;
    public GameObject battleStart;
    public GameObject health;
    private GameObject pauseMenu;
    private GameObject lossSlideIn;
    private GameObject endPopup;

    [SerializeField]
    private GameObject loseMenuObj;
    [SerializeField]
    private Sprite winPopupImg;
    [SerializeField]
    private Sprite losePopupImg;
    // [SerializeField]
    // private GameObject gameOverMenuObj;

    private PauseMenuManager loseMenu;
    private int heartsActive;
    
    void Start()
    {
        jumbotronObj = GameObject.Find("Jumbotron");
        jumbotron = jumbotronObj.GetComponent<JumbotronController>();
        jumbotronImg = GameObject.Find("Image");
        battleStart = GameObject.Find("BattleStart");
        health = GameObject.Find("Health");
        heartsActive = health.transform.childCount;
        pauseMenu = GameObject.Find("PauseMenu");
        lossSlideIn = GameObject.Find("WinLossSlideIn");
        endPopup = GameObject.Find("EndPopup");
    }

    /// <summary>
    /// Resets the timescale and triggers the beginning combat UI animation sequence. Delays for a duration
    /// of time and exposes the jumbotron hype bar.
    /// </summary>
    public IEnumerator StartCombat() {
        Time.timeScale = 1;
        battleStart.GetComponent<Animator>().SetTrigger("StartGame");
        yield return new WaitForSecondsRealtime(0.4f);
        jumbotron.GetComponent<Animator>().SetTrigger("StartCombat");
    }

    /// <summary>
    /// Triggers the beginning combat UI animation sequence.
    /// </summary>
    public void StartCombatJumbotronless() {
        battleStart.GetComponent<Animator>().SetTrigger("StartGame");
    }

    /// <summary>
    /// Shows the jumbotron pause menu, adjusting the sprite UI image central to the GameObject, and
    /// initializes the pause menu through the PauseMenuManager.
    /// </summary>
    public void ShowPauseMenu() {
        if (jumbotron.state == JumbotronState.Hidden) {
            jumbotronObj.GetComponent<Animator>().SetTrigger("OpenFromHidden");
        }
        else {
            jumbotronObj.GetComponent<Animator>().SetTrigger("OpenPause");
        }
        jumbotronImg.GetComponent<Animator>().SetTrigger("ToCenter");
        pauseMenu.GetComponent<PauseMenuManager>().InitMenu();
    }

    /// <summary>
    /// Triggers the ending popup UI animation with the victory sprite.
    /// </summary>
    public void GameOverWin() {
        print("gameoverwin uima");
        endPopup.GetComponent<Image>().sprite = winPopupImg;
        endPopup.GetComponent<Animator>().SetTrigger("GameEnd");
    }

    /// <summary>
    /// Triggers the ending popup UI animation with the loss sprite.
    /// </summary>
    public void GameOverLose() {
        endPopup.GetComponent<Image>().sprite = losePopupImg;
        endPopup.GetComponent<Animator>().SetTrigger("GameEnd");
    }

    /// <summary>
    /// Enables and animates the lose menu to slide onscreen.
    /// </summary>
    public void ShowLoseMenu() {
        loseMenuObj.GetComponent<Animator>().SetTrigger("SlideIn");
        loseMenuObj.SetActive(true);
    }

    /// <summary>
    /// Disables and hides the pause menu, readjusting the sprite UI image to show the hype bar.
    /// </summary>
    public void HidePauseMenu() {
        if (jumbotron.state == JumbotronState.PauseFromHidden) {
            jumbotronObj.GetComponent<Animator>().SetTrigger("CloseToHidden");
        }
        else {
            jumbotronObj.GetComponent<Animator>().SetTrigger("ClosePause");
        }
        pauseMenu.GetComponent<PauseMenuManager>().CloseMenu();
        jumbotronObj.GetComponent<Animator>().SetTrigger("ClosePause");
        jumbotronImg.GetComponent<Animator>().SetTrigger("ToBottom");
    }

    /// <summary>
    /// Triggers the animation to show the level loss UI sprite.
    /// </summary>
    public void ShowLoss() {
        lossSlideIn.GetComponent<Animator>().SetTrigger("showLoss");
    }

    /// <summary>
    /// Calculates the remaining health from a percentage multiplied by the max heart count floored to the
    /// nearest integer and disables the associated heart UI sprites that had been lost.
    /// </summary>
    /// <param name="healthPercent"> The percentage of health remaining. </param>
    public void UpdateHealth(float healthPercent) {
        int newHearts = (int) Mathf.Floor(healthPercent * health.transform.childCount);
        if (newHearts < heartsActive) {
            for (int heartIdx = newHearts; heartIdx < heartsActive; heartIdx++) {
                //health.transform.GetChild(heartIdx).gameObject.GetComponent<Image>().color = Color.clear;
                health.transform.GetChild(heartIdx).gameObject.SetActive(false);
            }
        }
    }
}
