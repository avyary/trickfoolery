using UnityEngine;

//*******************************************************************************************
// AnimOnTrigger
//*******************************************************************************************
/// <summary>
/// Plays a designated AnimationClip with a designated Animator upon a collision with
/// the player. 
/// </summary>
public class AnimOnTrigger : MonoBehaviour
{
    public Animator animator;
    public AnimationClip animationClip;

    /// <summary>
    /// Plays the <i> animationClip </i> upon collision with the player GameObject.
    /// </summary>
    /// <param name="collision"> The Collider of the other GameObject that collided with this Collider. </param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.Play(animationClip.name);
        }
    }
}
