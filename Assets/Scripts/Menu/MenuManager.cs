using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private int currentIdx = 0;
    private int numButtons;

    void Start()
    {
        numButtons = transform.childCount;
        HandleButtonChange();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            SceneManager.LoadScene("AlphaEnvironmentMar1", LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentIdx == 0) {
                currentIdx = numButtons - 1;
            }
            else {
                currentIdx -= 1;
            }
            HandleButtonChange();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentIdx == numButtons - 1) {
                currentIdx = 0;
            }
            else {
                currentIdx += 1;
            }
            HandleButtonChange();
        }
    }

    public void HandleButtonChange(int? idx = null){
        if (idx == null) {
            idx = currentIdx;
        }
        Transform newSelect = transform.GetChild((int) idx);
        newSelect.gameObject.GetComponent<Button>().Select();
    }

    public void OnButtonHover(GameObject button){
        button.GetComponent<Button>().Select();
    }

    public void OnButtonUnHover(){
        HandleButtonChange();
    }
}
