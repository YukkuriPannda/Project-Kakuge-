using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoEnemyVisualController : MonoBehaviour
{
    [SerializeField,ReadOnly]private string nowState;
    private ZakoEnemyController zakoEnemyController;
    private Animator animator;
    private Material material;
    public  GameObject HitMark;
    private string oldState;
    private float oldHealth;  
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        zakoEnemyController = gameObject.GetComponent<ZakoEnemyController>();
        material = animator.gameObject.GetComponent<SpriteRenderer>().material;
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
            }
        }
        if(zakoEnemyController.entityBase.Health < oldHealth){
            if(zakoEnemyController.nowState != "deathing"){
                if(zakoEnemyController.direction == 1) animator.Play("Damage_R",0,0);
                else animator.Play("Damage_L",0,0);
                StartCoroutine(HurtEffect());
            }else{
                if(zakoEnemyController.direction == 1) animator.Play("Death_R",0,0);
                else animator.Play("Death_L",0,0);
            }
        }
        oldHealth = zakoEnemyController.entityBase.Health;
        oldState = nowState;
    }    enum AnimState{
        staying,
        walking,
        running,
        attacking
    }
    IEnumerator HurtEffect(){
        StartCoroutine(HurtMarkEffect());
        material.SetInteger("_Hurt",1);
        yield return new WaitForSeconds(0.25f);
        material.SetInteger("_Hurt",0);
        yield break;
    }
    IEnumerator HurtMarkEffect(){
        Debug.Log("H");
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
