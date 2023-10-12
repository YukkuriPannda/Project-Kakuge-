using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZakoEnemyVisualController : MonoBehaviour
{
    [SerializeField,ReadOnly]private string nowState;
    private ZakoEnemyController zakoEnemyController;
    private Animator animator;
    private Material material;
    public  GameObject HitMark;
    private ZakoEnemyController.State oldState;
    private float oldHealth;  
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        zakoEnemyController = gameObject.GetComponent<ZakoEnemyController>();
        material = animator.gameObject.GetComponent<SpriteRenderer>().material;
    }
    void Update()
    {
        if(zakoEnemyController.direction == 1 && animator.GetInteger("Direction") != 0)animator.SetInteger("Direction",0);
        if(zakoEnemyController.direction == -1 && animator.GetInteger("Direction") != 1)animator.SetInteger("Direction",1);
        switch(zakoEnemyController.nowState){
            case ZakoEnemyController.State.Stopping:{
                animator.SetInteger("AnimNumber",(int)AnimState.staying);
            }break;
            case ZakoEnemyController.State.Finding:{
                animator.SetInteger("AnimNumber",(int)AnimState.walking);
            }break;
            case ZakoEnemyController.State.Following:{
                animator.SetInteger("AnimNumber",(int)AnimState.running);
            }break;
        }
        if(zakoEnemyController.nowState != oldState){//onchange state
            Debug.Log($"{zakoEnemyController.name} changes state from {oldState} to {zakoEnemyController.nowState}");
            switch(zakoEnemyController.nowState){
                case ZakoEnemyController.State.Attacking:{
                    if(zakoEnemyController.direction == 1) animator.Play(zakoEnemyController.DoingSkillKind.ToString()+"_R",0,0);
                    else animator.Play(zakoEnemyController.DoingSkillKind.ToString()+"_L",0,0);
                }break;
                case ZakoEnemyController.State.Deathing:{
                    Debug.Log(zakoEnemyController.nowState + " Death");
                    if(zakoEnemyController.direction == 1) animator.Play("Death_R",0,0);
                    else animator.Play("Death_L",0,0);
                }break;
                case ZakoEnemyController.State.OverHeating:{
                    if(zakoEnemyController.direction == 1) animator.Play("OverHeat_R",0,0);
                    else animator.Play("OverHeat_L",0,0);
                }break;
                case ZakoEnemyController.State.Damaging:{
                    if(zakoEnemyController.direction == 1) animator.Play("Damage_R",0,0);
                    else animator.Play("Damage_L",0,0);
                    Debug.Log(zakoEnemyController.nowState);
                    StartCoroutine(HurtEffect());
                }break;
            }
        }
        oldHealth = zakoEnemyController.entityBase.Health;
        oldState = zakoEnemyController.nowState;
    }    
    enum AnimState{
        staying,
        walking,
        running,
        attacking
    }
    IEnumerator HurtEffect(){
        StartCoroutine(HurtMarkEffect());
        if(material.GetInt("_Hurt") != 1) material.SetInt("_Hurt",1);
        yield return new WaitForSeconds(0.25f);
        material.SetInt("_Hurt",0);
        yield break;
    }
    IEnumerator HurtMarkEffect(){
        HitMark.SetActive(true);
        HitMark.transform.localScale = new Vector3(0.5f,0.5f,1);
        for(float t = 0.25f;t >= 0;t-=Time.deltaTime){
            HitMark.transform.localScale = new Vector3(2f * t,2f * t,1);
            yield return null;
        }
        HitMark.SetActive(false);
        yield break;
    }
}
