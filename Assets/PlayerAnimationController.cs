using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip moveForwardAnimation;
    [SerializeField] private AnimationClip moveLeftAnimation;
    [SerializeField] private AnimationClip moveBackwardAnimation;
    [SerializeField] private AnimationClip moveRightAnimation;
    [SerializeField] private AnimationClip dashAnimation;
    [SerializeField] private AnimationClip tauntAnimation;
    [SerializeField] private AnimationClip idleAnimation;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(idleAnimation.name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.Play(moveForwardAnimation.name);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            animator.Play(moveLeftAnimation.name);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            animator.Play(moveBackwardAnimation.name);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            animator.Play(moveRightAnimation.name);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play(dashAnimation.name);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            animator.Play(tauntAnimation.name);
        }
        else if (!Input.anyKey)
        {
            animator.Play(idleAnimation.name);
        }
    }
}
