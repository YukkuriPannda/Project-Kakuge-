using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualControler : MonoBehaviour
{
    /*
    0 = stay
    1 = RUN
    2 = 
    */
    public GameObject Player;
    int stayMotionNumber;
    private Animator playerAnimator;
    private PlayerController playerController;
    private void Start() {
        playerAnimator = Player.GetComponent<Animator>();
        playerController = Player.GetComponent<PlayerController>();

        playerAnimator.SetInteger("AnimNumber",0);
    }
    void Update()
    {
            playerAnimator.SetInteger("AnimNumber",0);
        if(playerController.rb2D.velocity.x != 0){
            playerAnimator.SetInteger("AnimNumber",1);
        }
    }
    void StayMotion() //ランダムにモーション変化を分岐させる
    {
        stayMotionNumber = Random.Range(1,10);
        switch(stayMotionNumber){
            case  int n when n <= 5:
            playerAnimator.SetInteger("StayAnimNumber",0);
            break;
        }
    }
}