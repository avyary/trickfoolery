using UnityEngine;

//*******************************************************************************************
// TutorialStart
//*******************************************************************************************
/// <summary>
/// Displays and disables the tutorial start UI at the beginning of the tutorial level.
/// </summary>
public class TutorialStart : MonoBehaviour
{
    /// <summary>
    /// Enables this GameObject.
    /// </summary>
    public void ShowPanel() {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables this GameObject.
    /// </summary>
    public void HidePanel() {
        gameObject.SetActive(false);
    }
}
