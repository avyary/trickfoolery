using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public AK.Wwise.Event selectSFX;
    public AK.Wwise.Event pauseMUS;
    public AK.Wwise.Event stopMUS;
    private int currentIdx;
    private int numButtons;
    private bool isChanging;
    public bool buttonSelected = false;

    [SerializeField]
    public float changeCooldown;

    void Start()
    {
        currentIdx = 0;
        isChanging = false;
        numButtons = transform.childCount;
        pauseMUS.Post(gameObject);
        Time.timeScale = 1;
        StartCoroutine(HandleButtonChange(true));
    }

    void Update()
    {
        if (!buttonSelected && !isChanging) {
            if (Input.GetAxisRaw("Vertical") > 0)  // up
            {
                if (currentIdx == 0) {
                    return;
                }
                currentIdx -= 1;
                StartCoroutine(HandleButtonChange());
            }
            else if (Input.GetAxisRaw("Vertical") < 0)  // down
            {
                if (currentIdx == numButtons - 1) {
                    return;
                }
                currentIdx += 1;
                StartCoroutine(HandleButtonChange());
            }
        }
        else {
        }
    }

    IEnumerator HandleButtonChange(bool silent = false){
        Transform newSelect = transform.GetChild(currentIdx);
        if (!silent) {
            selectSFX.Post(gameObject); 
        }
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
