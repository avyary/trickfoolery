using System.Collections;
using System.Collections.Generic;
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

public class JumbotronController : MonoBehaviour
{
    public JumbotronState state;
    private GameManager gameManager;

    void Start() {
        state = JumbotronState.Hidden;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void OnSlideIn() {
        state = JumbotronState.HypeBar;
    }

    void OnShowPause() {
        state = JumbotronState.Pause;
    }

    void OnShowPauseFromHidden() {
        state = JumbotronState.PauseFromHidden;
    }

    void OnHidePause() {
        state = JumbotronState.HypeBar;
        gameManager.isPaused = false;
    }
    
    void OnHidePauseToHidden() {
        state = JumbotronState.Hidden;
        gameManager.isPaused = false;
    }
}
