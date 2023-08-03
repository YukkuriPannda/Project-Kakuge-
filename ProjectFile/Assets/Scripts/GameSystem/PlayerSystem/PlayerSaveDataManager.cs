using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerSaveDataManager : MonoBehaviour
{
    public string saveDataPath;
    void Start()
    {
        if(!File.Exists(Application.dataPath + saveDataPath)){
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
