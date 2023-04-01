using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStart : MonoBehaviour
{
    public void StartBattle() {
        print("starting battle lol");
        GameObject.Find("Game Manager").GetComponent<GameManager>().StartCombat();
    }

    public void ShowPanel() {
        gameObject.SetActive(true);
    }

    public void HidePanel() {
        gameObject.GetComponent<Image>().color = Color.clear;
    }
}
