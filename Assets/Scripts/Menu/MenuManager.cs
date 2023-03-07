using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private int currentIdx;
    private int numButtons;
    private bool isChanging;

    [SerializeField]
    public float changeCooldown;

    void Start()
    {
        currentIdx = 0;
        isChanging = false;
        numButtons = transform.childCount;
        StartCoroutine(HandleButtonChange());
    }

    void Update()
    {
        if (!isChanging) {
            if (Input.GetAxisRaw("Vertical") > 0)  // up
            {
                print("up");
                if (currentIdx > 0) {
                    currentIdx -= 1;
                }
                StartCoroutine(HandleButtonChange());
            }
            else if (Input.GetAxisRaw("Vertical") < 0)  // down
            {
                print("down");
                if (currentIdx < numButtons - 1) {
                    currentIdx += 1;
                }
                StartCoroutine(HandleButtonChange());
            }
        }
        else {
            print("uhoh");
        }
    }

    IEnumerator HandleButtonChange(int? idx = null){
        if (idx == null) {
            idx = currentIdx;
        }
        Transform newSelect = transform.GetChild((int) idx);
        newSelect.gameObject.GetComponent<Button>().Select();
        isChanging = true;
        yield return new WaitForSeconds(changeCooldown);
        isChanging = false;
    }

    public void OnButtonHover(GameObject button){
        button.GetComponent<Button>().Select();
    }

    public void OnButtonUnHover(){
        HandleButtonChange();
    }
}
