using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleStart : MonoBehaviour
{
    public void StartBattle() {
        GameObject.Find("Game Manager").GetComponent<GameManager>().startCombatEvent.Invoke();
    }

    public void ShowPanel() {
        gameObject.SetActive(true);
    }

    public void HidePanel() {
        gameObject.GetComponent<Image>().color = Color.clear;
    }
}
