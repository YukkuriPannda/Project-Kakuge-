using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Item/ItemDataBase", order = 0)]
public class ItemDataBase : ScriptableObject
{
    public ItemBase[] items;
    public ItemPrefabData[] itemPrefabDatas;
    public SpecialMagicMotion[] specialMagicMotions;
    
    public GameObject GetItemPrefab(int id){
        foreach(ItemPrefabData itemPrefabData in itemPrefabDatas){
            if(itemPrefabData.ID == id)return itemPrefabData.Prefab;
        }
        return null;
    }
    public ItemBase GetItemBaseFromID(int id){
        return items[id];
    }
    public AnimationClip GetSpecialMagicMotion(PlayerMagicFactory.MagicKind magicKind){
        return specialMagicMotions[(int)magicKind].animationClip;
    }
}
[System.Serializable]
public class ItemPrefabData{
    public int  ID;
    public GameObject Prefab;
}
[System.Serializable]
public class SpecialMagicMotion{
    public PlayerMagicFactory.MagicKind magicKind;
    public AnimationClip animationClip;
}
