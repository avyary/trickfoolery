using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void OnConfirm() {
        dialogueManager.OnConfirm();
    }

    public void OnPause() {
        gameManager.OnPause();
    }

}
