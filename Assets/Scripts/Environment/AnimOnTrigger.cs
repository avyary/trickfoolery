using UnityEngine;

public class AnimOnTrigger : MonoBehaviour
{
  public Animator animator;
    public AnimationClip animationClip;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.Play(animationClip.name);
        }
    }
}
