using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [ReadOnly]private bool touchng;
    public KeyCode key;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))touchng = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))touchng = false;
    }
    void Update(){
        if (Input.GetKeyDown(key) && touchng)
        {
            Debug.Log("Action!!");
        }
    }
}