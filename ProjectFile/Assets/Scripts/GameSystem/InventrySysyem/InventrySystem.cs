using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventrySystem : MonoBehaviour
{
    [SerializeField] RectTransform MainInventryItemSlots;
    [SerializeField] RectTransform MagicBookSlots;
    [SerializeField] GameObject ItemPrefab;
    public List<ItemBase> mainInventry = new List<ItemBase>();
    public List<ItemBase> magicBookSlots = new List<ItemBase>();
    void Start()
    {
        SetInventryItem();
        SetMagicBookSlots(); 
    }
    public void LoadItem(){

    }
    public void SetInventryItem(){
        for(int i = 0;i < MainInventryItemSlots.childCount;i++){
            Destroy(MainInventryItemSlots.GetChild(i).gameObject);
        }
        
        for(int i = 0;i < mainInventry.Count;i ++){
            if(mainInventry[i].count > 0){
                GameObject inventryItem = Instantiate(ItemPrefab,MainInventryItemSlots);
                inventryItem.GetComponent<InventryItem>().item = mainInventry[i];
                inventryItem.GetComponent<InventryItem>().item.id = i;
            }
        }
    }
    public void SetMagicBookSlots(){
        for(int i = 0;i < MagicBookSlots.childCount;i++){
            Destroy(MagicBookSlots.GetChild(i).gameObject);
        }
        for(int i = 0;i < magicBookSlots.Count;i ++){
            if(magicBookSlots[i].count > 0){
                GameObject inventryItem = Instantiate(ItemPrefab,MagicBookSlots);
                inventryItem.GetComponent<InventryItem>().item = magicBookSlots[i];
                inventryItem.GetComponent<InventryItem>().item.id = i;
            }
        }
    }
}
[System.Serializable]
public enum ItemCategory{
    Weapon,
    Food,
    MagicBookFlame,
    MagicBookAqua,
    MagicBookElectro,
    MagicBookTerra
}
[System.Serializable]
public class UniqueParameter{
    public string name;
    public float value;
    public float maxValue = 100;
    public UniqueParameter(string name,float value){
        this.name = name;
        this.value = value;
    }
    public UniqueParameter(string name,float value,float maxValue){
        this.name = name;
        this.value = value;
        this.maxValue = maxValue;
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
    public MagicBook(string name,string spritePath,int count,float exp,PlayerMagicFactory.MagicKind magicName,ItemCategory itemCategory)
        :base(name,spritePath,count,itemCategory){
        uniqueParameters = new UniqueParameter[2]{
            new UniqueParameter("exp",exp),
            new UniqueParameter("MagicName",(int)magicName)
        };
    }
}
