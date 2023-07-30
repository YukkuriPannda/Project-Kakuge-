using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    //HP管理や被攻撃処理をする。HPを持つオブジェクト全部にアタッチ
    public float MaxHealth = 65535;
    public float Health = 65535;
    public MagicAttribute myMagicAttribute = MagicAttribute.none;

    public bool gard = false;
    public bool Invulnerable = false;
    public bool NoGravity = false;
    public bool OnGround = false;
    private WaitForSecondsRealtime chachedHitStop;
    [ReadOnly]public MagicAttribute hurtMagicAtttribute = MagicAttribute.none;
    void Start()
    {
        chachedHitStop = new WaitForSecondsRealtime(0.1f);
    }
    public void Hurt(float DMG,string AttackBelongedTeam,Vector2 knockBack,MagicAttribute magicAttribute = MagicAttribute.none,bool hitStop = true){
        if(!gameObject.CompareTag(AttackBelongedTeam) && Health >= 0){
            gameObject.GetComponent<Rigidbody2D>().AddForce(knockBack,ForceMode2D.Impulse);
            hurtMagicAtttribute = magicAttribute;
            switch(magicAttribute){
                case MagicAttribute m when(
                    (m == MagicAttribute.aqua && myMagicAttribute == MagicAttribute.flame)
                    || (m == MagicAttribute.flame && myMagicAttribute == MagicAttribute.terra)
                    || (m == MagicAttribute.terra && myMagicAttribute == MagicAttribute.electro)
                    || (m == MagicAttribute.electro && myMagicAttribute == MagicAttribute.aqua)
                ):
                    if(gard)Health -= DMG;
                    else Health-= DMG*2f;
                break;
                
                case MagicAttribute m when(
                    (m == MagicAttribute.flame && myMagicAttribute == MagicAttribute.aqua)
                    || (m == MagicAttribute.terra && myMagicAttribute == MagicAttribute.flame)
                    || (m == MagicAttribute.electro && myMagicAttribute == MagicAttribute.terra)
                    || (m == MagicAttribute.aqua && myMagicAttribute == MagicAttribute.electro)
                ):
                    if(!gard)Health -= DMG*0.5f;
                break;
                default:
                    if(gard)Health -= DMG*0.5f;
                    else Health -= DMG;
                break;
            }
            if(hitStop)StartCoroutine(HitStop());
            Debug.Log($"[DamageLog]Hurt DMG:{DMG} margicAttribute:{magicAttribute} knockBack:{knockBack}");
        }
    }
    IEnumerator HitStop(){
        Time.timeScale = 0.05f;
        if(Health != 0) yield return chachedHitStop;
        else yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1f;
        yield break;
    }
}