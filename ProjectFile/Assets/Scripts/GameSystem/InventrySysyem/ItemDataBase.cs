using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Item/ItemDataBase", order = 0)]
public class ItemDataBase : ScriptableObject
{
    public ItemBase[] items;
    public ItemBase GetItemBaseFromID(int id){
        return items[id];
    }
}
