using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
    private MenuManager menuManager;
    
    void Start() {
        menuManager = transform.parent.GetComponent<MenuManager>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        menuManager.OnButtonHover(transform.gameObject);
    }
}
