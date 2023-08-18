using UnityEngine;

//*******************************************************************************************
// LoseMenu
//*******************************************************************************************
/// <summary>
/// Handles the enabling and initialization of the LoseMenu via the PauseMenuManager class.
/// </summary>
public class LoseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuObj;

    /// <summary>
    /// Initializes the pause menu through the PauseMenuManager. Invoked by the LoseMenuSlide animation event.
    /// </summary>
    public void InitMenu() {
        menuObj.GetComponent<PauseMenuManager>().InitMenu();
    }
}
