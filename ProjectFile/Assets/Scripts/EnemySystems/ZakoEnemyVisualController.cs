using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoEnemyVisualController : MonoBehaviour
{
    [SerializeField,ReadOnly]private string nowState;
    private ZakoEnemyController zakoEnemyController;
    private Animator animator;
    private string oldState;    
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        zakoEnemyController = gameObject.GetComponent<ZakoEnemyController>();
    }
    void Update()
    {
        nowState = zakoEnemyController.nowState;
        if(zakoEnemyController.direction == 1 && animator.GetInteger("Direction") != 0)animator.SetInteger("Direction",0);
        if(zakoEnemyController.direction == -1 && animator.GetInteger("Direction") != 1)animator.SetInteger("Direction",1);
        switch(nowState){
            case "stopping":{
                animator.SetInteger("AnimNumber",(int)AnimState.staying);
            }break;
            case "finding":{
                animator.SetInteger("AnimNumber",(int)AnimState.walking);
            }break;
            case "following":{
                animator.SetInteger("AnimNumber",(int)AnimState.running);
            }break;
            case "attacking":{
                animator.SetInteger("AnimNumber",(int)AnimState.attacking);
            }break;
        }
        if(nowState != oldState){
            
        }
        oldState = nowState;
    }
    enum AnimState{
        staying,
        walking,
        running,
        attacking
    }
}
