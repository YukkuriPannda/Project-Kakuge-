using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GearBitController : MonoBehaviour
{
    public GameObject beam;
    public GameObject target;
    public GameObject returnObj;
    public Animator animator;
    public float distance;
    public float height;
    public float moveSpeed;
    public float rotate;
    float sp;
    public float rotSpeed;
    private Vector2 startTrgPos;
    [HideInInspector]public Vector2 ShotPos;
    public enum AttackMode{
        Follow,
        Volley
    }
    public AttackMode attackMode;
    public enum States{
        StartUp,
        Standby,
        Following,
        TargetLock,
        Shooting,
        RotateShooting,
        OverHeating,
        Returning
    }
    //Mode = Follow :StartUp  -> Standby -> Following -> Shooting
    //       Volley :StartUp  -> Standby -> RotateShooting
    public States nowState;
    public Vector2 moveVec = Vector2.zero;
    void Start()
    {
        startTrgPos = target.transform.position;
        if(attackMode == AttackMode.Follow)ShotPos = startTrgPos + new Vector2(distance * ((startTrgPos.x-transform.position.x > 0)?-1:1),height);
        nowState = States.StartUp;
        Debug.Log(attackMode);
    }

    void Update()
    {
        switch(nowState){
            case States.Following:{
                if(Vector2.Distance(ShotPos,transform.position)< 0.2f){
                    nowState = States.TargetLock;
                    break;
                }

                float myRad = (transform.localEulerAngles.z + 180) * Mathf.Deg2Rad;
                transform.Translate(moveSpeed * Time.deltaTime * Vector2.right);
                transform.Rotate(0, 0, 
                    Mathf.Clamp(Vector2.SignedAngle(new Vector2((float)Mathf.Cos(myRad), (float)Mathf.Sin(myRad)), (transform.position - (Vector3)ShotPos).normalized),-1,1) 
                    * rotSpeed * Time.deltaTime
                );
            }break;
            case States.TargetLock:{
                float myRad = (transform.localEulerAngles.z + 180) * Mathf.Deg2Rad;
                transform.Rotate(0, 0, 
                    Mathf.Clamp(Vector2.SignedAngle(new Vector2((float)Mathf.Cos(myRad), (float)Mathf.Sin(myRad)), (transform.position - target.transform.position).normalized),-1,1) 
                    * rotSpeed * Time.deltaTime
                );
                if(attackMode == AttackMode.Volley){
                    transform.localPosition = new Vector3(0,Mathf.Sin(rotate/180*Mathf.PI)*0.3f,Mathf.Cos(rotate/180*Mathf.PI)*0.3f) + (Vector3)ShotPos;
                    rotate += Time.deltaTime*180;
                }
            }break;
            case States.Returning:{
                if(Vector2.Distance(returnObj.transform.position,transform.position)< 0.5f){
                    Destroy(gameObject);
                    break;
                }

                float myRad = (transform.localEulerAngles.z + 180) * Mathf.Deg2Rad;
                float signedAngle =Vector2.SignedAngle(new Vector2((float)Mathf.Cos(myRad), (float)Mathf.Sin(myRad)), (transform.position - returnObj.transform.position).normalized);
                if(signedAngle < Mathf.PI/3)transform.Translate(moveSpeed * Time.deltaTime * Vector2.right*2);
                transform.Rotate(0, 0, 
                    Mathf.Clamp(signedAngle,-1,1) 
                    * rotSpeed * Time.deltaTime
                );
            }break;
            case States.RotateShooting:{
                transform.localPosition = new Vector3(0,Mathf.Sin(rotate/180*Mathf.PI)*0.3f,Mathf.Cos(rotate/180*Mathf.PI)*0.3f) + (Vector3)ShotPos;
                rotate += Time.deltaTime*180;
            }break;
        }
    }
    public void ShotBeam(){
        nowState = States.Shooting;
        animator.Play("ShotBeam");
        Destroy(Instantiate(beam,transform.position + new Vector3(0.1f,0,0) , transform.rotation,transform),2f);
    }
    public void ShootBeamLong(){
        nowState = States.RotateShooting;
        animator.Play("ShotBeam_Long");
        Destroy(Instantiate(beam,transform.position + new Vector3(0.1f,0,0) , transform.rotation,transform),3f);
    }
}
