using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventrySystem : MonoBehaviour
{
    [SerializeField] RectTransform MainInventryItemSlotsTrf;
    [SerializeField] RectTransform MagicBookSlotsTrf;
    [SerializeField] GameObject ItemPrefab;
    [SerializeField] PlayerController playerController;
    public List<ItemBase> mainInventry = new List<ItemBase>();
    public List<ItemBase> magicBookSlots = new List<ItemBase>();
    public GameObject mainInventrySlotMenuPrefab;
    public GameObject magicBookSlotMenuPrefab;
    void Start()
    {
        SetInventryItem();
        SetMagicBookSlots(); 
    }
    public void LoadItem(){

    }
    public void SetInventryItem(){
        for(int i = 0;i < MainInventryItemSlotsTrf.childCount;i++){
            Destroy(MainInventryItemSlotsTrf.GetChild(i).gameObject);
        }
        
        for(int i = 0;i < mainInventry.Count;i ++){
            if(mainInventry[i].count > 0){
                GameObject inventryItem = Instantiate(ItemPrefab,MainInventryItemSlotsTrf);
                inventryItem.GetComponent<InventryItem>().item = mainInventry[i];
                inventryItem.GetComponent<InventryItem>().item.id = i;
                inventryItem.GetComponent<InventryItem>().menuPrefab = mainInventrySlotMenuPrefab;
            }
        }
    }
    public void SetMagicBookSlots(){
        for(int i = 0;i < MagicBookSlotsTrf.childCount;i++){
            Destroy(MagicBookSlotsTrf.GetChild(i).gameObject);
        }
        for(int i = 0;i < magicBookSlots.Count;i ++){
            if(magicBookSlots[i].count > 0){
                GameObject inventryItem = Instantiate(ItemPrefab,MagicBookSlotsTrf);
                inventryItem.GetComponent<InventryItem>().item = magicBookSlots[i];
                inventryItem.GetComponent<InventryItem>().item.id = i;
                inventryItem.GetComponent<InventryItem>().menuPrefab = magicBookSlotMenuPrefab;
            }
        }
        playerController.magicHolder.flameMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[0].GetUniqueParameter("MagicName");
        playerController.magicHolder.aquaMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[1].GetUniqueParameter("MagicName");
        playerController.magicHolder.electroMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[1].GetUniqueParameter("MagicName");
        playerController.magicHolder.terraMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[1].GetUniqueParameter("MagicName");

    }
}
[System.Serializable]
public enum ItemCategory{
    Weapon,
    Food,
    MagicBookFlame,
    MagicBookAqua,
    MagicBookElectro,
    MagicBookTerra,
    Blank
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
    public float GetUniqueParameter(string name){
        int i = 0;
        for(;i < uniqueParameters.Length && uniqueParameters[i].name != name;i++){}
        return uniqueParameters[i].value;
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
