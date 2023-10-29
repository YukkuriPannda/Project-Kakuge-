using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Type43Contoller : MonoBehaviour
{
    public GameObject target;
    public GameObject gearBit;
    public float detecitrRadius= 5.0f;
    public float boostSpeed;
    public enum States{
        Stopping,
        StartUping,
        BeamCombo,
        Deathing,
        Damaging,
        OverHeating
    }
    public States nowState;
    public int direction;
    private Animator animator;
    private Rigidbody2D rb2D;
    void Start()
    {
        nowState = States.Stopping;
        animator = gameObject.GetComponent<Animator>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction = (target.transform.position.x - transform.position.x > 0)?1:-1;
        switch(nowState){
            case States.Stopping :{
                if(Mathf.Abs(target.transform.position.x - transform.position.x) < detecitrRadius){
                    nowState = States.StartUping;
                    animator.Play("StartUp_" + ((direction ==1)?  "R":"L"),0,0);
                }
            }break;
        }
    }
    public void EndAction(){
        switch(nowState){
            case States.StartUping:{
                StartCoroutine(BeamCombo());
            }break;
        }
    }
    IEnumerator BeamCombo(){
        StartCoroutine(Shot4Beams());
        yield break;
    }
    IEnumerator Shot4Beams(){
        List<GameObject> beams = new List<GameObject>();
        for(int i  = 0;i < 4;){
            for(int j = 0;j < 2;j++){
                beams.Add(Instantiate(gearBit,transform.position,Quaternion.identity));
                beams[i].GetComponent<GearBitController>().target = target;
                beams[i].GetComponent<GearBitController>().returnObj = gameObject;
                beams[i].GetComponent<GearBitController>().distance = j + 2;
                beams[i].GetComponent<GearBitController>().height = (i+1)/3+1;
                i++;
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.3f);
            beams[i-2].GetComponent<GearBitController>().nowState = GearBitController.States.Following;
            beams[i-1].GetComponent<GearBitController>().nowState = GearBitController.States.Following;
        }

        bool allBitTargLock = false;
        while(allBitTargLock){
            foreach(GameObject beam in beams){
                if(beam.GetComponent<GearBitController>().nowState == GearBitController.States.TargetLock)allBitTargLock  = true;
                else{
                    allBitTargLock = false;
                    break;
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        beams[0].GetComponent<GearBitController>().ShotBeam();
        beams[1].GetComponent<GearBitController>().ShotBeam();
        yield return new WaitForSeconds(0.5f);
        
        beams[2].GetComponent<GearBitController>().ShotBeam();
        beams[3].GetComponent<GearBitController>().ShotBeam();
        StartCoroutine(IAIGIRI());
        yield return new WaitForSeconds(2f);
        
        foreach(GameObject beam in beams){
            beam.GetComponent<GearBitController>().nowState = GearBitController.States.Returning;
        }
        yield break;
    }
    IEnumerator IAIGIRI(){
        animator.Play("BoostStart_" + ((direction == 1)?  "R":"L"),0,0);
        yield return new WaitForSeconds(0.6f);
        while(target.transform.position.x - transform.position.x > 1f){
            int StartDir = direction;
            if(StartDir == direction)rb2D.AddForce(new Vector2(rb2D.mass * (StartDir * boostSpeed - rb2D.velocity.x)/0.05f , 0));
            else yield break;
            yield return null;
        }
        rb2D.velocity = Vector2.zero;
        animator.Play("BoostSlash_" + ((direction == 1)?  "R":"L"),0,0);
        yield return new WaitForSeconds(0.2f);
        transform.position = target.transform.position + new Vector3(2f*direction,0);
        yield return new WaitForSeconds(2f);
        yield break;
    }
}
