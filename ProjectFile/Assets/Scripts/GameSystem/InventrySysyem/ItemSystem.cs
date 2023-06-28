using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour
{

}
public class ItemBase{
    public int id;
    public string name;
    public string spritePath;
    public int count;
    public string category;
    public ItemBase(int id,string name,string spritePath,int count,string category){
        this.id = id;
        this.name = name;
        this.spritePath = spritePath;
        this.count = count;
        this.category = category;
    }
}
/*public class WeaponItem : ItemBase{
    public WeaponItem(float damage,int level):base(){
        
    }
}*/
