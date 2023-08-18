using UnityEngine;

//*******************************************************************************************
// OpeningCutscene
//*******************************************************************************************
/// <summary>
/// Handles the animations and events outside of the CutsceneManager associated with 
/// the OpeningCutscene.
/// </summary>
public class OpeningCutscene : MonoBehaviour
{
    private CutsceneManager cutsceneManager;

    void Start() {
        cutsceneManager = GameObject.Find("Canvas").GetComponent<CutsceneManager>();
    }

    /// <summary>
    /// Triggers background cutscene animations according the current played cutscene dialogue. 
    /// </summary>
    public void CheckForTrigger() {
        if (cutsceneManager.pageIdx == 2) {
            GameObject.Find("Disco1").GetComponent<Animator>().SetTrigger("LightsActive");
        }
        else if (cutsceneManager.pageIdx == 4) {
            GameObject.Find("Disco2").GetComponent<Animator>().SetTrigger("LightsActive");
        }
    }
}
