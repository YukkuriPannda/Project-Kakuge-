using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateViewer : StateMachineBehaviour
{
    PlayerVisualController playerVisualController;
    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!(stateInfo.IsName("StayR")||stateInfo.IsName("StayL")||stateInfo.IsName("Run")) && stateInfo.normalizedTime >= 1){
            playerVisualController = animator.gameObject.GetComponent<PlayerVisualController>();
            playerVisualController.OnSpecialMotionExit();
        }
    }
}
