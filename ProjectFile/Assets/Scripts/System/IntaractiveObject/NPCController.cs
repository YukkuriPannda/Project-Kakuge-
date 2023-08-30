using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCController : InteractiveBase {
    public float textInterval;
    public bool openningMessageBox;
    public string[] TalkTexts;
    public GameObject messageBox;
    public GameObject keyBox;
    public GameObject actionName;
    public TextMeshProUGUI bodyTex;
    public Animator animator;
    public override IEnumerator Action_IE(){
        if(!openningMessageBox){
            bodyTex.gameObject.SetActive(true);
            animator.Play("Enable",1,0);
            openningMessageBox = true;
            touchingPlc[touchingPlc.Count - 1].lockOperation = true;
            for(int i = 0;i < TalkTexts.Length;i ++){
                yield return StartCoroutine(Talk(TalkTexts[i]));
            }
            touchingPlc[touchingPlc.Count - 1].lockOperation = false;
            openningMessageBox = false;
            animator.gameObject.SetActive(false);
        }
        yield break;
    }
    public override void OnEnterPlayer(PlayerController plc){
        keyBox.SetActive(true);
        actionName.SetActive(true);
        animator.Play("Activation",0,0);
    }
    public override void OnExitPlayer(PlayerController plc){
        StartCoroutine(Disable());
    }
    IEnumerator Disable(){
        animator.Play("Disable",0,0);
        yield return new WaitForSeconds(0.1f);
        keyBox.SetActive(false);
        actionName.SetActive(false);
    }
    public IEnumerator Talk(string text){
        bodyTex.text = text;
        messageBox.GetComponent<RectTransform>().sizeDelta = new Vector2(bodyTex.preferredWidth+10,bodyTex.preferredHeight+10);
        bodyTex.text = "";
        for(int i = 0;i < text.Length;i ++){
            bodyTex.text += text[i];
            yield return new WaitForSeconds(textInterval);
        }
        while(! Input.anyKeyDown)yield return null;
        yield break;
    }
}