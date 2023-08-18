using UnityEngine;

//*******************************************************************************************
// LossSlideIn
//*******************************************************************************************
/// <summary>
/// Enables the lose UI sprite image as the level failure animations play.
/// </summary>
public class LossSlideIn : MonoBehaviour
{
    /// <summary>
    /// Enables the first child component of this GameObject. Invoked by the SlideInLoss animation event.
    /// </summary>
    public void OnSlideIn() {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
