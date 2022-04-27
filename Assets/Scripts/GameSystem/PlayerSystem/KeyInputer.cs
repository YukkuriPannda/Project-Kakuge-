using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputer : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    float moveNumber;

    void Start()
    {

    }

    void Update()
    {
        moveNumber = 0;
        if (Input.GetKey(KeyCode.Space)) {
            playerController.Jump(1f);
        }
        if (Input.GetKey(KeyCode.D)) {
            moveNumber = 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveNumber = -1;
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            moveNumber *= 0.5f;
        }
        playerController.Move(moveNumber);
    }
}