using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntityBase2 : MonoBehaviour
{
    public float Health = 65535;
    public bool lockoperation = false;
    public float Heat = 0.0f; 
    public float HeatCapacity = 10.0f;
    public float OverHeatTime = 3.0f;
    public float CoolingSpeed = 5.0f;
    public float DMG = 1.0f;

    void Update()
    {
        if (!lockoperation)
        {
            Heat += DMG; 
            Heat = Mathf.Clamp(Heat, 0.0f, HeatCapacity); 

            if (Heat >= HeatCapacity)
            {
                lockoperation = true;
            }
        }

        Heat -= CoolingSpeed * Time.deltaTime;
        Heat = Mathf.Max(Heat, 0.0f); 

        if (lockoperation)
        {
            OverHeatTime -= Time.deltaTime;
            if (OverHeatTime <= 0.0f)
            {
                lockoperation = false; 
                Heat = 0.0f;
            }
        }
    }
}
