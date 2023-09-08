using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMagicBookButton : MenuButton
{
    public int id;
    public override void OnClickDown()
    {
        InventrySystem inventrySystem = transform.parent.parent.parent.gameObject.GetComponent<InventrySystem>();
        ItemBase tempItem = inventrySystem.magicBookSlots[(int)((int)inventrySystem.mainInventry[id].category - (int)ItemCategory.MagicBookFlame)];
        inventrySystem.magicBookSlots[(int)((int)inventrySystem.mainInventry[id].category - (int)ItemCategory.MagicBookFlame)] = inventrySystem.mainInventry[id];
        inventrySystem.SetMagicBookSlots();
        if(tempItem.category != ItemCategory.Blank){
            inventrySystem.mainInventry[id] = tempItem;
        }else inventrySystem.mainInventry.RemoveAt(id);
        inventrySystem.SetInventryItem();
        
        base.OnClickDown();
        Debug.Log("On Click Equip Button");
    }
}
