using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCutscene : MonoBehaviour
{
    private CutsceneManager cutsceneManager;

    void Start() {
        cutsceneManager = GameObject.Find("Canvas").GetComponent<CutsceneManager>();
    }

    public void CheckForTrigger() {
        if (cutsceneManager.pageIdx == 2) {
            GameObject.Find("Disco1").GetComponent<Animator>().SetTrigger("LightsActive");
        }
        else if (cutsceneManager.pageIdx == 4) {
            GameObject.Find("Disco2").GetComponent<Animator>().SetTrigger("LightsActive");
        }
    }
}
