using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputer : MonoBehaviour
{
    [SerializeField] PlayerController plc;//[P][L]ayer[C]ontller
    float moveNumber;
    Vector2 InputValueForMove;
    void Start()
    {

    }

    void Update()
    {
        InputValueForMove = new Vector2(0,0);
        if (Input.GetKey(KeyCode.W)) {
            InputValueForMove += new Vector2(0,1);
        }
        if (Input.GetKey(KeyCode.S)) {
            InputValueForMove += new Vector2(0,-1);
        }
        if (Input.GetKey(KeyCode.D)) {
            InputValueForMove += new Vector2(1,0);
        }
        if (Input.GetKey(KeyCode.A)) {
            InputValueForMove += new Vector2(-1,0);
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            InputValueForMove *= new Vector2(0.5f,1);
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            if(plc.InventryObj.activeSelf)plc.CloseInventry();
            else plc.OpenInventry();
        }
        plc.InputValueForMove = InputValueForMove;
    }
}