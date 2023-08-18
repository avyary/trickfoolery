using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//*******************************************************************************************
// MenuManager
//*******************************************************************************************
/// <summary>
/// Handles the selection of buttons associated with input as well as SFX.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public AK.Wwise.Event selectSFX;
    public AK.Wwise.Event pauseMUS;
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
    }

    /// <summary>
    /// Triggers associated button selection animations and SFX according to the provided flag and
    /// sets the corresponding button as selectable before delaying by <i> changeCooldown </i> time to
    /// switch buttons at a human-perceivable rate.
    /// </summary>
    /// <param name="silent"> Specifies that the button should not trigger animations and SFX. </param>
    IEnumerator HandleButtonChange(bool silent = false){
        Transform newSelect = transform.GetChild(currentIdx);
        if (!silent) {
            newSelect.gameObject.GetComponent<Animator>().SetTrigger("SelectButton");
            selectSFX.Post(gameObject); 
        }
        newSelect.gameObject.GetComponent<Button>().Select();
        isChanging = true;
        yield return new WaitForSeconds(changeCooldown);
        isChanging = false;
    }

    /// <summary>
    /// Sets the corresponding button as selectable.
    /// </summary>
    /// <param name="button"> The button GameObject to be selected. </param>
    public void OnButtonHover(GameObject button){
        button.GetComponent<Button>().Select();
    }

    /// <summary>
    /// Selects a button according to input with associated animations and SFX.
    /// </summary>
    public void OnButtonUnHover(){
        HandleButtonChange();
    }
}
