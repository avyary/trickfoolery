using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart : MonoBehaviour
{
    public void StartBattle() {
        GameObject.Find("Game Manager").GetComponent<GameManager>().StartCombat();
    }
}
