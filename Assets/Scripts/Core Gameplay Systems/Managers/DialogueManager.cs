using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

// *******************************************************************************************
// DialogueManager
//*******************************************************************************************
/// <summary>
/// Handles the parsing and execution of TextAssets associated with the game dialogue
/// system. Supports dialogue cutscene portraits, character dialogue portraits, named
/// dialogue bubbles, etc.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueUI;
    [SerializeField]
    private GameObject nameTextObj;
    [SerializeField]
    private GameObject speechTextObj;
    [SerializeField]
    private GameObject portraitObj;
    [SerializeField]
    private GameObject arrowObj;
    [SerializeField]
    private string jsonName;

    private LoadedDScenes loadedDScenes;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI speechText;
    private Image portraitImg;
    private GameManager gameManager;
    private Image arrowImg;

    private bool sceneIsActive = false;
    private int lineIdx = 0;
    private DScene targetScene;

    private System.Action sceneCallback;
    
    private bool inDelay = false;

    [SerializeField]
    UnityEvent[] triggers;

    // Start is called before the first frame update
    void Start()
    {
        nameText = nameTextObj.GetComponent<TextMeshProUGUI>();
        speechText = speechTextObj.GetComponent<TextMeshProUGUI>();
        portraitImg = portraitObj.GetComponent<Image>();
        arrowImg = arrowObj.GetComponent<Image>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        TextAsset jsonObj = Resources.Load<TextAsset>(System.String.Format("Dialogue/{0}", jsonName));
        if (jsonObj is null) {
            Debug.Log(System.String.Format("Could not load dialogue scenes from {0}.json", jsonName));
        }
        else {
            loadedDScenes = JsonUtility.FromJson<LoadedDScenes>(jsonObj.text);
        }
        dialogueUI.SetActive(false);
    }

    /// <summary>
    /// Progresses the dialogue scene if the game is not currently paused, the <i> inDelay </i> flag is
    /// cleared, or if the dialogue scene has been toggled as active and accepting input.
    /// </summary>
    public void OnConfirm() {
        print("onconfirm");
        if (sceneIsActive && !gameManager.isPaused && !inDelay) {
            StartCoroutine(ProgressScene());
        }
    }

    /// <summary>
    /// Caches the callback to invoke when the dialogue scene ends and does nothing if the scene is already
    /// active. Otherwise, finds the scene to play from all the loaded scenes by the provided scene name and
    /// enables the dialogue UI, toggles the <i> sceneIsActive </i> flag and sets the beginning <i> lineIdx
    /// </i>, and automatically progresses the scene to populate the dialogue with text. Logs error data
    /// if the scene cannot be found. 
    /// </summary>
    /// <param name="sceneName"> The name of the dialogue scene to search for. </param>
    /// <param name="callback"> The delegate that references a method to be invoked at the end of the
    /// dialogue scene. </param>
    public void StartDialogueScene(string sceneName, System.Action callback = null)
    {
        print("starting dialogue scene");
        sceneCallback = callback;
        if (sceneIsActive) {
            return;
        }
        targetScene = System.Array.Find(loadedDScenes.scenes, i => i.sceneName == sceneName);
        if (targetScene is null) {
            Debug.Log(System.String.Format("Could not find scene with name {0}", sceneName));
            return;
        }
        dialogueUI.SetActive(true);
        sceneIsActive = true;
        
        lineIdx = 0;
        StartCoroutine(ProgressScene(0));
    }

    /// <summary>
    /// Increments the current dialogue line index and loads the next line of dialogue corresponding to
    /// the line index. If event triggers are detected to occur instead of the next dialogue line, fires the
    /// corresponding delegates and delays for the <i> delay </i> value stored in the dialogue line type
    /// before enabling the progression of the dialogue. Otherwise, sets the dialogue text box UI with the
    /// corresponding character name, dialogue text, and sprite.
    /// </summary>
    /// <param name="jump"> The amount to increment the current dialogue line index. </param>
    IEnumerator ProgressScene(int jump = 1) {
        if (!sceneIsActive) {
            yield return null;
        }
        lineIdx += jump;
        if (lineIdx >= targetScene.lines.Length) {
            EndScene();
            yield return null;
        }
        else {
            DLine newLine = targetScene.lines[lineIdx];
            if (newLine.trigger) {
                triggers[newLine.triggerIdx].Invoke();
                if (newLine.delay > 0f) {
                    inDelay = true;
                    arrowImg.color = Color.clear;
                    yield return new WaitForSeconds(newLine.delay);
                    arrowImg.color = Color.black;
                    inDelay = false;
                }
            }
            else {
                nameText.text = newLine.name;
                speechText.text = newLine.text;
                portraitImg.sprite = Resources.Load<Sprite>("Dialogue/Portraits/" + newLine.portrait);
                yield return null;
            }
        }
    }

    /// <summary>
    /// Disables the dialogue UI and clears the "sceneIsActive" flag before invoking the
    /// <i> sceneCallback </i> delegate.
    /// </summary>
    void EndScene() {
        print("ending scene");
        dialogueUI.SetActive(false);
        sceneIsActive = false;
        if (sceneCallback != null) {
            sceneCallback();
        }
    }
}

// *******************************************************************************************
// LoadedDScenes
//*******************************************************************************************
/// <summary>
/// A class purely for storing multiple dialogue scenes, a sequential array of DScenes to
/// compose a complete narrative story.
/// </summary>
[System.Serializable]
public class LoadedDScenes
{
    public DScene[] scenes;
}

// *******************************************************************************************
// DScene
//*******************************************************************************************
/// <summary>
/// A class purely for storing dialogue scene data, such as:
/// <p> The <b> sceneName </b> to be queried to locate this scene for playback. </p>
/// <p> A sequential array of DLine <b> lines </b> to read through to progress the scene. </p>
/// </summary>
[System.Serializable]
public class DScene
{
    public string sceneName;
    public DLine[] lines;
}

// *******************************************************************************************
// DLine
//*******************************************************************************************
/// <summary>
/// A class purely for storing data associated with a single line of dialogue.
/// <p> Contains information on: </p>
/// <p> The <b> name </b> of the speaker. </p>
/// <p> The dialogue <b> text </b> that is being spoken. </p>
/// <p> The name to search for a <b> portrait </b> sprite asset. </p>
/// <p> A <b> trigger </b> flag to denote if delegates should be invoked. </p>
/// <p> The <b> triggerIdx </b> to find the delegate to invoke. </p>
/// <p> The <b> delay </b> duration of time to wait while the delegate is invoked before
/// accepting player input to progress the dialogue. </p>
/// </summary>
[System.Serializable]
public class DLine
{
    public string name;
    public string text;
    public string portrait;
    public bool trigger;
    public int triggerIdx;
    public float delay;
}