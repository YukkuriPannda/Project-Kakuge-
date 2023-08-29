using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCController : InteractiveBase {
    public float textInterval;
    public string text;
    public GameObject messageBox;
    public GameObject keyBox;
    public GameObject actionName;
    public TextMeshProUGUI bodyTex;
    public override IEnumerator Action_IE(){
        messageBox.GetComponent<Animator>().Play("Enable",1,0);
        bodyTex.text = text;
        messageBox.GetComponent<RectTransform>().sizeDelta = new Vector2(bodyTex.preferredWidth+10,bodyTex.preferredHeight+10);
        bodyTex.text = "";
        for(int i = 0;i < text.Length;i ++){
            bodyTex.text += text[i];
            yield return new WaitForSeconds(textInterval);
        }
        yield break;
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