using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventrySystem : MonoBehaviour
{
    [SerializeField] RectTransform MainInventryItemSlotsTrf;
    [SerializeField] RectTransform MagicBookSlotsTrf;
    [SerializeField] RectTransform WeaponSlotTrf;
    [SerializeField] GameObject ItemPrefab;
    [SerializeField] PlayerController plc;
    public List<ItemBase> mainInventry = new List<ItemBase>();
    public List<ItemBase> magicBookSlots = new List<ItemBase>();
    public ItemBase weaponSlot;
    public GameObject magicBookMenuPrefab;
    public GameObject weaponMenuPrefab;
    public GameObject magicBookSlotMenuPrefab;
    public GameObject weaponSlotMenuPrefab;
    void Start()
    {
        SetInventryItem();
        SetMagicBookSlots(); 
        SetWeaponSlot();
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
                switch(mainInventry[i].category){
                    case ItemCategory.Weapon:
                        inventryItem.GetComponent<InventryItem>().menuPrefab = weaponMenuPrefab;
                    break;
                    case ItemCategory.MagicBookAqua :case ItemCategory.MagicBookElectro:case ItemCategory.MagicBookFlame:case ItemCategory.MagicBookTerra:
                        inventryItem.GetComponent<InventryItem>().menuPrefab = magicBookMenuPrefab;
                    break;
                }
                
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
        plc.magicHolder.flameMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[0].GetUniqueParameter("MagicName");
        plc.magicHolder.aquaMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[1].GetUniqueParameter("MagicName");
        plc.magicHolder.electroMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[2].GetUniqueParameter("MagicName");
        plc.magicHolder.terraMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[3].GetUniqueParameter("MagicName");

    }
    public void SetWeaponSlot(){
        if(WeaponSlotTrf.childCount > 0)Destroy(WeaponSlotTrf.GetChild(0).gameObject);
        GameObject inventryItem = Instantiate(ItemPrefab,WeaponSlotTrf);
        inventryItem.GetComponent<InventryItem>().item = weaponSlot;
        inventryItem.GetComponent<InventryItem>().item.id = 0;
        inventryItem.GetComponent<InventryItem>().menuPrefab = weaponSlotMenuPrefab;
        if(weaponSlot.category == ItemCategory.Weapon){
            GameObject weapon = Instantiate(GameObject.Find("GameManager").GetComponent<ItemPrefabManager>().GetItemPrefab((int)weaponSlot.GetUniqueParameter("Modelid")));
            plc.weapon = weapon;
        }else plc.weapon = null;
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
        for(int i = 0;i < uniqueParameters.Length;i++){
            if(uniqueParameters[i].name == name)return uniqueParameters[i].value;
        }
        return 0;
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
