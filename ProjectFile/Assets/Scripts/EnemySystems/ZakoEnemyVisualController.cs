using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoEnemyVisualController : MonoBehaviour
{
    [SerializeField,ReadOnly]private string nowState;
    private ZakoEnemyController zakoEnemyController;
    private Animator animator;
    private string oldState;
    private float oldHealth;  
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
        }
        if(nowState != oldState){//onchange state
            switch(nowState){
                case "attacking":{
                    if(zakoEnemyController.direction == 1) animator.Play("Attack_R",0,0);
                    else animator.Play("Attack_L",0,0);
                }break;
                case "deathing":{
                    if(zakoEnemyController.direction == 1) animator.Play("Death_R",0,0);
                    else animator.Play("Death_L",0,0);
                }break;
            }
        }
        if(zakoEnemyController.entityBase.Health < oldHealth && zakoEnemyController.nowState == "damaging"){
            if(zakoEnemyController.direction == 1) animator.Play("Damage_R",0,0);
            else animator.Play("Damage_L",0,0);
        }
        oldHealth = zakoEnemyController.entityBase.Health;
        oldState = nowState;
    }    enum AnimState{
        staying,
        walking,
        running,
        attacking
    }
}
