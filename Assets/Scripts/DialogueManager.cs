using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

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
    private string jsonName;

    private LoadedDScenes loadedDScenes;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI speechText;
    private Image portraitImg;

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

        TextAsset jsonObj = Resources.Load<TextAsset>(System.String.Format("Dialogue/{0}", jsonName));
        if (jsonObj is null) {
            Debug.Log(System.String.Format("Could not load dialogue scenes from {0}.json", jsonName));
        }
        else {
            loadedDScenes = JsonUtility.FromJson<LoadedDScenes>(jsonObj.text);
        }
        dialogueUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartDialogueScene("testScene");
        }

        if (sceneIsActive && !inDelay && Input.GetButtonDown("Confirm")) {
            StartCoroutine(ProgressScene());
        }
    }

    public void StartDialogueScene(string sceneName, System.Action callback = null)
    {
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
                    yield return new WaitForSecondsRealtime(newLine.delay);
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

    void EndScene() {
        dialogueUI.SetActive(false);
        sceneIsActive = false;
        if (sceneCallback != null) {
            sceneCallback();
        }
    }
}

[System.Serializable]
public class LoadedDScenes
{
    public DScene[] scenes;
}

[System.Serializable]
public class DScene
{
    public string sceneName;
    public DLine[] lines;
}

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