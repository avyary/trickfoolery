using UnityEngine;

//*******************************************************************************************
// PlayerAnimationController
//*******************************************************************************************
/// <summary>
/// Handles the animations and triggers SFX for the player. Contains various methods
/// for the different states such as Walking (footsteps) and Taunting actions.
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip tomRunning;
    [SerializeField] private AnimationClip tomRolling;
    [SerializeField] private AnimationClip tomTaunt;
    [SerializeField] private AnimationClip tomIdle;

    public AkEvent tauntSFX;

    /// footsteps
    [SerializeField]
    private PlayerSounds playerSounds;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(tomIdle.name);
    }

    /// <summary>
    /// Triggers the tauntSFX if it is not null.
    /// </summary>
    void PlayTauntSound() 
    {   
        if (tauntSFX != null) 
        {
            tauntSFX.HandleEvent(gameObject);
        }
    }

    /// <summary>
    /// Triggers the footstep SFX through PlayerSounds.
    /// </summary>
    private void PlayFootstep()
    {
        playerSounds.PlayFootstepSound();
    }
}
