using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    public GameObject mainBar;
    public GameObject subBar;
    public EntityBase entityBase;
    void Update()
    {
        if(entityBase.Health >= 0)mainBar.transform.localScale = new Vector3(entityBase.Health/entityBase.MaxHealth*10,0.75f,1);
        if(subBar.transform.localScale.x > 0&&mainBar.transform.localScale.x < subBar.transform.localScale.x)subBar.transform.localScale -= new Vector3(1.5f*Time.deltaTime,0,0);
    }
}
