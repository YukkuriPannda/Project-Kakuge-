using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class NPCController : InteractiveBase {
    public float textInterval;
    public bool openningMessageBox;
    public string[] TalkTexts;
    public GameObject messageBox;
    public GameObject keyBox;
    public GameObject actionName;
    public TextMeshProUGUI bodyTex;
    public Animator animator;
    public GuildManager guildManager;
    private bool complateRequest= false;
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
        //$mkreq[k[A],d[Upstreight]] sampleTitle
        if(text[0] == '$'){
            switch(text.Substring(1,6)){
                case "mkreq[":{
                    int i = 7;
                    int reqLengh = 0;
                    for(;text.Substring(i,2) == "]]";i++)if(text[i] == '[')reqLengh ++;
                    for(int j = 0;j < reqLengh;j++){
                        if(text.Substring(7+j,2) == "k["){
                            KeyCode result = (KeyCode)Enum.Parse(typeof(KeyCode),StringUp("]",7+j,text));
                        }

                    }
                }break;
            }
        }
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
    public IEnumerator MakeRequest(string title,GuildManager.Request.Goal[] goals){
        guildManager.MakeRequest(new GuildManager.Request(title,goals,this));
        while(complateRequest)yield return null;
        yield break;
    }
    public string StringUp(string to ,int startIndex,string text){
        for(int i = startIndex;i < text.Length;i ++){
            if(text.Substring(i,to.Length) == to)return text.Substring(startIndex,i);
        }
        return "ERR";
    }
    public void  ComplateRequest(){
        complateRequest = true;
    }
}