using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveDataManager : MonoBehaviour
{
    public string saveDataPath;
    public ItemDataBase initialStateItemDataBase;
    public bool SavePlayerInventryData(InventryData inventryData){
        Debug.Log("Start to save inventry data");
        string json = JsonUtility.ToJson(inventryData,true);
        Debug.Log("Converted to json: "+json);
        StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + saveDataPath + "/inventry.json");
        streamWriter.WriteLine(json);
        streamWriter.Flush();
        streamWriter.Close();
        return true;
    }
    public void CreateNewSaveData(){
        ItemBase[] magicBooks = {new ItemBase("blank"),new ItemBase("blank"),new ItemBase("blank"),new ItemBase("blank")};
        SavePlayerInventryData(new InventryData(initialStateItemDataBase.items,magicBooks,new ItemBase("blank")));
    }
    public void CreateNewTutrialSaveData(){
        ItemBase[] magicBooks = {initialStateItemDataBase.items[1],new ItemBase("blank"),new ItemBase("blank"),new ItemBase("blank")};
        ItemBase weapon = initialStateItemDataBase.items[0];
        ItemBase[] main = new ItemBase[initialStateItemDataBase.items.Length-2];
        for(int i =0;i < initialStateItemDataBase.items.Length-2;i ++){
            main[i] = initialStateItemDataBase.items[i+2];
        }
        SavePlayerInventryData(new InventryData(main,magicBooks,weapon));
    }
    public InventryData LoadPlayerInventryData(){
        StreamReader streamReader = new StreamReader(Application.persistentDataPath + saveDataPath +"/inventry.json");
        string json = streamReader.ReadToEnd();
        InventryData res = JsonUtility.FromJson<InventryData>(json);
        streamReader.Close();
        return res;
    }
    public bool ExistSaveData(){
        bool res = File.Exists(Application.persistentDataPath + saveDataPath +"/inventry.json");
        return res;
    }
}
public class InventryData{
    public ItemBase[] mains;
    public ItemBase weapon;
    public ItemBase[] magicBooks;
    public InventryData(ItemBase[] mains,ItemBase[] magicBooks,ItemBase weapon){
        this.mains = mains;
        this.weapon = weapon;
        this.magicBooks = magicBooks;
    }
}