using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//*******************************************************************************************
// MainMenuButtons
//*******************************************************************************************
/// <summary>
/// Contains various methods for OnPress events, such as loading another level and
/// quitting the game.
/// </summary>
public class MainMenuButtons : MonoBehaviour
{
    public AK.Wwise.Event confirmSFX;
    public AK.Wwise.Event stopTrickfooleryMUS;
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private float loadDelay;

    private MenuManager menuManager;

    void Start()
    {
        menuManager = GameObject.Find("Buttons").GetComponent<MenuManager>();
    }

    /// <summary>
    /// Freezes the UI menu button selection and plays associated SFX before triggering a UI animation
    /// to fade the scene out.
    /// </summary>
    public void LoadLevel() {
        menuManager.buttonSelected = true;
        stopTrickfooleryMUS.Post(gameObject);
        confirmSFX.Post(gameObject);
        StartCoroutine(DelayLoad());
    }

    /// <summary>
    /// Delays for a duration of time calculated from <i> loadDelay </i> before triggering a UI animation to fade
    /// the scene out and reset the ProgressTracker and load the scene specified by <i> sceneName </i>.
    /// </summary>
    private IEnumerator DelayLoad()
    {
        yield return new WaitForSeconds(Mathf.Max(0, loadDelay - 1.5f));
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.5f);
        //AkSoundEngine.StopAll();
        GameObject.Find("ProgressTracker").GetComponent<ProgressTracker>().isRestart = false;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Quits the game by closing the application for a build or finishing the playing process in the Editor.
    /// </summary>
    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
