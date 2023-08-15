using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuObj;

    public void InitMenu() {
        menuObj.GetComponent<PauseMenuManager>().InitMenu();
    }
}
