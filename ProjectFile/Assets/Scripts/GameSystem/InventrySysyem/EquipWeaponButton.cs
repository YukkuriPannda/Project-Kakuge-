using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeaponButton : MenuButton
{
    public int id;
    public override void OnClickDown()
    {
        InventrySystem inventrySystem = transform.parent.parent.parent.gameObject.GetComponent<InventrySystem>();
        ItemBase tempItem = inventrySystem.weaponSlot;
        inventrySystem.weaponSlot = inventrySystem.mainInventry[id];
        inventrySystem.SetWeaponSlot();
        
        if(tempItem.category != ItemCategory.Blank){
            inventrySystem.mainInventry[id] = tempItem;
        }else inventrySystem.mainInventry.RemoveAt(id);
        inventrySystem.SetInventryItem();
        
        base.OnClickDown();
        Debug.Log("On Click Equip Button");
    }
}
