using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class HypeManager : MonoBehaviour
{
    [SerializeField]
    private float hypeGoal;
    private GameObject _hypeBar;

    private Slider hypeBar;

    private float currentHype = 0;
    public int hypePercent = 0;

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


    [SerializeField]
    private float decaySpeed;

    private List<GameObject> availablePopups;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        hypeBar = GameObject.Find("HypeMeter").GetComponent<Slider>();
        availablePopups = new List<GameObject>(hypePopups);
        hypeBar.maxValue = hypeGoal;
        hypeBar.value = 0;
        UpdateHype(0f);
    }

    public float GetHype()
    {
        return currentHype;
    }
    
    void Update() {
        if (!gameManager.isGameOver) {
            UpdateHype((-1 * Time.deltaTime * decaySpeed) + currentHype);
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            UpdateHype(500f);
        }

        AkSoundEngine.SetRTPCValue("Hype", hypePercent);

        if (hypePercent < 25)
{
   AkSoundEngine.SetState("hype", "hype_0");
} else if (hypePercent >= 25)
{
   AkSoundEngine.SetState("hype", "hype_25");
} else if (hypePercent >= 50)
{
   AkSoundEngine.SetState("hype", "hype_50");
} else if (hypePercent >= 75)
{
   AkSoundEngine.SetState("hype", "hype_75");
} else if (hypePercent == 100)
{
   AkSoundEngine.SetState("hype", "hype_100");
}
    }

    public void IncreaseHype(float hypeDiff)
    {
        if (gameManager.isGameOver) {
            return;
        }
        // if has higher popup level, show higher popup
        if (hypePopupLevel < hypePopups.Length) {
            hypePopupLevel++;
            GameObject chosenPopup = availablePopups[Random.Range(0, availablePopups.Count - 1)];
            availablePopups.Remove(chosenPopup);
            chosenPopup.GetComponent<Animator>().SetTrigger("ShowPopup");
            StartCoroutine(StackHype(chosenPopup));
        }
        UpdateHype(currentHype + hypeDiff);
    }

    public void UpdateHype(float newHypeVal)
    {
        currentHype = Mathf.Min(newHypeVal, hypeGoal);
        currentHype = Mathf.Max(0, newHypeVal);
        hypePercent = (int) Mathf.Floor(100 * (newHypeVal / hypeGoal));
        hypeBar.value = currentHype;

        if (currentHype >= hypeGoal)
        {
            StartCoroutine(gameManager.GameOverWin()); // replace with end of level
        }
    }

    IEnumerator StackHype(GameObject chosenPopup) {
        yield return new WaitForSeconds(hypeStackTime);
        hypePopupLevel--;
        availablePopups.Add(chosenPopup);
    }
    
    public void testttt() {
        print("wuh");
    }
}
