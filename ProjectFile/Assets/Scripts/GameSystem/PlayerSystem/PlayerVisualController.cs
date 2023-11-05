using System.Collections;
using UnityEngine;

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
    public GameObject fade_GameOver;
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
                        plAnim.Play(plc.magicHolder.flameMagic.ToString());
                    }break;
                    case "InvertedTriangle":{
                        plAnim.Play(plc.magicHolder.aquaMagic.ToString());
                    }break;
                    case "Thunder":{
                        plAnim.Play(plc.magicHolder.electroMagic.ToString());
                    }break;
                    case "Grass":{
                        plAnim.Play(plc.magicHolder.terraMagic.ToString());
                    }break;
                }
                StartCoroutine(BlendAnimToStay(plAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length));
                Debug.Log("Special");
            }break;
            case PlayerController.PlayerStates.Hurt:{
                plAnim.Play("Damage",0,0);
            }break;
            case PlayerController.PlayerStates.Deathing:{
                plAnim.Play("Deathing",0,0);
                fade_GameOver.SetActive(true);

            }break;
            case PlayerController.PlayerStates.CounterAttack:{
                if(plc.direction > 0) model.transform.localEulerAngles = new Vector3(0,0,0);
                else model.transform.localEulerAngles = new Vector3(0,180,0);
                plAnim.Play("Counter",0,0);
                PickUpWeapon();
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
    public IEnumerator BlendAnimToStay(float t){
        Debug.Log(t);
        yield return new WaitForSeconds(t+1);
        plAnim.CrossFade("Stay"+(plc.direction == 1?"R":"L"), 0.1f, 0, 0);
        yield break;
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
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(plAnim.runtimeAnimatorController);
        plAnim.runtimeAnimatorController = animatorOverrideController;
        animatorOverrideController["Up"] = normalAttackMotions.upMotion;
        animatorOverrideController["Thrust"] = normalAttackMotions.thrustMotion;
        animatorOverrideController["Down"] = normalAttackMotions.downMotion;
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
}
