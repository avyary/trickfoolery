using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip tomRunning;
    [SerializeField] private AnimationClip tomRolling;
    [SerializeField] private AnimationClip tomTaunt;
    [SerializeField] private AnimationClip tomIdle;

    [SerializeField]
    public AkEvent tauntSound;

    public AkEvent playerSounds;

    public AkEvent tauntSFX;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(tomIdle.name);
    }

    // void PlayTauntSound()
    // {
    //     Debug.Log("Tauntsfx");
    // }

    void PlayTauntSound() 
    {   
        Debug.Log("Tauntsfx");
        if (tauntSFX != null) 
        {
            tauntSFX.HandleEvent(gameObject);
        }
    }

    private void Update()
    {
        if (tauntSound != null)
        {
            tauntSound.HandleEvent(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.Play(tomRunning.name);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            animator.Play(tomRunning.name);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            animator.Play(tomRunning.name);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            animator.Play(tomRunning.name);
        }

        else if (Input.GetKeyDown(KeyCode.F))
        {
            animator.Play(tomTaunt.name);
        }
        else if (!Input.anyKey)
        {
            animator.Play(tomIdle.name);
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play(tomRolling.name);
        }
         



    }
}
