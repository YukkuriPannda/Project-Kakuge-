using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualControler : MonoBehaviour
{
    public GameObject Player;
    public Transform PlayerModel;
    private Animator playerAnimator;
    private PlayerController playerController;
    public string nowPlayerState;
    enum AnimMotions :int {
        Stay,
        Walk,
        Run,
        Jump,
        SwordAttack,
        MagicAttack,
        Doyaa = 50
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
        switch(playerController.InputValueForMove.x){
            case float f when (f>=1f)://RunR
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Run);
            playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
            nowPlayerState = "RUN";
            break;
            case float f when(f<=-1)://RunL
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Run);
            playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
            nowPlayerState = "RUN";
            break;
            case float f when(f>0):
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Walk);
            playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
            nowPlayerState = "WALK";
            break;
            case float f when(f<0):
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Walk);
            playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
            nowPlayerState = "WALK";
            break;
            case float f when(f==0 && nowPlayerState != "STAY"):
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
            nowPlayerState = "STAY";
            break;
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