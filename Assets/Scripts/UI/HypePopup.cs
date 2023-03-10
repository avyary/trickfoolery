using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypePopup : MonoBehaviour
{
    [SerializeField]
    private GameObject _hypeManagerObj;

    private HypeManager hypeManager;

    void Start() {
        hypeManager = _hypeManagerObj.GetComponent<HypeManager>();

    }
    public void DecrementOnCompletion() {
        hypeManager.hypePopupLevel = Mathf.Min(0, --hypeManager.hypePopupLevel);
    }
}
