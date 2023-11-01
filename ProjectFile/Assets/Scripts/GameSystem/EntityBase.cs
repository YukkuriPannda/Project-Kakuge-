using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityBase : MonoBehaviour
{
    //HP管理や被攻撃処理をする。HPを持つオブジェクト全部にアタッチ
    public float MaxHealth = 65535; //最大HP
    public float Health = 65535; //HP
    public MagicAttribute myMagicAttribute = MagicAttribute.none; //Magic変数
    public float Heat = 0.0f; //Heat
    public float HeatCapacity = 50.0f; //Heat容量
    public float OverHeatTime = 10.0f; //OverHeatしてる時間
    public float CoolingSpeed = 5.0f; //冷えろ
    public float Heating = 5.0f;
    public float parryCooltime;

    public bool gard = false; //Gard
    [SerializeField,ReadOnly] public bool ParryReception = false;
    [SerializeField,ReadOnly] public bool acceptDamage = true;
    [SerializeField,ReadOnly] public float timeLastHurt = 0;
    public bool Invulnerable = false;
    public bool NoGravity = false;
    public bool OnGround = false;
    [ReadOnly,SerializeField]public  bool overHeating;
    private WaitForSecondsRealtime chachedHitStop;
    [ReadOnly]public MagicAttribute hurtMagicAtttribute = MagicAttribute.none;
    void Start()
    {
        chachedHitStop = new WaitForSecondsRealtime(0.1f); //0.1f後まで次の処理をしない
    }
    public void Hurt(float DMG,string AttackBelongedTeam,Vector2 knockBack,float hitStopTime,MagicAttribute magicAttribute = MagicAttribute.none){
        int resutDMG = DMGCalucation(DMG,magicAttribute);
        if(!gameObject.CompareTag(AttackBelongedTeam) && Health >= 0 && acceptDamage){

            gameObject.GetComponent<Rigidbody2D>().AddForce(knockBack,ForceMode2D.Impulse);
            if(hitStopTime > 0)StartCoroutine(HitStop(hitStopTime));
            Debug.Log($"[DamageLog]Hurt DMG:{DMG} margicAttribute:{magicAttribute} knockBack:{knockBack} HitStop:{hitStopTime}");
            Health -= resutDMG;
            if (gameObject.GetComponent<ZakoEnemyController>())
            {
                if (!overHeating)
                {
                    Heat += DMG; // lockOperationじゃなかったらHeatにDMGを追加します
                    Heat = Mathf.Clamp(Heat, 0.0f, HeatCapacity); // 0.0fは下限、HeatCapacityが上限。Heatは0以上HeatCapacity以下

                    if (Heat >= HeatCapacity)
                    {
                        overHeating = true; // HeatがHeatCapacity超えたらlockOperationがtrue
                    }
                }

            }
        }
        if(ParryReception && timeLastHurt > 1.0f){
            if(gameObject.GetComponent<PlayerController>())StartCoroutine(gameObject.GetComponent<PlayerController>().Parry(resutDMG));
        }
        timeLastHurt = 0;
    }
    public int DMGCalucation(float DMG ,MagicAttribute magicAttribute){
        float res = 0;
        switch(magicAttribute){
            case MagicAttribute m when(
                (m == MagicAttribute.aqua && myMagicAttribute == MagicAttribute.flame)
                || (m == MagicAttribute.flame && myMagicAttribute == MagicAttribute.terra)
                || (m == MagicAttribute.terra && myMagicAttribute == MagicAttribute.electro)
                || (m == MagicAttribute.electro && myMagicAttribute == MagicAttribute.aqua)
            ):
                if(gard)res = DMG; //gardの時はHealthからDMGを引く
                else res = DMG*2f; //gardでない時はDMGの二倍をHealthから引く
            break;
            
            case MagicAttribute m when(
                (m == MagicAttribute.flame && myMagicAttribute == MagicAttribute.aqua)
                || (m == MagicAttribute.terra && myMagicAttribute == MagicAttribute.flame)
                || (m == MagicAttribute.electro && myMagicAttribute == MagicAttribute.terra)
                || (m == MagicAttribute.aqua && myMagicAttribute == MagicAttribute.electro)
            ):
                if(!gard)res= DMG*0.5f; //gardでないときはDMGの0.5倍をHealthから引く
            break;
            default:
                if(gard)res= DMG*0.5f; 
                else res = DMG;
            break;
        }
        
        return (int)res;
    }
    IEnumerator HitStop(float time){
        Time.timeScale = 0.05f;
        if(Health != 0)  yield return new WaitForSecondsRealtime(time);
        else yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        yield break;
    }

    private void Update() {
        
        if (Heat == HeatCapacity)
        {
            // OverHeatTimeの間は冷却処理を行う
            Heating -= CoolingSpeed * Time.deltaTime;
            
        }

        if(overHeating)
        {
            OverHeatTime -= Time.deltaTime;
            if(OverHeatTime < 0.0f)
            {
            overHeating = false;

                if(!overHeating)
                     {
                         Heat = 0.0f;
                         OverHeatTime = 10.0f;
                     }

            Debug.Log("Heat");
            }
            
        }else {
            if(Heat > 0)Heat -= CoolingSpeed * Time.deltaTime;
        }
        timeLastHurt += Time.deltaTime;
        
    }
            
}

