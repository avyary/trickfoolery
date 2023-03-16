using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public AK.Wwise.Event confirmSFX;
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private float loadDelay;

    private MenuManager menuManager;

    void Start()
    {
        menuManager = GameObject.Find("Buttons").GetComponent<MenuManager>();
    }

    public void LoadLevel() {
        menuManager.buttonSelected = true;
        confirmSFX.Post(gameObject);
        StartCoroutine(DelayLoad());
    }

    private IEnumerator DelayLoad()
    {
        yield return new WaitForSeconds(Mathf.Max(0, loadDelay - 1.5f));
        GameObject.Find("FadeInOut").GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.5f);
        //AkSoundEngine.StopAll();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
