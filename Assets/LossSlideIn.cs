using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossSlideIn : MonoBehaviour
{
    public void OnSlideIn() {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
