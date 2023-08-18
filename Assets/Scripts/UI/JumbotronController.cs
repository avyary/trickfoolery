using UnityEngine;

public enum JumbotronState {
    Disabled,
    Hidden,
    HypeBar,
    Pause,
    PauseFromHidden,
    Win,
    Loss
}

//*******************************************************************************************
// JumbotronController
//*******************************************************************************************
/// <summary>
/// Tracks the various states of the jumbotron to be accessed from core gameplay
/// classes that change through activity of the jumbotron animation controller.
/// </summary>
public class JumbotronController : MonoBehaviour
{
    public JumbotronState state;
    private GameManager gameManager;

    void Start() {
        state = JumbotronState.Hidden;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    /// <summary>
    /// Sets the jumbotron state to showing the hype bar. Invoked by the SlideIn animation event,
    /// which slides the hype bar part of the jumbotron onto the UI canvas from outside.
    /// </summary>
    void OnSlideIn() {
        state = JumbotronState.HypeBar;
    }

    /// <summary>
    /// Sets the jumbotron state to paused. Invoked by the OpenPause animation event, which slides the
    /// entire jumbotron onto the UI canvas from the previous position only showing the hype bar.
    /// </summary>
    void OnShowPause() {
        state = JumbotronState.Pause;
    }

    /// <summary>
    /// Sets the jumbotron state to paused from the hidden state. Invoked by the OpenFromHidden animation event,
    /// which slides the entire jumbotron onto the UI canvas from outside.
    /// </summary>
    void OnShowPauseFromHidden() {
        state = JumbotronState.PauseFromHidden;
    }

    /// <summary>
    /// Sets the jumbotron state to showing the hype bar and clears the GameManager isPaused flag.
    /// Invoked by the ClosePause animation event, which slides part of the jumbotron off the UI canvas
    /// to show only the hype bar.
    /// </summary>
    void OnHidePause() {
        state = JumbotronState.HypeBar;
        gameManager.isPaused = false;
    }
    
    /// <summary>
    /// Sets the jumbotron state to hidden and clears the GameManager isPaused flag. Invoked by the
    /// CloseToHidden animation event, which slides the entire jumbotron off the UI canvas.
    /// </summary>
    void OnHidePauseToHidden() {
        state = JumbotronState.Hidden;
        gameManager.isPaused = false;
    }
}
