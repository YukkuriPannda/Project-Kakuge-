using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBase : MonoBehaviour
{
    [ReadOnly]private bool touchng;
    public KeyCode key;
    public List<PlayerController> touchingPlc = new List<PlayerController>();
    void OnTriggerEnter2D(Collider2D collision)
    {
        touchingPlc.Add(collision.gameObject.GetComponent<PlayerController>());
        if (collision.gameObject.CompareTag("Player")){
            OnEnterPlayer(touchingPlc[touchingPlc.Count - 1]);
            touchng = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        touchingPlc.Remove(collision.gameObject.GetComponent<PlayerController>());
        if (collision.gameObject.CompareTag("Player")){
            OnExitPlayer(collision.gameObject.GetComponent<PlayerController>());
            touchng = false;
        }
    }
    void Update(){
        if (Input.GetKeyDown(key) && touchng)
        {
            Debug.Log("Intaractive Action!!");
            Action();
            StartCoroutine(Action_IE());
        }
    }
    public virtual void Action(){}
    public virtual IEnumerator Action_IE(){yield break;}
    public virtual void OnEnterPlayer(PlayerController plc){}
    public virtual void OnExitPlayer(PlayerController plc){}
}