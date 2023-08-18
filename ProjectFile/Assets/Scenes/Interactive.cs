using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [SerializeField] GameObject player;

void OnTriggerStay2D(Collider2D collision)
{
    if (collision.gameObject == player)
    {
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("ドアが開いた！！");
        }
    }
}
}