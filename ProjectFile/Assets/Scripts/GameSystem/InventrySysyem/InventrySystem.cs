using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventrySystem : MonoBehaviour
{
    [SerializeField] RectTransform ItemSlots;
    [SerializeField] GameObject ItemPrefab;
    public List<WeaponItem> itemBases = new List<WeaponItem>();
    void Start()
    {
        SetInventryItem();
    }
    public void LoadItem(){

    }
    public void SetInventryItem(){
        foreach(ItemBase itemBase in itemBases){
            if(itemBase.count > 0){
                Debug.Log(ItemPrefab.name);
                Debug.Log(ItemSlots.name);
                GameObject inventryItem = Instantiate(ItemPrefab,ItemSlots);
                inventryItem.GetComponent<InventryItem>().item = itemBase;
            }
        }
    }
}
[System.Serializable]
public class ItemDatas{
    public List<WeaponItem> weaponItems = new List<WeaponItem>();
    public List<FoodItem> foodItems = new List<FoodItem>();
    public List<MagicBook> magicBooks = new List<MagicBook>();
}
public enum ItemCategory{
    Weapon,
    Food,
    MagicBook
}
[System.Serializable]
public class UniqueParameter{
    public string name;
    public float value;
    public UniqueParameter(string name,float value){
        this.name = name;
        this.value = value;
    }
}
[System.Serializable]
public class ItemBase{
    [ReadOnly]public int id;
    public string name;
    public string explanation;
    public string spritePath;
    public int count;
    public ItemCategory category;
    public UniqueParameter[] uniqueParameters = null;
    public ItemBase(string name,string spritePath,int count,ItemCategory category){
        this.name = name;
        this.spritePath = spritePath;
        this.count = count;
        this.category = category;
    }
    
}
[System.Serializable]
public class WeaponItem : ItemBase{
    public WeaponItem(string name,string spritePath,int count,string category,float damage,float exp)
        :base(name,spritePath,count,ItemCategory.Weapon){
        uniqueParameters = new UniqueParameter[2]{
            new UniqueParameter("damage",damage),
            new UniqueParameter("exp",exp)
        };
    }
}
[System.Serializable]
public class FoodItem : ItemBase{
    public FoodItem(string name,string spritePath,int count,string category,float healPower)
        :base(name,spritePath,count,ItemCategory.Food){
        uniqueParameters = new UniqueParameter[1]{
            new UniqueParameter("healPower",healPower)
        };
    }
}
[System.Serializable]
public class  MagicBook: ItemBase{
    public float exp;
    public MagicBook(string name,string spritePath,int count,float exp)
        :base(name,spritePath,count,ItemCategory.MagicBook){
        uniqueParameters = new UniqueParameter[1]{
            new UniqueParameter("exp",exp)
        };
    }
}
