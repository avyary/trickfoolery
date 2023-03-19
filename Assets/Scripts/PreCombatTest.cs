using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreCombatTest : MonoBehaviour
{
    public UIManager uiManager;

    void Start() {
        uiManager = GameObject.Find("Game Manager").GetComponent<UIManager>();
    }

    public void Test() {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence() {
        yield return new WaitForSecondsRealtime(5f);
        StartCoroutine(uiManager.StartCombat()); // starts combat
    }
}
