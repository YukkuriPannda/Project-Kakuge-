using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualControler : MonoBehaviour
{
    public GameObject Player;
    private Animator playerAnimator;
    private PlayerController playerController;
    enum AnimMotions :int {
        Stay,
        Walk,
        Run,
        Jump,
        SwordAttack,
        MagicAttack
    }
    enum Orientation{
        Right,Left
    }
    private void Start() {
        playerAnimator = Player.GetComponent<Animator>();
        playerController = Player.GetComponent<PlayerController>();
        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
        playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
    }
    void Update()
    {
        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
        switch(playerController.rb2D.velocity.x){
            case float f when (f>2f):
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Run);
            playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
            break;
            case float f when(f<-2f):
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Run);
            playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
            break;
            case float f when(f>0):
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Walk);
            playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
            break;
            case float f when(f<0):
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Walk);
            playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
            break;
            case float f when(f==0):
            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
            break;
        }
    }
    void StayMotion() //ランダムにモーション変化を分岐させる
    {
       
    }
}