using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    private int currentIdx;

    [SerializeField]
    public float changeCooldown;

    void Start()
    {
        InitMenu();
    }

    void InitMenu() {
        currentIdx = 0;
        HandleButtonChange();
    }

    void Update()
    {
        
    }

    void HandleButtonChange() {
        Transform newSelect = transform.GetChild(currentIdx);
        newSelect.gameObject.GetComponent<Button>().Select();
    }
}