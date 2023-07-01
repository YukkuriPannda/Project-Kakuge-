using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventrySystem : MonoBehaviour
{
    public ItemDatas itemDatas;
    [SerializeField] public List<WeaponItem> itemBases = new List<WeaponItem>();
    public void LoadItem(){

    }
}
[System.Serializable]
public class ItemDatas{
    public List<WeaponItem> weaponItems = new List<WeaponItem>();
    public List<FoodItem> foodItems = new List<FoodItem>();
    public List<MagicBook> magicBooks = new List<MagicBook>();
}[System.Serializable]
public class UniqueParameter{
    public string name;
    public float value;
}
[System.Serializable]
public class ItemBase{
    public int id;
    public string name;
    public string spritePath;
    public int count;
    public string category;
    public UniqueParameter[] uniqueParameters = null;
    public ItemBase(int id,string name,string spritePath,int count,string category){
        this.id = id;
        this.name = name;
        this.spritePath = spritePath;
        this.count = count;
        this.category = category;
    }
}
[System.Serializable]
public class WeaponItem : ItemBase{
    public float damage;
    public float exp;
    public WeaponItem(int id,string name,string spritePath,int count,string category,float damage,float exp)
        :base(id,name,spritePath,count,"Weapon"){
        this.damage = damage;
        this.exp = exp;
    }
}
[System.Serializable]
public class FoodItem : ItemBase{
    public float healPower;
    public FoodItem(int id,string name,string spritePath,int count,string category,float healPower,float exp)
        :base(id,name,spritePath,count,"Food"){
        this.healPower = healPower;
    }
}
[System.Serializable]
public class  MagicBook: ItemBase{
    public string magicName;
    public float exp;
    public MagicBook(int id,string name,string spritePath,int count,string magicName,float exp)
        :base(id,name,spritePath,count,"MagicBook"){
        this.magicName = magicName;
        this.exp = exp;
    }
}
