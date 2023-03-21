using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    private int currentIdx;
    private bool isActive = false;

    [SerializeField]
    public float changeCooldown;

    void Start()
    {
        CloseMenu();
    }

    public void InitMenu() {
        isActive = true;
        for (int buttonIdx = 0; buttonIdx < transform.childCount; buttonIdx++) {
            transform.GetChild(buttonIdx).GetComponent<Button>().interactable = true;
        }
        HandleButtonChange(0);
    }

    public void CloseMenu() {
        isActive = false;
        for (int buttonIdx = 0; buttonIdx < transform.childCount; buttonIdx++) {
            transform.GetChild(buttonIdx).GetComponent<Button>().interactable = false;
        }
    }

    void Update() {
        if (Input.GetAxisRaw("Horizontal") > 0)  // right
        {
            HandleButtonChange(1);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)  // left
        {
            HandleButtonChange(0);
        }
    }

    void HandleButtonChange(int newIdx) {
        currentIdx = newIdx;
        transform.GetChild(newIdx).gameObject.GetComponent<Button>().Select();
    }
}