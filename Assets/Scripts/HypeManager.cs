using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HypeManager : MonoBehaviour
{
    [SerializeField]
    private float hypeGoal;
    [SerializeField]
    private GameObject hypeUI;

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
        UpdateHype(0f);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void IncreaseHype(float hypeDiff)
    {
        // if has higher popup level, show higher popup
        if (hypePopupLevel < hypePopups.Length) {
            hypePopups[hypePopupLevel++].GetComponent<Animator>().SetTrigger("ShowPopup");
            StartCoroutine(StackHype());
        }
        print(hypePopupLevel);
        UpdateHype(currentHype + hypeDiff);
    }

    public void UpdateHype(float newHypeVal)
    {
        currentHype = Mathf.Min(newHypeVal, hypeGoal);
        hypeUI.GetComponent<TextMeshProUGUI>().text = System.String.Format("Hype: {0}/{1}", currentHype.ToString(), hypeGoal.ToString());

        if (currentHype >= hypeGoal)
        {
            gameManager.GameOverWin(); // replace with end of level
        }
    }

    IEnumerator StackHype() {
        yield return new WaitForSeconds(hypeStackTime);
        hypePopupLevel--;
    }
}
