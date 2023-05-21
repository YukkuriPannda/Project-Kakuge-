using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;


public class PlayerVisualController : MonoBehaviour
{
    public PlayerController plc;
    public Animator plAnim;
    private PlayerEffectController plEC;
    public Transform model;
    public Transform rightHand;
    public Transform leftHand;
    public Transform back;
    [System.Serializable]
    class SpecialMagicMotions{
        public AnimationClip flame;
        public AnimationClip aqua;
        public AnimationClip electro;
        public AnimationClip terra;
    }
    [SerializeField]SpecialMagicMotions specialMagicMotions;
    enum AnimMotions :int {
        Stay,
        Walk,
        Run,
        Jump,
        Damage = 10,
        SwordAttack=50,
        MagicAttack=70,
        Enchant,
        Gard,
        Doyaa = 100
    }
    private PlayerController.PlayerStates oldPlcState;
    private int oldDire;
    void Start()
    {
        SetSpecialMagicAnimClip();
        plEC = gameObject.GetComponent<PlayerEffectController>();
    }

    void Update()
    {
        switch(plc.nowPlayerState){
            case PlayerController.PlayerStates.Stay:{
            }break;
            case PlayerController.PlayerStates.Runing:{
            }break;
        }
        if(plc.nowPlayerState != oldPlcState || oldDire != plc.direction){
            PlayAnim();
            plAnim.SetInteger("Direction",plc.direction);
        }
        oldPlcState = plc.nowPlayerState;
        oldDire = plc.direction;
    }
    void SetSpecialMagicAnimClip(){
        Debug.Log(plAnim.runtimeAnimatorController.animationClips[0]);
        AnimatorController animController = plAnim.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        animController.layers[0].stateMachine.states[GetStateFromName("Special_Flame",animController.layers[0].stateMachine.states)].state.motion = specialMagicMotions.flame;
        animController.layers[0].stateMachine.states[GetStateFromName("Special_Aqua",animController.layers[0].stateMachine.states)].state.motion = specialMagicMotions.flame;
        animController.layers[0].stateMachine.states[GetStateFromName("Special_Electro",animController.layers[0].stateMachine.states)].state.motion = specialMagicMotions.flame;
        animController.layers[0].stateMachine.states[GetStateFromName("Special_Terra",animController.layers[0].stateMachine.states)].state.motion = specialMagicMotions.flame;
    }
    void PlayAttackAnim(string playAnimName){
        plAnim.Play(playAnimName,0,0);
        plc.weapon.transform.parent = rightHand.transform;
        plc.weapon.transform.localPosition = new Vector3(0,0,0);
        plc.weapon.transform.localEulerAngles = new Vector3(0,0,0);
        if(plc.direction > 0) model.transform.localEulerAngles = new Vector3(0,0,0);
        else model.transform.localEulerAngles = new Vector3(0,180,0);
        plEC.StartCoroutine(plEC.ActivationAttackParticle(plc.eBase.myMagicAttribute));
    }
    void PlayAnim(){
        switch (plc.nowPlayerState){
            case PlayerController.PlayerStates.Stay:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.Stay);
                if(plAnim.GetCurrentAnimatorStateInfo(0).IsName("StayR") || plAnim.GetCurrentAnimatorStateInfo(0).IsName("StayL") 
                    || oldPlcState == PlayerController.PlayerStates.Runing){
                    Debug.Log("Stay");
                    plc.weapon.transform.parent = back.transform;
                    plc.weapon.transform.localPosition = new Vector3(0,0,0);
                    plc.weapon.transform.localEulerAngles = new Vector3(0,0,0);
                    model.transform.localEulerAngles = new Vector3(0,0,0);
                }
            }break;
            case PlayerController.PlayerStates.Runing:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.Run);
                plAnim.Play("Run",0,0);
                plc.weapon.transform.parent = back.transform;
                plc.weapon.transform.localPosition = new Vector3(0,0,0);
                plc.weapon.transform.localEulerAngles = new Vector3(0,0,0);
                if(plc.direction > 0) model.transform.localEulerAngles = new Vector3(0,0,0);
                else model.transform.localEulerAngles = new Vector3(0,180,0);
            }break;
            case PlayerController.PlayerStates.UpSlash:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.SwordAttack);
                PlayAttackAnim("UpSlash");
            }break;
            case PlayerController.PlayerStates.Thrust:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.SwordAttack);
                PlayAttackAnim("Thrust");
            }break;
            case PlayerController.PlayerStates.DownSlash:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.SwordAttack);
                PlayAttackAnim("DownSlash");
            }break;
            case PlayerController.PlayerStates.ShotMagicBullet:{
                plAnim.Play("ShotMagic",0,0);
                if(plc.direction > 0) model.transform.localEulerAngles = new Vector3(0,0,0);
                else model.transform.localEulerAngles = new Vector3(0,180,0);
            }break;
            case PlayerController.PlayerStates.Garding:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.Gard);
                plAnim.Play(GetDirectionAnimationName("Gard"),0,0);
                plc.weapon.transform.parent = back.transform;
                plc.weapon.transform.localPosition = new Vector3(0,0,0);
                plc.weapon.transform.localEulerAngles = new Vector3(0,0,0);
                model.transform.localEulerAngles = new Vector3(0,0,0);
            }break;
            case PlayerController.PlayerStates.EnchantMySelf:{
                plAnim.Play(GetDirectionAnimationName("Enchant"),0,0);
                plEC.StartCoroutine(plEC.ActivationNormalParticle(plc.eBase.myMagicAttribute,plc.enchantDuraction));
            }break;
            case PlayerController.PlayerStates.ActivateSpecialMagic:{
                if(plc.direction > 0) model.transform.localEulerAngles = new Vector3(0,0,0);
                else model.transform.localEulerAngles = new Vector3(0,180,0);
                switch(plc.drawMagicSymbols[plc.drawMagicSymbols.Count - 2].magicSymbol){
                    case "RegularTriangle":{
                        plAnim.Play("Special_Flame");
                    }break;
                    case "InvertedTriangle":{
                        plAnim.Play("Special_Aqua");
                    }break;
                    case "Thunder":{
                        plAnim.Play("Special_Electro");
                    }break;
                    case "Grass":{
                        plAnim.Play("Special_Terra");
                    }break;

                }
            }break;
            case PlayerController.PlayerStates.Hurt:{
                plAnim.Play("Damage",0,0);
            }break;
        }
    }
    string GetDirectionAnimationName(string name){
        string res = name;
        if(plc.direction > 0)res += "_R";
        else res +="_L";
        return res;
    }
    Color EffectColor(MagicAttribute magicAttribute){
        Color res =Color.white;
        switch(magicAttribute){
            case MagicAttribute.flame:{
                res = MagicColorManager.flame;
            }break;
            case MagicAttribute.aqua:{
                res = MagicColorManager.aqua;
            }break;
            case MagicAttribute.electro:{
                res = MagicColorManager.electro;
            }break;
            case MagicAttribute.terra:{
                res = MagicColorManager.terra;
            }break;
        }
        return res;
    }
    int GetStateFromName(string name,ChildAnimatorState[] states){
        int result = 0;
        for(; states[result].state.name != name;result ++){
            if(result >= states.Length){
                Debug.LogError("Not Found AnimationState "+name);
                return -1;
            }
        }
        return result;
    }
    public void OnSpecialMotionExit(){
        Debug.Log("Exit");
        PlayAnim();
        plAnim.SetInteger("Direction",plc.direction);
    }
}
