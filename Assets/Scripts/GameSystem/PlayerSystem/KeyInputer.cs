using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputer : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            playerController.Jump(1f);
        }
    }
}
