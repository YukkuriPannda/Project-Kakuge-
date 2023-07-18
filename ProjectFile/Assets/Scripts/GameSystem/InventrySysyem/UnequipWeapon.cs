using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnequipWeapon : MenuButton
{
    public int id;
    public override void OnClickDown()
    {
        InventrySystem inventrySystem = transform.parent.parent.parent.gameObject.GetComponent<InventrySystem>();
        ItemBase tempItem = inventrySystem.weaponSlot;
        inventrySystem.weaponSlot = new ItemBase("Blank","None",1,ItemCategory.Blank);
        inventrySystem.SetWeaponSlot();
        inventrySystem.mainInventry.Add(tempItem);
        inventrySystem.SetInventryItem();
        
        base.OnClickDown();
        Debug.Log("On Click Equip Button");
    }
}
