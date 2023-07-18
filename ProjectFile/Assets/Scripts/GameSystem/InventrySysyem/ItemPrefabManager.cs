using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabManager : MonoBehaviour
{
    [SerializeField]private List<ItemPrefabData> itemPrefabDatas = new List<ItemPrefabData>();
    [System.Serializable]
    private class ItemPrefabData{
        public string  Name;
        public string Path;
    }
    public GameObject GetItemPrefab(string name){
        foreach(ItemPrefabData itemPrefabData in itemPrefabDatas){
            if(itemPrefabData.Name == name)return Resources.Load<GameObject>(itemPrefabData.Path);
        }
        return null;
    }
}
