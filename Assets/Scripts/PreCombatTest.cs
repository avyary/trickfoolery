using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreCombatTest : MonoBehaviour
{
    public UIManager uiManager;
    private DialogueManager DialogueManager;

    private Lvl1DialogueTriggers lvl1DialogueTriggers;

    void Start() {
        uiManager = GameObject.Find("Game Manager").GetComponent<UIManager>();
        DialogueManager = GameObject.Find("Game Manager").GetComponent<DialogueManager>();
        lvl1DialogueTriggers = gameObject.GetComponent<Lvl1DialogueTriggers>();
    }

    public void Test() {
        DialogueManager.StartDialogueScene("tutorial0", Sequence);
        // StartCoroutine(Sequence());
    }

    private void Sequence() {
        lvl1DialogueTriggers.DodgeTutorial();
        // StartCoroutine(uiManager.StartCombat()); // starts combat
    }
}
