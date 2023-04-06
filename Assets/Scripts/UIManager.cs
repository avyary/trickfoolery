using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

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

    public IEnumerator StartCombat() {
        Time.timeScale = 1;
        battleStart.GetComponent<Animator>().SetTrigger("StartGame");
        yield return new WaitForSecondsRealtime(0.4f);
        jumbotron.GetComponent<Animator>().SetTrigger("StartCombat");
    }

    public void StartCombatJumbotronless() {
        battleStart.GetComponent<Animator>().SetTrigger("StartGame");
    }

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

    public void GameOverWin() {
        print("gameoverwin uima");
        endPopup.GetComponent<Image>().sprite = winPopupImg;
        endPopup.GetComponent<Animator>().SetTrigger("GameEnd");
    }

    public void GameOverLose() {
        endPopup.GetComponent<Image>().sprite = losePopupImg;
        endPopup.GetComponent<Animator>().SetTrigger("GameEnd");
    }

    public void ShowLoseMenu() {
        loseMenuObj.GetComponent<Animator>().SetTrigger("SlideIn");
        loseMenuObj.SetActive(true);
        loseMenuObj.transform.GetChild(2).GetComponent<PauseMenuManager>().InitMenu();
    }

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

    public void ShowLoss() {
        lossSlideIn.GetComponent<Animator>().SetTrigger("showLoss");
    }

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
