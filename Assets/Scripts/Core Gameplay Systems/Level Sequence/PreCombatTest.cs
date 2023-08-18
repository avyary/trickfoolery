using UnityEngine;

// *******************************************************************************************
// PreCombatTest
//*******************************************************************************************
/// <summary>
/// Specifies dialogue to play for Lvl1PreCombat and starts the battle in the UIManager
/// upon dialogue completion.
/// </summary>
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

    /// <summary>
    /// Triggers the dialogue <i> tutorial0 </i> event with a delegate to invoke to trigger UI animations
    /// associated with the start of a battle.
    /// </summary>
    public void Test() {
        DialogueManager.StartDialogueScene("tutorial0", Sequence);
        // StartCoroutine(Sequence());
    }

    /// <summary>
    /// Begins the dodge tutorial through the Lvl1DialogueTriggers.
    /// </summary>
    private void Sequence() {
        lvl1DialogueTriggers.DodgeTutorial();
        // StartCoroutine(uiManager.StartCombat()); // starts combat
    }
}
