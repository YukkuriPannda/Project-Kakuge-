using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unequipe : MenuButton
{
    public int id;
    public override void OnClickDown()
    {
        InventrySystem inventrySystem = transform.parent.parent.parent.gameObject.GetComponent<InventrySystem>();
        ItemBase tempItem = inventrySystem.magicBookSlots[(int)((int)inventrySystem.mainInventry[id].category - (int)ItemCategory.MagicBookFlame)];
        inventrySystem.magicBookSlots[(int)((int)inventrySystem.mainInventry[id].category - (int)ItemCategory.MagicBookFlame)] = inventrySystem.mainInventry[id];
        inventrySystem.SetMagicBookSlots();
        inventrySystem.mainInventry[id] = tempItem;
        inventrySystem.SetInventryItem();
        
        base.OnClickDown();
        Debug.Log("On Click Equip Button");
    }
}
