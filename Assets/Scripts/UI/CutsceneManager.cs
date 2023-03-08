using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    private string jsonName;
    [SerializeField]
    private GameObject _imageObj;
    [SerializeField]
    private GameObject _captionObj;
    [SerializeField]
    private GameObject continueArrow;
    [SerializeField]
    private string nextSceneName;

    private RawImage image;
    private TextMeshProUGUI caption;

    private int pageIdx;
    private Cutscene cutscene;
    private Page currentPage;

    private bool pageComplete;
    private Coroutine typingText;

    // Start is called before the first frame update
    void Start()
    {
        image = _imageObj.GetComponent<RawImage>();
        caption = _captionObj.GetComponent<TextMeshProUGUI>();
        continueArrow.SetActive(false);

        TextAsset jsonObj = Resources.Load<TextAsset>(System.String.Format("Cutscenes/{0}", jsonName));
        if (jsonObj is null) {
            Debug.Log(System.String.Format("Could not load cutscene from {0}.json", jsonName));
        }
        else {
            cutscene = JsonUtility.FromJson<Cutscene>(jsonObj.text);
        }
        pageIdx = 0;
        UpdatePage();
    }

    void UpdatePage() {
        if (pageIdx == cutscene.pages.Length) {
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
            return;
        }
        currentPage = cutscene.pages[pageIdx];

        image.texture = Resources.Load<Texture2D>("Cutscenes/Images/" + currentPage.image); 
        typingText = StartCoroutine(PlayText());
    }
    
	IEnumerator PlayText()
	{
        pageComplete = false;
        caption.text = "";
        continueArrow.SetActive(false);
		foreach (char c in currentPage.caption)
		{
			caption.text += c;
			yield return new WaitForSeconds(0.1f);
		}
        pageComplete = true;
        yield return new WaitForSeconds(1);
        continueArrow.SetActive(true);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Confirm")) {
            if (pageComplete) {
                pageIdx++;
                UpdatePage();
            }
            else {
                StartCoroutine(RapidFillText());
            }
        }
    }

    IEnumerator RapidFillText() {
        StopCoroutine(typingText);
        caption.text = currentPage.caption;
        pageComplete = true;
        yield return new WaitForSeconds(1);
        continueArrow.SetActive(true);
    }
    
    [System.Serializable]
    public class Cutscene
    {
        public string cutsceneName;
        public Page[] pages;
    }

    [System.Serializable]
    public class Page
    {
        public string image;
        public string caption;
    }
}
