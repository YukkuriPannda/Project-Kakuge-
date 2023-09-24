using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TestEntityBase : MonoBehaviour
{
    public float Health = 65535;
    public bool lockoperation = false;
    public float Heat =10.0f;
    public float HeatCapacity = 10.0f;
    public float OverHeatTime =3.0f;
    public float CoolingSpeed = 5.0f;
    public float DMG = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Health > HeatCapacity)
        {
            Health -= DMG;
            Heat -= CoolingSpeed * Time.deltaTime;
        }
        if(Health == HeatCapacity)
        {
            lockoperation = true;
        }
    }
}
