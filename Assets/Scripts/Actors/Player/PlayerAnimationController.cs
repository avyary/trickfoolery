using System.Runtime.CompilerServices;
using UnityEngine;

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

    void PlayTauntSound() 
    {   
        if (tauntSFX != null) 
        {
            tauntSFX.HandleEvent(gameObject);
        }
    }

    private void PlayFootstep()
    {
        playerSounds.PlayFootstepSound();
    }
}
