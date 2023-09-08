using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabManager : MonoBehaviour
{
    [SerializeField]private List<ItemPrefabData> itemPrefabDatas = new List<ItemPrefabData>();
    [System.Serializable]
    private class ItemPrefabData{
        public int  ID;
        public string Path;
    }
    public GameObject GetItemPrefab(int id){
        foreach(ItemPrefabData itemPrefabData in itemPrefabDatas){
            if(itemPrefabData.ID == id)return Resources.Load<GameObject>(itemPrefabData.Path);
        }
        return null;
    }
}
