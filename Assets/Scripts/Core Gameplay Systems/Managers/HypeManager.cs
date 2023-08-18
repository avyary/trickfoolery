using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// *******************************************************************************************
// HypeManager
//*******************************************************************************************
/// <summary>
/// Class containing all logic and methods associated with hype (model) such as increasing
/// and decreasing hype, triggering the win condition when maximizing hype, and spawning
/// hype popups.
///
/// <para> Hype can change its value depending on certain events: </para>
/// <p> Hype decreases by 1pt each frame </p>
/// <p> Player dodges enemies: Increases hype (10pt) </p>
/// <p> Player taunts enemies: Increases hype (5pt) </p>
/// <p> Enemy takes damage: Increases hype (10pt) </p>
/// <p> Enemy dies: Increases hype (10pt) </p>
/// </summary>
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

    /// <summary>
    /// Retrieves the current hype.
    /// </summary>
    /// <returns> The current hype value. </returns>
    public float GetHype()
    {
        return currentHype;
    }
    
    void Update() {
        if (!gameManager.isGameOver) {
            UpdateHype((-1 * Time.deltaTime * decaySpeed) + currentHype);
        }
        AkSoundEngine.SetRTPCValue("Hype", hypePercent);
        if (hypePercent < 25) {
           AkSoundEngine.SetState("hype", "hype_0");
        } else if (hypePercent >= 25 && hypePercent < 50) {
           AkSoundEngine.SetState("hype", "hype_25");
        } else if (hypePercent >= 50 && hypePercent < 75) {
           AkSoundEngine.SetState("hype", "hype_50");
        } else if (hypePercent >= 75 && hypePercent < 100) {
           AkSoundEngine.SetState("hype", "hype_75");
        } else if (hypePercent >= 100) {
           AkSoundEngine.SetState("hype", "hype_100");
        }
    }

    /// <summary>
    /// If game state has not reached game over, spawns a random hype popup animation and removes it from
    /// the list of available popups if any are available and increments the "currentHype".
    /// </summary>
    /// <param name="hypeDiff"> The amount to increment the current hype. </param>
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

    /// <summary>
    /// Clamps the current hype between zero and <i> hypeGoal </i>, adjusting the hype bar Slider value to
    /// the new value. Triggers the winning condition through the GameManager if the <i> currentHype </i>
    /// reaches the <i> hypeGoal </i>.
    /// </summary>
    /// <param name="newHypeVal"> The target hype value to set the current hype. </param>
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

    /// <summary>
    /// Delays for <i> hypeStackTime </i> duration, then decrements the <i> hypePopupLevel </i> and
    /// returns the provided popup GameObject to the available hype popups list.
    /// </summary>
    /// <param name="chosenPopup"> The hype popup to be reintroduced to the available hype popups list. </param>
    IEnumerator StackHype(GameObject chosenPopup) {
        yield return new WaitForSeconds(hypeStackTime);
        hypePopupLevel--;
        availablePopups.Add(chosenPopup);
    }
    
    public void testttt() {
        print("wuh");
    }
}
