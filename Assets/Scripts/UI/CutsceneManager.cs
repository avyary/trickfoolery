using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
    private GameObject _sizeCalcObj;
    [SerializeField]
    private GameObject continueArrow;
    [SerializeField]
    private string nextSceneName;

    private Image image;
    private TextMeshProUGUI caption;
    private TextMeshProUGUI sizeCalc;

    public int pageIdx;
    private Cutscene cutscene;
    private Page currentPage;

    private bool pageComplete;
    private Coroutine typingText;

    private GameObject fadeInOut;

    [SerializeField]
    private float timeBetweenLetters;
    [SerializeField]
    private UnityEvent CheckForTrigger;

    // Start is called before the first frame update
    void Start()
    {
        image = _imageObj.GetComponent<Image>();
        caption = _captionObj.GetComponent<TextMeshProUGUI>();
        sizeCalc = _sizeCalcObj.GetComponent<TextMeshProUGUI>();
        fadeInOut = GameObject.Find("FadeInOut");
        continueArrow.SetActive(false);

        TextAsset jsonObj = Resources.Load<TextAsset>(System.String.Format("Cutscenes/{0}", jsonName));
        if (jsonObj is null) {
            Debug.Log(System.String.Format("Could not load cutscene from {0}.json", jsonName));
        }
        else {
            cutscene = JsonUtility.FromJson<Cutscene>(jsonObj.text);
        }
        StartCoroutine(StartCutscene());
    }

    IEnumerator StartCutscene() {
        currentPage = cutscene.pages[0];
        if (currentPage.image is null) {
            image.sprite = Resources.Load<Sprite>("Cutscenes/Images/black"); 
        }
        else {
            image.sprite = Resources.Load<Sprite>("Cutscenes/Images/" + currentPage.image); 
        }
        yield return new WaitForSeconds(1f);
        pageIdx = 0;
        UpdatePage();
    }

    public void UpdatePage() {
        if (pageIdx == cutscene.pages.Length) {
            StartCoroutine(DelayLoad());
        }
        else {
            currentPage = cutscene.pages[pageIdx];
            CheckForTrigger.Invoke();
            if (currentPage.image is null) {
                image.sprite = Resources.Load<Sprite>("Cutscenes/Images/black"); 
            }
            else {
                image.sprite = Resources.Load<Sprite>("Cutscenes/Images/" + currentPage.image); 
            }
            sizeCalc.text = currentPage.caption;
            Canvas.ForceUpdateCanvases();
            _captionObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeCalc.textBounds.size.x);
            typingText = StartCoroutine(PlayText());
        }
    }

    IEnumerator DelayLoad() {
        fadeInOut.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }
    
	IEnumerator PlayText()
	{
        pageComplete = false;
        caption.text = "";
        continueArrow.SetActive(false);
		foreach (char c in currentPage.caption)
		{
			caption.text += c;
			yield return new WaitForSeconds(timeBetweenLetters);
		}
        pageComplete = true;
        yield return new WaitForSeconds(0.5f);
        continueArrow.SetActive(true);
	}

    public void OnConfirm() {
        if (pageComplete) {
            pageIdx++;
            UpdatePage();
        }
        else {
            StartCoroutine(RapidFillText());
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
