using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventryItem : MonoBehaviour
{
    public ItemBase item;
    private Image icon;
    void OnEnable()
    {
        icon.sprite = Resources.Load<Sprite>(item.spritePath);
        
    }
}
