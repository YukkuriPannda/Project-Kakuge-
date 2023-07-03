using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventryItem : MonoBehaviour
{
    public ItemBase item;
    private Image icon;
    private bool oldIsPointerOverGameObject =false;
    void Start()
    {
        icon = gameObject.GetComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>(item.spritePath);
        icon.sprite = sprite;
    }
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {

        }
    }
}
