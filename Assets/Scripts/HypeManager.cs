using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HypeManager : MonoBehaviour
{
    [SerializeField]
    private float hypeGoal;
    private GameObject _hypeBar;

    private Slider hypeBar;

    private float currentHype = 0;

    // how much hype is added for each event
    [SerializeField]
    public float DEATH_HYPE;
    [SerializeField]
    public float HIT_HYPE;
    [SerializeField]
    public float DODGE_HYPE;

    private GameManager gameManager;

    [SerializeField]
    public GameObject[] hypePopups;

    private bool isShowingHype = false;
    public int hypePopupLevel = 0;

    [SerializeField]
    private float hypeStackTime;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        hypeBar = GameObject.Find("HypeMeter").GetComponent<Slider>();
        hypeBar.maxValue = hypeGoal;
        hypeBar.value = 0;
        UpdateHype(0f);
    }

    public void IncreaseHype(float hypeDiff)
    {
        // if has higher popup level, show higher popup
        if (hypePopupLevel < hypePopups.Length) {
            hypePopups[hypePopupLevel++].GetComponent<Animator>().SetTrigger("ShowPopup");
            StartCoroutine(StackHype());
        }
        UpdateHype(currentHype + hypeDiff);
    }

    public void UpdateHype(float newHypeVal)
    {
        currentHype = Mathf.Min(newHypeVal, hypeGoal);
        hypeBar.value = currentHype;

        if (currentHype >= hypeGoal)
        {
            StartCoroutine(gameManager.GameOverWin()); // replace with end of level
        }
    }

    IEnumerator StackHype() {
        yield return new WaitForSeconds(hypeStackTime);
        hypePopupLevel--;
    }
    
    public void testttt() {
        print("wuh");
    }
}
