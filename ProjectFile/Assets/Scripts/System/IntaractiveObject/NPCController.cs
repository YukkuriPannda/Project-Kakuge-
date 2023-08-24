using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : InteractiveBase {
    public GameObject messageBox;
    public GameObject keyBox;
    public GameObject actionName;
    public override void Action(){

    }
    public override void OnEnterPlayer(PlayerController plc){
        keyBox.SetActive(true);
        actionName.SetActive(true);
        messageBox.GetComponent<Animator>().Play("Activation",0,0);
    }
    public override void OnExitPlayer(PlayerController plc){
        StartCoroutine(Disable());
    }
    IEnumerator Disable(){
        messageBox.GetComponent<Animator>().Play("Disable",0,0);
        yield return new WaitForSeconds(0.1f);
        keyBox.SetActive(false);
        actionName.SetActive(false);
    }
}