using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualControler : MonoBehaviour
{
    public Transform PlayerModel;
    public Animator playerAnimator;
    public PlayerController playerController;
    [SerializeField] private Transform centerBone;
    [SerializeField] private Transform rightHandBone;

    public string nowPlayerState;
    enum AnimMotions :int {
        Stay,
        Walk,
        Run,
        Jump,
        Damage = 10,
        SwordAttack=50,
        MagicAttack=70,
        Enchant,
        Doyaa = 100
    }
    enum SwordAttackType:int{
        Up,
        Trutht,
        Down
    }
    enum Orientation{
        Right,Left
    }
    private void Start() {
        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
        playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
        nowPlayerState = "STAY";
    }
    void Update()
    {
        if(!playerController.lockOperation && !(playerController.InputValueForMove.x == 0 && 
            (nowPlayerState =="UpSlash" || nowPlayerState =="DownSlash" || nowPlayerState =="Streight" || nowPlayerState == "Damage")
                )){
            playerController.weapon.transform.position = centerBone.position;
            playerController.weapon.transform.rotation = centerBone.rotation;
            switch(playerController.InputValueForMove.x){
                case float f when (f>=1f)://RunR
                    PlayerModel.localEulerAngles = new Vector3(0,180,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Run);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                    nowPlayerState = "RUN";
                break;
                case float f when(f<=-1)://RunL
                    PlayerModel.localEulerAngles = new Vector3(0,0,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Run);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
                    nowPlayerState = "RUN";
                break;
                case float f when(f>0)://WalkR
                    PlayerModel.localEulerAngles = new Vector3(0,0,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Walk);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                    nowPlayerState = "WALK";
                break;
                case float f when(f<0)://WalkL
                    PlayerModel.localEulerAngles = new Vector3(0,180,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Walk);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
                    nowPlayerState = "WALK";
                break;//Stay
                case float f when(f==0 && nowPlayerState != "STAY"):
                    PlayerModel.localEulerAngles = new Vector3(0,0,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
                    nowPlayerState = "STAY";
                break;
            }
        }else{
            switch(playerController.drawShapeName){
                case "StraightToUp":
                    if(playerController.drawShapePos.x - transform.position.x > 0){ 
                        PlayerModel.localEulerAngles = new Vector3(0,0,0);
                        playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                    }else{
                        PlayerModel.localEulerAngles = new Vector3(0,180,0);
                        playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
                    }
                    nowPlayerState = "UpSlash";
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.SwordAttack);
                    playerAnimator.SetInteger("SwordNumber",(int)SwordAttackType.Up);
                    playerController.weapon.transform.position = rightHandBone.position;
                    playerController.weapon.transform.rotation = rightHandBone.rotation;
                break;
                case "StraightToRight":
                    PlayerModel.localEulerAngles = new Vector3(0,0,0);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Right);

                    nowPlayerState = "Streight";
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.SwordAttack);
                    playerAnimator.SetInteger("SwordNumber",(int)SwordAttackType.Trutht);
                    playerController.weapon.transform.position = rightHandBone.position;
                    playerController.weapon.transform.rotation = rightHandBone.rotation;
                break;
                case "StraightToLeft":
                    PlayerModel.localEulerAngles = new Vector3(0,180,0);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Left);

                    nowPlayerState = "Streight";
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.SwordAttack);
                    playerAnimator.SetInteger("SwordNumber",(int)SwordAttackType.Trutht);
                    playerController.weapon.transform.position = rightHandBone.position;
                    playerController.weapon.transform.rotation = rightHandBone.rotation;
                break;
                case "StraightToDown":
                    if(playerController.drawShapePos.x - transform.position.x > 0){ 
                        PlayerModel.localEulerAngles = new Vector3(0,0,0);
                        playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                    }else{
                        PlayerModel.localEulerAngles = new Vector3(0,180,0);
                        playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
                    }
                    nowPlayerState = "DownSlash";
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.SwordAttack);
                    playerAnimator.SetInteger("SwordNumber",(int)SwordAttackType.Down);
                    playerController.weapon.transform.position = rightHandBone.position;
                    playerController.weapon.transform.rotation = rightHandBone.rotation;
                break;
                case "tap":
                    if(playerController.drawMagicSymbols.Count > 0){
                        nowPlayerState = "Enchant";
                        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Enchant);
                        playerController.weapon.transform.position = rightHandBone.position;
                        playerController.weapon.transform.rotation = rightHandBone.rotation;
                    }
                break;
                case "None":{
                    if(playerController.lockOperation){
                        nowPlayerState = "Damage";
                        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Damage);
                    }
                }break;
            }
        }
    }
    public void OnFinishAttackMotion(){
        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
        PlayerModel.localEulerAngles = new Vector3(0,0,0);
        nowPlayerState = "STAY";
    }
    void StayMotion() //ランダムにモーション変化を分岐させる
    {
        switch(Random.Range(0,100)){
            case 1:
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Doyaa);
            Debug.Log("DOYAAAA!!!!");
            break;
            default:
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
            break;
        }
       
    }
}