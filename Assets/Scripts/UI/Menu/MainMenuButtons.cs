using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public AK.Wwise.Event confirmSFX;
    [SerializeField]
    private string sceneName;
    public AK.Wwise.Event pauseMus;

    void Start()
    {
        pauseMus.Post(gameObject);
    } 

        public void LoadLevel() {
        confirmSFX.Post(gameObject);
        StartCoroutine(DelayLoad());
        pauseMus.Stop(gameObject);
        pauseMus.Stop(gameObject);
    }

    private IEnumerator DelayLoad()
    {
        yield return new WaitForSeconds(2f);
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
