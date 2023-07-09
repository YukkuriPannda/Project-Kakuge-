using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBaseEX : MonoBehaviour
{
    [HideInInspector]public Vector2 mousePos;
    [HideInInspector]public RectTransform myRectTrf;
    [ReadOnly]public bool pointerStay;
    [ReadOnly]public bool clicking;
    void Start()
    {
        OnStart();
        myRectTrf = gameObject.GetComponent<RectTransform>();
    }
    void Update()
    {
        OnUpdate();
        mousePos = Input.mousePosition;
        if(pointerStay){
            if(myRectTrf.sizeDelta.x < mousePos.x - myRectTrf.position.x
            || -myRectTrf.sizeDelta.x > mousePos.x - myRectTrf.position.x
            || myRectTrf.sizeDelta.y < mousePos.y - myRectTrf.position.y
            || -myRectTrf.sizeDelta.y > mousePos.y - myRectTrf.position.y
            ){
                OnPointerExit();
                pointerStay = false;
            }
            if(Input.GetMouseButtonDown(0)){
                clicking = true;
                OnClickDown();
            }
            if(Input.GetMouseButtonUp(0)){
                clicking = false;     
                OnClickUp();   
            }
        }else {
            if (!(myRectTrf.sizeDelta.x < mousePos.x - myRectTrf.position.x
            || -myRectTrf.sizeDelta.x > mousePos.x - myRectTrf.position.x
            || myRectTrf.sizeDelta.y < mousePos.y - myRectTrf.position.y
            || -myRectTrf.sizeDelta.y > mousePos.y - myRectTrf.position.y
            )){
                OnPointerEnter();
                pointerStay = true;
            }
        }
       
    }
    public virtual void OnPointerEnter(){}
    public virtual  void OnPointerExit(){}
    public virtual  void OnClickDown(){}
    public virtual  void OnClickUp(){}
    public virtual void OnStart(){}
    public virtual void OnUpdate(){}
}
