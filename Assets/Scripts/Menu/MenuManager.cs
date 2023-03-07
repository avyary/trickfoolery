using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private int currentIdx = 0;
    private int numButtons;
    private bool isChanging = false;

    [SerializeField]
    public float changeCooldown;

    void Start()
    {
        numButtons = transform.childCount;
        StartCoroutine(HandleButtonChange());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            SceneManager.LoadScene("AlphaEnvironmentMar1", LoadSceneMode.Single);
        }
        if (!isChanging) {
            if (Input.GetAxisRaw("Vertical") > 0)  // up
            {
                if (currentIdx > 0) {
                    currentIdx -= 1;
                }
                StartCoroutine(HandleButtonChange());
            }
            else if (Input.GetAxisRaw("Vertical") < 0)  // down
            {
                if (currentIdx < numButtons - 1) {
                    currentIdx += 1;
                }
                StartCoroutine(HandleButtonChange());
            }
        }
    }

    IEnumerator HandleButtonChange(int? idx = null){
        if (idx == null) {
            idx = currentIdx;
        }
        Transform newSelect = transform.GetChild((int) idx);
        newSelect.gameObject.GetComponent<Button>().Select();
        isChanging = true;
        print("hi");
        yield return new WaitForSeconds(changeCooldown);
        isChanging = false;
        print("bye");
    }

    public void OnButtonHover(GameObject button){
        button.GetComponent<Button>().Select();
    }

    public void OnButtonUnHover(){
        HandleButtonChange();
    }
}
