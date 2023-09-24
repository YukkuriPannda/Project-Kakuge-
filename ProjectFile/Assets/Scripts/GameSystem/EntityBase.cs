using System.Collections;
using System.Collections.Generic;
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

    public bool gard = false; //Gard
    public bool Invulnerable = false;
    public bool NoGravity = false;
    public bool OnGround = false;
    [ReadOnly]private  bool overHeating;
    private WaitForSecondsRealtime chachedHitStop;
    [ReadOnly]public MagicAttribute hurtMagicAtttribute = MagicAttribute.none;
    void Start()
    {
        chachedHitStop = new WaitForSecondsRealtime(0.1f); //0.1f後まで次の処理をしない
    }
    public void Hurt(float DMG,string AttackBelongedTeam,Vector2 knockBack,MagicAttribute magicAttribute = MagicAttribute.none,bool hitStop = true){
        if(!gameObject.CompareTag(AttackBelongedTeam) && Health >= 0){ //AttackBelongedTeamでない＆体力が０以上の時
            gameObject.GetComponent<Rigidbody2D>().AddForce(knockBack,ForceMode2D.Impulse);
            hurtMagicAtttribute = magicAttribute;
            switch(magicAttribute){
                case MagicAttribute m when(
                    (m == MagicAttribute.aqua && myMagicAttribute == MagicAttribute.flame)
                    || (m == MagicAttribute.flame && myMagicAttribute == MagicAttribute.terra)
                    || (m == MagicAttribute.terra && myMagicAttribute == MagicAttribute.electro)
                    || (m == MagicAttribute.electro && myMagicAttribute == MagicAttribute.aqua)
                ):
                    if(gard)Health -= DMG; //gardの時はHealthからDMGを引く
                    else Health-= DMG*2f; //gardでない時はDMGの二倍をHealthから引く
                break;
                
                case MagicAttribute m when(
                    (m == MagicAttribute.flame && myMagicAttribute == MagicAttribute.aqua)
                    || (m == MagicAttribute.terra && myMagicAttribute == MagicAttribute.flame)
                    || (m == MagicAttribute.electro && myMagicAttribute == MagicAttribute.terra)
                    || (m == MagicAttribute.aqua && myMagicAttribute == MagicAttribute.electro)
                ):
                    if(!gard)Health -= DMG*0.5f; //gardでないときはDMGの0.5倍をHealthから引く
                break;
                default:
                    if(gard)Health -= DMG*0.5f; 
                    else Health -= DMG;
                break;
            }
            if(hitStop)StartCoroutine(HitStop());//HitStopが有効の時、StartCoroutineが呼び出されHitStopが有効に
            Debug.Log($"[DamageLog]Hurt DMG:{DMG} margicAttribute:{magicAttribute} knockBack:{knockBack}");


       if (gameObject.GetComponent<ZakoEnemyController>())
{
    if (!overHeating)
    {
        Heat += DMG; // lockOperationじゃなかったらHeatにDMGを追加します
        Heat = Mathf.Clamp(Heat, 0.0f, HeatCapacity); // 0.0fは下限、HeatCapacityが上限。Heatは0以上HeatCapacity以下

        if (Heat == HeatCapacity)
        {
            overHeating = true; // HeatがHeatCapacity超えたらlockOperationがtrueになるんだ
            
        }
    }

}

        }
    }
    IEnumerator HitStop(){
        Time.timeScale = 0.05f;
        if(Health != 0) yield return chachedHitStop;
        else yield return new WaitForSecondsRealtime(0.2f);
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
            
        }
        
    }
            
}

