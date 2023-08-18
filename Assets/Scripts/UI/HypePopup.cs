using UnityEngine;

//*******************************************************************************************
// HypePopup
//*******************************************************************************************
/// <summary> 
/// Contains a method to adjust the hypePopupLevel from the HypeManager.
/// </summary>
public class HypePopup : MonoBehaviour
{
    [SerializeField]
    private GameObject _hypeManagerObj;

    private HypeManager hypeManager;

    void Start() {
        hypeManager = _hypeManagerObj.GetComponent<HypeManager>();

    }
    
    /// <summary>
    /// Decrements the HypeManager hypePopupLevel used to randomly select a hype popup.
    /// </summary>
    public void DecrementOnCompletion() {
        hypeManager.hypePopupLevel = Mathf.Min(0, --hypeManager.hypePopupLevel);
    }
}
