using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lvl2PreCombat : MonoBehaviour
{
    public UIManager uiManager;
    private DialogueManager DialogueManager;

    void Start() {
        uiManager = GameObject.Find("Game Manager").GetComponent<UIManager>();
        DialogueManager = GameObject.Find("Game Manager").GetComponent<DialogueManager>();
    }

    public void Test() {
        DialogueManager.StartDialogueScene("intro", Sequence);
    }

    private void Sequence() {
        StartCoroutine(uiManager.StartCombat()); // starts combat
    }
}
