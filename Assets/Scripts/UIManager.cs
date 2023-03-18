using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerMovement player;

    public GameObject[] hearts;

    public int numberOfHearts;
    public int[] heartChunks;

    public int health;

    public GameObject jumbotron;
    public GameObject jumbotronImg;
    public GameObject battleStart;
    
    void Start()
    {
        jumbotron = GameObject.Find("Jumbotron");
        jumbotronImg = GameObject.Find("Image");
        battleStart = GameObject.Find("BattleStart");

        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        numberOfHearts = hearts.Length;
        health =  Convert.ToInt32(player.MAX_HEALTH);

        heartChunks = new int[numberOfHearts];
        int chunkSize = health / numberOfHearts;
        for (int i = 0; i < numberOfHearts; i++)
        {
            heartChunks[i] = chunkSize * (i + 1); // Assign each part with the same size

            if (i < health % numberOfHearts)
            {
                // Add one to the current part if the remainder is not zero
                heartChunks[i]++;
            }
        }
    }

    public IEnumerator StartCombat() {
        battleStart.GetComponent<Animator>().SetTrigger("StartGame");
        yield return new WaitForSecondsRealtime(0.4f);
        jumbotron.GetComponent<Animator>().SetTrigger("StartCombat");
    }

    public void ShowPauseMenu() {
        jumbotron.GetComponent<Animator>().SetTrigger("OpenPause");
        jumbotronImg.GetComponent<Animator>().SetTrigger("ToCenter");
    }

    public void HidePauseMenu() {
        jumbotron.GetComponent<Animator>().SetTrigger("ClosePause");
        jumbotronImg.GetComponent<Animator>().SetTrigger("ToBottom");
    }

    void Update()
    {
        health = Convert.ToInt32(player.health);
        if (health < heartChunks[3])
        {
            Destroy(hearts[3].gameObject);
        }
        if (health < heartChunks[2])
            Destroy(hearts[2].gameObject);
        if (health < heartChunks[1])
            Destroy(hearts[1].gameObject);
        if (health < heartChunks[0])
            Destroy(hearts[0].gameObject);
    }
}
