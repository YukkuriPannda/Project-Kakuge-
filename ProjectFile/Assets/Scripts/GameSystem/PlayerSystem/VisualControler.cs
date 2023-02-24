using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualControler : MonoBehaviour
{
    public GameObject Player;
    public Transform PlayerModel;
    public Animator playerAnimator;
    private PlayerController playerController;
    [SerializeField] private Transform centerBone;
    [SerializeField] private Transform rightHandBone;

    public string nowPlayerState;
    enum AnimMotions :int {
        Stay,
        Walk,
        Run,
        Jump,
        SwordAttack=50,
        MagicAttack=70,
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
        playerAnimator = Player.GetComponent<Animator>();
        playerController = Player.GetComponent<PlayerController>();
        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
        playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
        nowPlayerState = "STAY";
    }
    void Update()
    {
        if(!playerController.lockMove && !(playerController.InputValueForMove.x == 0 && nowPlayerState =="DownSlash")){
            playerController.weapon.transform.position = centerBone.position;
            playerController.weapon.transform.rotation = centerBone.rotation;
            switch(playerController.InputValueForMove.x){
                case float f when (f>=1f)://RunR
                PlayerModel.localEulerAngles = new Vector3(0,0,0);
                playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Run);
                playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                nowPlayerState = "RUN";
                break;
                case float f when(f<=-1)://RunL
                PlayerModel.localEulerAngles = new Vector3(0,180,0);
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
            }
        }
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