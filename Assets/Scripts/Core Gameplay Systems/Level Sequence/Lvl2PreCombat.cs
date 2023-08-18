using UnityEngine;

// *******************************************************************************************
// Lvl2PreCombat
//*******************************************************************************************
/// <summary>
/// Specifies dialogue to play for Lvl2 and starts the battle in the UIManager upon
/// dialogue completion.
/// </summary>
public class Lvl2PreCombat : MonoBehaviour
{
    public UIManager uiManager;
    private DialogueManager DialogueManager;

    void Start() {
        uiManager = GameObject.Find("Game Manager").GetComponent<UIManager>();
        DialogueManager = GameObject.Find("Game Manager").GetComponent<DialogueManager>();
    }

    /// <summary>
    /// Triggers the dialogue <i> intro </i> event with a delegate to invoke to trigger UI animations
    /// associated with the start of a battle.
    /// </summary>
    public void Test() {
        DialogueManager.StartDialogueScene("intro", Sequence);
    }

    /// <summary>
    /// Triggers the UI animations associated with the beginning of combat through the UIManager.
    /// </summary>
    private void Sequence() {
        StartCoroutine(uiManager.StartCombat()); // starts combat
    }
}
