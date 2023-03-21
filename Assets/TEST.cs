using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    public void HITHERE() {
        gameManager.StartCombat();
    }
}
