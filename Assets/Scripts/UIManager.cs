using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject[] hearts;

    public GameObject jumbotron;
    public GameObject jumbotronImg;
    public GameObject battleStart;
    public GameObject health;
    private GameObject pauseMenu;

    private int heartsActive;

    
    void Start()
    {
        jumbotron = GameObject.Find("Jumbotron");
        jumbotronImg = GameObject.Find("Image");
        battleStart = GameObject.Find("BattleStart");
        health = GameObject.Find("Health");
        heartsActive = health.transform.childCount;
        pauseMenu = GameObject.Find("PauseMenu");
    }

    public IEnumerator StartCombat() {
        Time.timeScale = 1;
        battleStart.GetComponent<Animator>().SetTrigger("StartGame");
        yield return new WaitForSecondsRealtime(0.4f);
        jumbotron.GetComponent<Animator>().SetTrigger("StartCombat");
    }

    public void StartCombatJumbotronless() {
        print("jumbotronlessing");
        battleStart.GetComponent<Animator>().SetTrigger("StartGame");
    }

    public void ShowPauseMenu() {
        jumbotron.GetComponent<Animator>().SetTrigger("OpenPause");
        jumbotronImg.GetComponent<Animator>().SetTrigger("ToCenter");
        pauseMenu.GetComponent<PauseMenuManager>().InitMenu();
    }

    public void HidePauseMenu() {
        pauseMenu.GetComponent<PauseMenuManager>().CloseMenu();
        jumbotron.GetComponent<Animator>().SetTrigger("ClosePause");
        jumbotronImg.GetComponent<Animator>().SetTrigger("ToBottom");
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
