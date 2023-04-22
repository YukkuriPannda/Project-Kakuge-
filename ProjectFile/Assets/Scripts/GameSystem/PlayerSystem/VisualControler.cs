using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class VisualControler : MonoBehaviour
{
    public Transform PlayerModel;
    public Animator playerAnimator;
    public PlayerController playerController;
    [SerializeField] private Transform centerBone;
    [SerializeField] private Transform rightHandBone;

    //Normal
    [System.Serializable]
    public class NormalMagicAttributeParticles{
        public ParticleSystem flame;
        [HideInInspector]public ParticleSystem.EmissionModule flameEmission;
        public ParticleSystem aqua;
        public ParticleSystem electro;
        public ParticleSystem terra;
    }
    public NormalMagicAttributeParticles normalAttributeParticles;

    //Slash
    [System.Serializable]
    public class SlashMagicAttributeParticles{
        public ParticleSystem flame;
        [HideInInspector]public ParticleSystem.EmissionModule flameEmission;
        public ParticleSystem aqua;
        public ParticleSystem electro;
        public ParticleSystem terra;
    }
    public SlashMagicAttributeParticles magicAttributeParticles;
    public Animator slashanim;

    //BladeMaterial
    [System.Serializable]
    public class EnchantedBladeEmissionColors{
        [ColorUsage(false,true)]public Color normal;
        [ColorUsage(false,true)]public Color flame;
        [ColorUsage(false,true)]public Color aqua;
        [ColorUsage(false,true)]public Color electro;
        [ColorUsage(false,true)]public Color terra;
    }
    [SerializeField] private MeshRenderer meshRenderer;
    public EnchantedBladeEmissionColors enchantedBladeColors;

    //BladeMaterial
    [System.Serializable]
    public class EffectMaterials{
        public Material flame;
        public Material aqua;
        public Material electro;
        public Material terra;
        public Material none;
    }
    public EffectMaterials effectMaterials;

    //Enchant
    [System.Serializable]
    public class EnchantMagicAttributeParticles{
        public ParticleSystem flame;
        public ParticleSystem aqua;
        public ParticleSystem electro;
        public ParticleSystem terra;
    }
    public EnchantMagicAttributeParticles enchantMagicAttributeParticles;
    public Animator enchantEffectAnim;

    [System.Serializable]
    public class SpecialMagicAnimationClips{
        public AnimationClip flame;
        public AnimationClip aqua;
        public AnimationClip electro;
        public AnimationClip terra;
    }
    public SpecialMagicAnimationClips specialMagicAnimationClips;

    //infos
    [ReadOnly]public string nowPlayerState;
    private string oldPlayerState;
    private EntityBase plEntityBase;//pl=player
    private MagicAttribute oldPlayerMagicAttribute;
    private bool oldLockOperation;
    private float oldHealth;
    private string oldDrawShapeName;

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
    enum SwordAttackType:int{
        Up,
        Trutht,
        Down
    }
    enum Orientation{
        Right,Left
    }
    private void Start() {
        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
        playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
        nowPlayerState = "STAY";
        plEntityBase = playerController.gameObject.GetComponent<EntityBase>();
        normalAttributeParticles.flameEmission = normalAttributeParticles.flame.emission;
        meshRenderer.material.EnableKeyword("_EMISSION");
        SetSpecialMagicAnimClip();
    }
    void Update()
    {
        if(!playerController.lockOperation && !(playerController.InputValueForMove.x == 0 && 
            (nowPlayerState != "RUN" && nowPlayerState != "WALK" && nowPlayerState != "STAY")
                )){
            enchantEffectAnim.SetBool("Gard",false);
            playerController.weapon.transform.position = centerBone.position;
            playerController.weapon.transform.rotation = centerBone.rotation;
            switch(playerController.InputValueForMove.x){
                case float f when (f>=1f)://RunR
                    PlayerModel.localEulerAngles = new Vector3(0,180,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Run);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                    nowPlayerState = "RUN";
                break;
                case float f when(f<=-1)://RunL
                    PlayerModel.localEulerAngles = new Vector3(0,0,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Run);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
                    nowPlayerState = "RUN";
                break;
                case float f when(f>0)://WalkR
                    PlayerModel.localEulerAngles = new Vector3(0,0,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Walk);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                    nowPlayerState = "WALK";
                break;
                case float f when(f<0)://WalkL
                    PlayerModel.localEulerAngles = new Vector3(0,180,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Walk);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
                    nowPlayerState = "WALK";
                break;//Stay
                case float f when(f==0 && nowPlayerState != "STAY"):
                    PlayerModel.localEulerAngles = new Vector3(0,0,0);
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
                    nowPlayerState = "STAY";
                break;
            }
        }else{
            int direction = 0;
            switch(playerController.drawShapeName){
                case "StraightToUp":
                    if(playerController.drawShapePos.x > 0){ 
                        PlayerModel.localEulerAngles = new Vector3(0,0,0);
                        playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                        direction = 1;
                    }else{
                        PlayerModel.localEulerAngles = new Vector3(0,180,0);
                        playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
                        direction = -1;
                    }
                    nowPlayerState = "UpSlash";
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.SwordAttack);

                    playerController.weapon.transform.position = rightHandBone.position;
                    playerController.weapon.transform.rotation = rightHandBone.rotation;

                    if(playerController.lockOperation != oldLockOperation){
                        PlaySlashParticle();
                        playerAnimator.Play("SlashUp",0,0);
                        slashanim.Play("Slash_Up",0,-0.05f);
                        if(direction < 0) slashanim.transform.localEulerAngles = new Vector3(-30,180,0);
                        else slashanim.transform.localEulerAngles = new Vector3(30,0,0);
                        slashanim.transform.position = transform.position + new Vector3(1.3f * direction,0.2f,-2);
                    }
                break;
                case "StraightToRight":
                    PlayerModel.localEulerAngles = new Vector3(0,0,0);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Right);

                    nowPlayerState = "Streight";
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.SwordAttack);

                    playerController.weapon.transform.position = rightHandBone.position;
                    playerController.weapon.transform.rotation = rightHandBone.rotation;
                    if(playerController.lockOperation != oldLockOperation){
                        playerAnimator.Play("Thrust",0,0);
                        slashanim.Play("Slash_Thrust",0,0);
                        slashanim.transform.localEulerAngles = new Vector3(0,0,0);
                        slashanim.transform.position = transform.position + new Vector3(2.7f,0,-3);
                    }
                break;
                case "StraightToLeft":
                    PlayerModel.localEulerAngles = new Vector3(0,180,0);
                    playerAnimator.SetInteger("Orientation",(int)Orientation.Left);

                    nowPlayerState = "Streight";
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.SwordAttack);
                    playerController.weapon.transform.position = rightHandBone.position;
                    playerController.weapon.transform.rotation = rightHandBone.rotation;

                    if(playerController.lockOperation != oldLockOperation){
                        PlaySlashParticle();
                        playerAnimator.Play("Thrust",0,0);
                        slashanim.Play("Slash_Thrust",0,0);
                        slashanim.transform.localEulerAngles = new Vector3(0,180,0);
                        slashanim.transform.position = transform.position + new Vector3(-2.7f,0,-3);
                    }
                break;
                case "StraightToDown":
                    if(playerController.drawShapePos.x > 0){ 
                        PlayerModel.localEulerAngles = new Vector3(0,0,0);
                        playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                        direction = 1;
                    }else{
                        PlayerModel.localEulerAngles = new Vector3(0,180,0);
                        playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
                        direction = -1;
                    }
                    nowPlayerState = "DownSlash";
                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.SwordAttack);
                    playerController.weapon.transform.position = rightHandBone.position;
                    playerController.weapon.transform.rotation = rightHandBone.rotation;

                    //on change playerstate
                    if(playerController.lockOperation != oldLockOperation){
                        PlaySlashParticle();
                        playerAnimator.Play("SlashDown",0,0);
                        slashanim.Play("Slash_Down",0,0);
                        if(direction < 0) slashanim.transform.localEulerAngles = new Vector3(-30,180,0);
                        else slashanim.transform.localEulerAngles = new Vector3(30,0,0);
                        slashanim.transform.position = transform.position + new Vector3(0.9f * direction,0.2f,-2);
                    }
                break;
                case "tap":
                    if(playerController.drawMagicSymbols.Count > 0){
                        nowPlayerState = "ShotMagic";
                        if(playerController.drawShapePos.x > 0){ 
                            playerAnimator.SetInteger("Orientation",(int)Orientation.Right);
                            direction = 1;
                        }else{
                            playerAnimator.SetInteger("Orientation",(int)Orientation.Left);
                            direction = -1;
                        }
                        playerController.weapon.transform.position = centerBone.position;
                        playerController.weapon.transform.rotation = centerBone.rotation;
                        
                        //on change playerstate
                        if(playerController.lockOperation != oldLockOperation){
                            if(Vector2.Distance(playerController.drawShapePos,new Vector2(0,0)) > playerController.enchantDetectionRadius){
                                //ShotMagicBullet
                                if(direction > 0)PlayerModel.localEulerAngles = new Vector3(0,0,0);
                                else PlayerModel.localEulerAngles = new Vector3(0,180,0);
                                if(playerController.drawMagicSymbols[playerController.drawMagicSymbols.Count - 1].magicSymbol != "Circle"){
                                    switch(playerController.drawMagicSymbols[playerController.drawMagicSymbols.Count - 1].magicSymbol){
                                        case "RegularTriangle":{
                                            enchantEffectAnim.gameObject.GetComponent<SpriteRenderer>().material = effectMaterials.flame;
                                        }break;
                                        case "InvertedTriangle":{
                                            enchantEffectAnim.gameObject.GetComponent<SpriteRenderer>().material = effectMaterials.aqua;
                                        }break;
                                        case "Thunder":{
                                            enchantEffectAnim.gameObject.GetComponent<SpriteRenderer>().material = effectMaterials.electro;
                                        }break;
                                        case "Grass":{
                                            enchantEffectAnim.gameObject.GetComponent<SpriteRenderer>().material = effectMaterials.terra;
                                        }break;
                                    }
                                    enchantEffectAnim.transform.position = transform.position + new Vector3(0.6f * direction,0.3f,-2);
                                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.MagicAttack);
                                    playerAnimator.Play("ShotMagicBullet",0,0);
                                    enchantEffectAnim.Play("ShotMagicBullet");
                                    Debug.Log("B");
                                }else{
                                    //SpecialMagic
                                    playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.MagicAttack);
                                    playerAnimator.Play("FlameMagic",0,0);
                                }
                            }
                        }
                    }
                break;
                case "Gard":{
                    if(playerController.lockOperation != oldLockOperation){
                        if(plEntityBase.gard){
                            nowPlayerState = "Gard";
                            playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Gard);
                            playerAnimator.Play("Gard",0,0);
                            if(playerAnimator.GetInteger("Orientation") == (int)Orientation.Right)PlayerModel.localEulerAngles = new Vector3(0,0,0);
                            else PlayerModel.localEulerAngles = new Vector3(0,180,0);
                            enchantEffectAnim.Play("Gard",0,0);
                            enchantEffectAnim.SetBool("Gard",true);
                            enchantEffectAnim.transform.position = new Vector3(transform.position.x,transform.position.y,-3);
                            enchantEffectAnim.transform.localScale = new Vector3(1.5f,1.5f,1);
                        }else{
                            //Enchant
                            if(plEntityBase.myMagicAttribute != MagicAttribute.none){
                                nowPlayerState = "Enchant";
                                switch(playerController.drawMagicSymbols[playerController.drawMagicSymbols.Count - 1].magicSymbol){
                                    case "RegularTriangle":{
                                        enchantEffectAnim.gameObject.GetComponent<SpriteRenderer>().material = effectMaterials.flame;
                                        enchantMagicAttributeParticles.flame.Play();
                                    }break;
                                    case "InvertedTriangle":{
                                        enchantEffectAnim.gameObject.GetComponent<SpriteRenderer>().material = effectMaterials.aqua;
                                        enchantMagicAttributeParticles.aqua.Play();
                                    }break;
                                    case "Thunder":{
                                        enchantEffectAnim.gameObject.GetComponent<SpriteRenderer>().material = effectMaterials.electro;
                                        enchantMagicAttributeParticles.electro.Play();
                                    }break;
                                    case "Grass":{
                                        enchantEffectAnim.gameObject.GetComponent<SpriteRenderer>().material = effectMaterials.terra;
                                        enchantMagicAttributeParticles.terra.Play();
                                    }break;
                                }
                                enchantEffectAnim.transform.position = transform.position + new Vector3(0,0.8f,-2);
                                enchantEffectAnim.transform.localScale = new Vector3(1,1,1);
                                playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Enchant);
                                enchantEffectAnim.Play("Enchant",0,0);
                                if(direction > 0) playerAnimator.Play("Enchant_R");
                                else playerAnimator.Play("Enchant_L");
                            }else enchantEffectAnim.gameObject.GetComponent<SpriteRenderer>().material = effectMaterials.none;
                        }
                    }
                }break;
                case "None":{
                    if(nowPlayerState == "Gard"){
                        nowPlayerState = "Stay";
                        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
                        enchantEffectAnim.SetBool("Gard",false);
                        PlayerModel.localEulerAngles = new Vector3(0,0,0);
                    }
                }break;
            }
        }
        if(oldPlayerMagicAttribute != plEntityBase.myMagicAttribute){
            switch(plEntityBase.myMagicAttribute){
                case MagicAttribute.flame:
                    normalAttributeParticles.flame.Play();
                    meshRenderer.material.SetColor("_EmissionColor",enchantedBladeColors.flame);
                    slashanim.gameObject.GetComponent<SpriteRenderer>().color = new Color(enchantedBladeColors.flame.r,enchantedBladeColors.flame.g,enchantedBladeColors.flame.b,1);
                break;
                case MagicAttribute.aqua:
                    normalAttributeParticles.aqua.Play();
                    meshRenderer.material.SetColor("_EmissionColor",enchantedBladeColors.aqua);
                    slashanim.gameObject.GetComponent<SpriteRenderer>().color = new Color(enchantedBladeColors.aqua.r,enchantedBladeColors.aqua.g,enchantedBladeColors.aqua.b,1);
                break;
                case MagicAttribute.electro:
                    normalAttributeParticles.electro.Play();
                    meshRenderer.material.SetColor("_EmissionColor",enchantedBladeColors.electro);
                    slashanim.gameObject.GetComponent<SpriteRenderer>().color = new Color(enchantedBladeColors.electro.r,enchantedBladeColors.electro.g,enchantedBladeColors.electro.b,1);
                break;
                case MagicAttribute.terra:
                    normalAttributeParticles.terra.Play();
                    meshRenderer.material.SetColor("_EmissionColor",enchantedBladeColors.terra);
                    slashanim.gameObject.GetComponent<SpriteRenderer>().color = new Color(enchantedBladeColors.terra.r,enchantedBladeColors.terra.g,enchantedBladeColors.terra.b,1);
                break;
                case MagicAttribute.none:
                    normalAttributeParticles.flame.Stop();
                    normalAttributeParticles.aqua.Stop();
                    normalAttributeParticles.electro.Stop();
                    normalAttributeParticles.terra.Stop();
                    meshRenderer.material.SetColor("_EmissionColor",enchantedBladeColors.normal);
                    slashanim.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
                break;
            }
        }
        if(oldHealth > plEntityBase.Health){//Damage
            if(plEntityBase.gard){
                
            }else{
                nowPlayerState = "Damage";
                playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Damage);
                if(playerAnimator.GetInteger("Orientation") == 0) playerAnimator.Play("Damage_R",0,0);
                else playerAnimator.Play("Damage_L",0,0);
            }
        }
        oldHealth = plEntityBase.Health;
        oldPlayerState = nowPlayerState;
        oldLockOperation = playerController.lockOperation;
        oldPlayerMagicAttribute = plEntityBase.myMagicAttribute;
        oldDrawShapeName = playerController.drawShapeName;
    }
    public void OnFinishAttackMotion(){
        playerAnimator.SetInteger("AnimNumber",(int)AnimMotions.Stay);
        PlayerModel.localEulerAngles = new Vector3(0,0,0);
        nowPlayerState = "STAY";
    }
    void PlaySlashParticle(){
        switch(plEntityBase.myMagicAttribute){
            case MagicAttribute.flame:
                magicAttributeParticles.flame.Play();
            break;
            case MagicAttribute.aqua:
                magicAttributeParticles.aqua.Play();
            break;
            case MagicAttribute.electro:
                magicAttributeParticles.electro.Play();
            break;
            case MagicAttribute.terra:
                magicAttributeParticles.terra.Play();
            break;
            case MagicAttribute.none:
            break;
        }
    }
    void SetSpecialMagicAnimClip(){
        Debug.Log(playerAnimator.runtimeAnimatorController.animationClips[0]);
        AnimatorController animController = playerAnimator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        animController.layers[0].stateMachine.states[GetStateFromName("FlameMagic",animController.layers[0].stateMachine.states)].state.motion = specialMagicAnimationClips.flame;
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
    
}