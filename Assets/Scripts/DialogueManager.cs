using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private RawImage portraitImg;

    private bool sceneIsActive = false;
    private int lineIdx = 0;
    private DScene targetScene;

    // Start is called before the first frame update
    void Start()
    {
        nameText = nameTextObj.GetComponent<TextMeshProUGUI>();
        speechText = speechTextObj.GetComponent<TextMeshProUGUI>();
        portraitImg = portraitObj.GetComponent<RawImage>();

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

        if (sceneIsActive && Input.GetKeyDown(KeyCode.Space)) {
            ProgressScene();
        }
    }

    void StartDialogueScene(string sceneName)
    {
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
        ProgressScene(0);
    }

    void ProgressScene(int jump = 1) {
        if (!sceneIsActive) {
            return;
        }
        lineIdx += jump;
        if (lineIdx >= targetScene.lines.Length) {
            EndScene();
            return;
        }

        DLine newLine = targetScene.lines[lineIdx];
        nameText.text = newLine.name;
        speechText.text = newLine.text;
        portraitImg.texture = Resources.Load<Texture2D>("Dialogue/Portraits/" + newLine.portrait);
    }

    void EndScene() {
        dialogueUI.SetActive(false);
        sceneIsActive = false;
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
}