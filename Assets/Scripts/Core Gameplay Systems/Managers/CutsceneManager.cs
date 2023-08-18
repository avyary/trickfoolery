using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// *******************************************************************************************
// CutsceneManager
//*******************************************************************************************
/// <summary>
/// Handles the parsing and execution of TextAssets associated with the game cutscene
/// system. Contains logic to load the next scene once the cutscene and associated
/// dialogue comes to an end.
/// </summary>
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

    public AK.Wwise.Event playCutsceneSelectSFX;
    public AK.Wwise.Event playCutsceneMUS;

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
        AkSoundEngine.SetState("Cutscenes", "Op");
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

    /// <summary>
    /// Sets the first page of the cutscene dialogue, playing and rendering associated SFX and sprites and
    /// delaying for a duration of time. Upon the delay completion, automatically plays the following page
    /// of dialogue to populate the cutscene with dialogue text. 
    /// </summary>
    IEnumerator StartCutscene() {
        currentPage = cutscene.pages[0];
        playCutsceneMUS.Post(gameObject);
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

    /// <summary>
    /// Loads the next scene if the last page of dialogue had been read. Otherwise, sets the page of dialogue
    /// at <i> pageIdx </i>, firing the CheckForTrigger delegate and rendering associated sprites and dialogue text
    /// animations within a specified text area. Logs the current page index out of the total number of pages.
    /// </summary>
    public void UpdatePage() {
        print(pageIdx + "/" + cutscene.pages.Length);
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
            _captionObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 
                sizeCalc.textBounds.size.x);
            typingText = StartCoroutine(PlayText());
        }
    }

    /// <summary>
    /// Triggers a UI animation to fade out before delaying for a duration of time and loading the
    /// scene specified by <i> nextSceneName </i>.
    /// </summary>
    IEnumerator DelayLoad() {
        fadeInOut.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }
    
    /// <summary>
    /// Disables the dialogue prompt UI and types the current dialogue page text one letter at a time
    /// with <i> timeBetweenLetters </i> delay between each one. Once all the text has been typed, toggles
    /// the <i> pageComplete </i> flag, delays for a duration of time, and enables the dialogue prompt UI.
    /// </summary>
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

    /// <summary>
    /// Sets the next dialogue page and plays associated SFX if the <i> pageComplete </i> flag has been toggled.
    /// Otherwise, auto-fills the dialogue text box. Adjusts the cutscene music according to the current
    /// dialogue <i> pageIdx </i> via the AkSoundEngine.
    /// </summary>
    public void OnConfirm() {
        if (pageComplete) {
            pageIdx++;
            playCutsceneSelectSFX.Post(gameObject);
            UpdatePage();
        }
        else {
            StartCoroutine(RapidFillText());
        }

        if (pageIdx == 3) {
            AkSoundEngine.SetState("Cutscenes", "Op_Ending");
        }

    }

    /// <summary>
    /// Cancels the typing text animations and fills the text box with the complete dialogue for the page.
    /// Toggles the <i> pageComplete </i> flag, delays for a duration of time, and enables the dialogue
    /// prompt UI.
    /// </summary>
    IEnumerator RapidFillText() {
        StopCoroutine(typingText);
        caption.text = currentPage.caption;
        pageComplete = true;
        yield return new WaitForSeconds(1);
        continueArrow.SetActive(true);
    }
    
    // *******************************************************************************************
    // Cutscene
    //*******************************************************************************************
    /// <summary>
    /// A class purely for storing cutscene data, such as the name associated with the parsed
    /// cutscene TextAsset and a sequential array of Pages to compose a complete narrative.
    /// </summary>
    [System.Serializable]
    public class Cutscene
    {
        public string cutsceneName;
        public Page[] pages;
    }

    // *******************************************************************************************
    // Page
    //*******************************************************************************************
    /// <summary>
    /// A class purely for storing data of a sprite asset name to be used to search for
    /// sprite assets and text.
    /// </summary>
    [System.Serializable]
    public class Page
    {
        public string image;
        public string caption;
    }
}
