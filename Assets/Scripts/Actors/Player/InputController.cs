using UnityEngine;

//*******************************************************************************************
// InputController
//*******************************************************************************************
/// <summary>
/// Handles player input associated with the game system, such as pausing and
/// progressing through dialogue.
/// </summary>
public class InputController : MonoBehaviour
{
    private GameManager gameManager;
    private DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        GameObject managers = GameObject.Find("Game Manager");
        gameManager = managers.GetComponent<GameManager>();
        dialogueManager = managers.GetComponent<DialogueManager>();
    }

    /// <summary>
    /// Triggers the progression of the dialogue system through the DialogueManager.
    /// </summary>
    public void OnConfirm() {
        dialogueManager.OnConfirm();
    }

    /// <summary>
    /// Pauses or unpauses the game through the GameManager.
    /// </summary>
    public void OnPause() {
        gameManager.OnPause();
    }

}
