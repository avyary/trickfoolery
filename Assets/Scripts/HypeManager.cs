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

    private float currentHype;

    // how much hype is added for each event
    [SerializeField]
    public float DEATH_HYPE;
    [SerializeField]
    public float HIT_HYPE;
    [SerializeField]
    public float DODGE_HYPE;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHype(0f);
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void ChangeHype(float hypeDiff)
    {
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
}
