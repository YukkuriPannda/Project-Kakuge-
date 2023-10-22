using System.Collections;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerVisualController : MonoBehaviour
{
    public PlayerController plc;
    public Animator plAnim;
    private PlayerEffectController plEC;
    public WeaponEffectSystem weaponEffectSystem;
    public Transform model;
    public Transform rightHand;
    public Transform leftHand;
    public Transform back;
    enum AnimMotions :int {
        Stay,
        Walk,
        Run,
        Jump,
        Damage = 10,
        NormalAttack=50,
        MagicAttack=70,
        Enchant,
        Gard,
        Doyaa = 100
    }
    [SerializeField]private bool OutPutLog = false;

    //olds
    private PlayerController.PlayerStates oldPlcState;
    private int oldDire;
    private bool oldOpeningInventry;
    [System.Serializable]
    public class SpecialAttackMotions{
        public AnimationClip flame;
        public AnimationClip aqua;
        public AnimationClip electro;
        public AnimationClip terra;
    }
    public SpecialAttackMotions specialAttackMotions;
    [System.Serializable]
    public class NormalAttackDatas{
        public AnimationClip upMotion;
        public float upDistance;
        public GameObject upPrefab;
        [Space(5)]
        public AnimationClip thrustMotion;
        public float thrustDistance;
        public GameObject thrustPrefab;
        [Space(5)]
        public AnimationClip downMotion;
        public float downDistance;
        public GameObject downPrefab;
        [Space(5)]
        public AnimationClip CounterMotion;
        public float counterDistance;
        public GameObject counterPrefab;
    }
    public NormalAttackDatas normalAttackMotions;

    void Start()
    {
        UpdateAnimStateMachines();
        plEC = plc.gameObject.GetComponent<PlayerEffectController>();
        if(plc.weapon)weaponEffectSystem = plc.weapon.GetComponent<WeaponEffectSystem>();
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
        if(oldOpeningInventry != plc.openingInventry){
            if(plc.openingInventry){
                plAnim.Play("StayUp");
                PickUpWeapon();
            }
            else {
                plAnim.Play("StayR");
                SheatheWeapon();
            }
        }
        oldPlcState = plc.nowPlayerState;
        oldDire = plc.direction;
        oldOpeningInventry = plc.openingInventry;
    }
    void PlayAnim(){
        switch (plc.nowPlayerState){
            case PlayerController.PlayerStates.Stay:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.Stay);
                if(plAnim.GetCurrentAnimatorStateInfo(0).IsName("StayR") || plAnim.GetCurrentAnimatorStateInfo(0).IsName("StayL") 
                    || oldPlcState == PlayerController.PlayerStates.Runing){
                    if(OutPutLog)Debug.Log("Stay");
                    SheatheWeapon();
                    model.transform.localEulerAngles = new Vector3(0,0,0);
                }
            }break;
            case PlayerController.PlayerStates.Runing:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.Run);
                plAnim.Play("Run",0,0);
                SheatheWeapon();
                if(plc.direction > 0) model.transform.localEulerAngles = new Vector3(0,0,0);
                else model.transform.localEulerAngles = new Vector3(0,180,0);
            }break;
            case PlayerController.PlayerStates.Up:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.NormalAttack);
                PlayAttackAnim("Up");
            }break;
            case PlayerController.PlayerStates.Thrust:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.NormalAttack);
                PlayAttackAnim("Thrust");
            }break;
            case PlayerController.PlayerStates.Down:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.NormalAttack);
                PlayAttackAnim("Down");
            }break;
            case PlayerController.PlayerStates.ShotMagicBullet:{
                plAnim.Play("ShotMagic",0,0);
                if(plc.direction > 0) model.transform.localEulerAngles = new Vector3(0,0,0);
                else model.transform.localEulerAngles = new Vector3(0,180,0);
            }break;
            case PlayerController.PlayerStates.Garding:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.Gard);
                plAnim.Play(GetDirectionAnimationName("Gard"),0,0);
                model.transform.localEulerAngles = new Vector3(0,0,0);
            }break;
            case PlayerController.PlayerStates.EnchantMySelf:{
                plAnim.Play(GetDirectionAnimationName("Enchant"),0,0);
                weaponEffectSystem.EnableNormalParticle(plc.eBase.myMagicAttribute);
                plEC.StartCoroutine(plEC.EnableNormalParticle(plc.eBase.myMagicAttribute));
                PickUpWeapon();
                StartCoroutine(UnEnableEffectTime());
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
                Debug.Log("Special");
            }break;
            case PlayerController.PlayerStates.Hurt:{
                plAnim.Play("Damage",0,0);
            }break;
            case PlayerController.PlayerStates.CounterAttack:{
                plAnim.Play("Counter",0,0);
            }break;
        }
    }
    void PlayAttackAnim(string playAnimName){
        plAnim.Play(playAnimName,0,0);
        PickUpWeapon();
        if(plc.direction > 0) model.transform.localEulerAngles = new Vector3(0,0,0);
        else model.transform.localEulerAngles = new Vector3(0,180,0);
        if(plc.weapon)weaponEffectSystem.PlayAttackParticle(plc.eBase.myMagicAttribute);
    }
    string GetDirectionAnimationName(string name){
        string res = name;
        if(plc.direction > 0)res += "_R";
        else res +="_L";
        return res;
    }
    public void PickUpWeapon(){
        if(plc.weapon){
            WeaponEffectSystem wES = plc.weapon.GetComponent<WeaponEffectSystem>();
            switch(wES.category){
                case ItemCategory.Sword:{
                    plc.weapon.transform.parent = rightHand.transform;
                    plc.weapon.transform.localPosition = new Vector3(0,0,0);
                    plc.weapon.transform.localEulerAngles = new Vector3(0,0,0);
                }break;
                case ItemCategory.Blank:{
                    wES.attackParticles.flames[0].transform.parent.parent.parent = rightHand.transform;
                    wES.attackParticles.flames[0].transform.parent.parent.localPosition = new Vector3(0,0,0);
                    wES.attackParticles.flames[0].transform.parent.parent.localEulerAngles = new Vector3(0,0,0);

                    wES.attackParticles.flames[1].transform.parent.parent.parent = leftHand.transform;
                    wES.attackParticles.flames[1].transform.parent.parent.localPosition = new Vector3(0,0,0);
                    wES.attackParticles.flames[1].transform.parent.parent.localEulerAngles = new Vector3(0,0,0);
                }break;
            }
        }
    }
    public void SheatheWeapon(){
        if(plc.weapon){
            switch(plc.weapon.GetComponent<WeaponEffectSystem>().category){
                case ItemCategory.Sword:{
                    plc.weapon.transform.parent = back.transform;
                    plc.weapon.transform.localPosition = new Vector3(0,0,0);
                    plc.weapon.transform.localEulerAngles = new Vector3(0,0,0);
                }break;
            }
        }
    }
    public void UpdateAnimStateMachines(){
        UnityEditor.Animations.AnimatorController animatorController = plAnim.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        animatorController.layers[0].stateMachine.states[GetStateFromName("Special_Flame",animatorController.layers[0].stateMachine.states)].state.motion = specialAttackMotions.flame;
        animatorController.layers[0].stateMachine.states[GetStateFromName("Special_Aqua",animatorController.layers[0].stateMachine.states)].state.motion = specialAttackMotions.aqua;
        animatorController.layers[0].stateMachine.states[GetStateFromName("Special_Electro",animatorController.layers[0].stateMachine.states)].state.motion = specialAttackMotions.electro;
        animatorController.layers[0].stateMachine.states[GetStateFromName("Special_Terra",animatorController.layers[0].stateMachine.states)].state.motion = specialAttackMotions.terra;
        
        ChildAnimatorState[] NormalStates
         = animatorController.layers[0].stateMachine.stateMachines[GetSubStateFromName("NormalAttack",animatorController.layers[0].stateMachine.stateMachines)].stateMachine.states;
        NormalStates[GetStateFromName("Up",NormalStates)].state.motion = normalAttackMotions.upMotion;
        NormalStates[GetStateFromName("Thrust",NormalStates)].state.motion = normalAttackMotions.thrustMotion;
        NormalStates[GetStateFromName("Down",NormalStates)].state.motion = normalAttackMotions.downMotion;
        NormalStates[GetStateFromName("Counter",NormalStates)].state.motion = normalAttackMotions.CounterMotion;
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
    public void OnSpecialMotionExit(){
        PlayAnim();
        plAnim.SetInteger("Direction",plc.direction);
    }
    IEnumerator UnEnableEffectTime(){
        yield return new WaitForSeconds(plc.enchantDuraction);
        plc.weapon.GetComponent<WeaponEffectSystem>().UnEnableNormalParticle();
        plc.gameObject.GetComponent<PlayerEffectController>().DisableNormalParticle();
    }
    int GetStateFromName(string name,ChildAnimatorState[] states){
        int result = 0;
        for(; states[result].state.name != name;result ++){
            if(result >= states.Length-1){
                Debug.LogError("Not Found AnimationState "+name);
                return -1;
            }
        }
        return result;
    }int GetSubStateFromName(string name,ChildAnimatorStateMachine[] states){
        int result = 0;
        for(; states[result].stateMachine.name != name;result ++){
            if(result >= states.Length-1){
                Debug.LogError("Not Found AnimationStateMachine "+name);
                return -1;
            }
        }
        return result;
    }
}
