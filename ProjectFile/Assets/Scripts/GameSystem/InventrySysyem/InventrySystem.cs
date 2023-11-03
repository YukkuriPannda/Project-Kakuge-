using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventrySystem : MonoBehaviour
{
    [SerializeField] RectTransform MainInventryItemSlotsTrf;
    [SerializeField] RectTransform MagicBookSlotsTrf;
    [SerializeField] RectTransform WeaponSlotTrf;
    [SerializeField] GameObject ItemPrefab;
    [SerializeField] public PlayerController plc;
    [SerializeField] public PlayerEffectController plEC;
    public List<ItemBase> mainInventry;
    public List<ItemBase> magicBookSlots;
    public ItemBase weaponSlot;
    public GameObject magicBookMenuPrefab;
    public GameObject weaponMenuPrefab;
    public GameObject magicBookSlotMenuPrefab;
    public GameObject weaponSlotMenuPrefab;
    public SaveDataManager saveDataManager;
    [Space(10)]
    [Header("WeaponAnimations")]
    public PlayerVisualController.NormalAttackDatas swordAnimMotions;
    public PlayerVisualController.NormalAttackDatas kobushiAnimMotions;
    
    

    [ReadOnly]public GameObject menu;
    void Awake()
    {   
        LoadItem();
        SetInventryItem();
        SetMagicBookSlots(); 
        SetWeaponSlot();
    }
    public void LoadItem(){
        if(!saveDataManager.ExistSaveData())saveDataManager.CreateNewSaveData();
        InventryData inventryData = saveDataManager.LoadPlayerInventryData();
        mainInventry = new List<ItemBase>(inventryData.mains);
        magicBookSlots = new List<ItemBase>(inventryData.magicBooks);
        weaponSlot = inventryData.weapon;
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
                inventryItem.GetComponent<InventryItem>().inventrySystem = this;
                switch(mainInventry[i].category){
                    case ItemCategory.Sword:
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
                inventryItem.GetComponent<InventryItem>().inventrySystem = this;
                inventryItem.GetComponent<InventryItem>().menuPrefab = magicBookSlotMenuPrefab;
            }
        }
        
        plc.magicHolder.flameMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[0].GetUniqueParameter("MagicName");
        plc.magicHolder.aquaMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[1].GetUniqueParameter("MagicName");
        plc.magicHolder.electroMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[2].GetUniqueParameter("MagicName");
        plc.magicHolder.terraMagic = (PlayerMagicFactory.MagicKind)magicBookSlots[3].GetUniqueParameter("MagicName");

        PlayerVisualController plvc = plc.gameObject.GetComponent<PlayerVisualController>();
        plvc.specialAttackMotions.flame   = saveDataManager.initialStateItemDataBase.GetSpecialMagicMotion((PlayerMagicFactory.MagicKind)magicBookSlots[0].GetUniqueParameter("MagicName"));
        plvc.specialAttackMotions.aqua    = saveDataManager.initialStateItemDataBase.GetSpecialMagicMotion((PlayerMagicFactory.MagicKind)magicBookSlots[1].GetUniqueParameter("MagicName"));
        plvc.specialAttackMotions.electro = saveDataManager.initialStateItemDataBase.GetSpecialMagicMotion((PlayerMagicFactory.MagicKind)magicBookSlots[2].GetUniqueParameter("MagicName"));
        plvc.specialAttackMotions.terra   = saveDataManager.initialStateItemDataBase.GetSpecialMagicMotion((PlayerMagicFactory.MagicKind)magicBookSlots[3].GetUniqueParameter("MagicName"));
        plvc.UpdateAnimStateMachines();
    }
    public void SetWeaponSlot(){
        if(WeaponSlotTrf.childCount > 0)Destroy(WeaponSlotTrf.GetChild(0).gameObject);
        GameObject inventryItem = Instantiate(ItemPrefab,WeaponSlotTrf);
        inventryItem.GetComponent<InventryItem>().item = weaponSlot;
        inventryItem.GetComponent<InventryItem>().item.id = 0;
        inventryItem.GetComponent<InventryItem>().inventrySystem = this;
        inventryItem.GetComponent<InventryItem>().menuPrefab = weaponSlotMenuPrefab;

        GameObject weapon = Instantiate(Resources.Load<GameObject>("ItemPrefabs/Kobushi"));
        if(weaponSlot.category == ItemCategory.Sword){
            weapon = Instantiate(saveDataManager.initialStateItemDataBase.GetItemPrefab((int)weaponSlot.GetUniqueParameter("Modelid")));
        }
        Destroy(plc.weapon);
        plc.weapon = weapon;
        PlayerVisualController plvc = plc.gameObject.GetComponent<PlayerVisualController>();

        plvc.weaponEffectSystem = plc.weapon.GetComponent<WeaponEffectSystem>();
        switch(weaponSlot.category){
            case ItemCategory.Sword:{
                plvc.normalAttackMotions = swordAnimMotions; 
            }break;
            case ItemCategory.Blank:{
                plvc.normalAttackMotions = kobushiAnimMotions;
            }break;
        }
        plc.gameObject.GetComponent<PlayerVisualController>().UpdateAnimStateMachines();

        plc.upForwardDistance = plvc.normalAttackMotions.upDistance;
        plc.thrustForwardDistance = plvc.normalAttackMotions.thrustDistance;
        plc.downForwardDistance = plvc.normalAttackMotions.downDistance;

        plc.attackColliders.Up = plvc.normalAttackMotions.upPrefab;
        plc.attackColliders.Thrust = plvc.normalAttackMotions.thrustPrefab;
        plc.attackColliders.Down = plvc.normalAttackMotions.downPrefab;
        plc.attackColliders.counterAttack = plvc.normalAttackMotions.counterPrefab;
    }
}
[System.Serializable]
public enum ItemCategory{
    Sword,
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
    public string name;
    [ReadOnly]public int id =1;
    public string explanation;
    public string spritePath;
    public int count;
    public ItemCategory category;
    public UniqueParameter[] uniqueParameters = null;
    public ItemBase(string name){
        if(name == "blank"){
            this.name = name;
            spritePath = "none";
            count = 1;
            category = ItemCategory.Blank;
        }
    }
    public ItemBase(string name,string spritePath,int count,ItemCategory category){
        this.name = name;
        this.spritePath = spritePath;
        this.count = count;
        this.category = category;
    }
    public ItemBase(string name,string spritePath,int count,ItemCategory category,UniqueParameter[] uniqueParameters){
        this.name = name;
        this.spritePath = spritePath;
        this.count = count;
        this.category = category;
        this.uniqueParameters = uniqueParameters;
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
        :base(name,spritePath,count,ItemCategory.Sword){
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
