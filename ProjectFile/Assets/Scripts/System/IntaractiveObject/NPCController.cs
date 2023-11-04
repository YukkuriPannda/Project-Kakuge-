using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEditor.PackageManager.Requests;

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
        //$mkreq{k[A] Push K key,d[Upstreight]} sampleTitle
        if(text[0] == '$'){
            switch(text.Substring(1,6)){
                case "mkreq{":{
                    int text_i = 7;
                    int goalsLengh = 0;
                    List<GuildManager.Request.Goal> goals = new List<GuildManager.Request.Goal>();
                    for(;text[text_i] != '}';text_i++)if(text[text_i] == '[')goalsLengh ++;
                    Debug.Log($"text_i:{text_i}reqLengh:{goalsLengh}");
                    for(int goals_i = 7;goals_i < text_i;){
                        Debug.Log(text.Substring(goals_i,2));
                        if(text.Substring(goals_i,2) == "k["){
                            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode),StringUp("]",2+goals_i,text));
                            goals_i += StringUp("]",2+goals_i,text).Length+1+1;//]+次に送る
                            string goalTitle = StringUp(",",2+goals_i,text);
                            if(goalTitle == "ERR"){
                                goalTitle = StringUp("}",2+goals_i,text);
                                goals_i += goalTitle.Length+1+1;//次に送る
                            }else {
                                goals_i += goalTitle.Length+1+1+1;//,+次に送る
                            }
                            goals.Add(new GuildManager.Request.Goal(goalTitle,keyCode));
                            Debug.Log(goals_i);
                        }else if(text.Substring(goals_i,2) == "d["){
                            Debug.Log("D");
                            string drawShapeName = StringUp("]",2+goals_i,text);
                            goals_i += drawShapeName.Length+1+1;//]+次に送る
                            string goalTitle = StringUp(",",2+goals_i,text);
                            if(goalTitle == "ERR"){
                                goalTitle = StringUp("}",2+goals_i,text); 
                                goals_i += goalTitle.Length+1+1;//,+次に送る
                            }else {
                                goals_i += goalTitle.Length+1+1+1;//,+次に送る
                            }
                            goals.Add(new GuildManager.Request.Goal(goalTitle,drawShapeName));
                        }else {
                            break;
                        }
                    }
                    text_i += 1+1;//スペース分+次におくる
                    string title = text.Substring(text_i);
                    guildManager.MakeRequest(new GuildManager.Request(title,goals.ToArray(),this));
                    Debug.Log(title);
                }break;
            }
            yield break;
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
        int i = startIndex;
        for(;i < text.Length;i ++){
            if(text.Substring(i,to.Length) == to)return text.Substring(startIndex,i - startIndex);
        }
        Debug.LogError($"Not Found {to} from {i}");
        return "ERR";
    }
    public void  ComplateRequest(){
        complateRequest = true;
    }
}