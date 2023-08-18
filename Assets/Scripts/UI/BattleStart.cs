using UnityEngine;
using UnityEngine.UI;

//*******************************************************************************************
// BattleStart
//*******************************************************************************************
/// <summary>
/// Handles the battle start popup at the beginning of a match, with logic to invoke 
/// the beginning of a battle through the GameManager.
/// </summary>
public class BattleStart : MonoBehaviour
{
    /// <summary>
    /// Fires the GameManager startCombatEvent delegate.
    /// </summary>
    public void StartBattle() {
        GameObject.Find("Game Manager").GetComponent<GameManager>().startCombatEvent.Invoke();
    }

    /// <summary>
    /// Enables this GameObject.
    /// </summary>
    public void ShowPanel() {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Sets this GameObject to be invisible.
    /// </summary>
    public void HidePanel() {
        gameObject.GetComponent<Image>().color = Color.clear;
    }
}
