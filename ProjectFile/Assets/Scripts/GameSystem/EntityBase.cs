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
    private WaitForSeconds chachedHitStop;
    void Start()
    {
        chachedHitStop = new WaitForSeconds(0.005f);
    }
    public void Hurt(float DMG,string AttackBelongedTeam,Vector2 knockBack,MagicAttribute magicAttribute = MagicAttribute.none){
        if(!gameObject.CompareTag(AttackBelongedTeam) && Health >= 0){
            gameObject.GetComponent<Rigidbody2D>().AddForce(knockBack,ForceMode2D.Impulse);
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
            StartCoroutine(HitStop());
            Debug.Log($"Hurt  DMG:{DMG} margicAttribute:{magicAttribute} knockBack:{knockBack}");
        }
    }
    IEnumerator HitStop(){
        Debug.Log("HitStop Start");
        Time.timeScale = 0.1f;
        yield return chachedHitStop;
        Time.timeScale = 1f;
        Debug.Log("HitStop End");
        yield break;
    }
}
public enum MagicAttribute{
    none,
    flame,
    aqua,
    electro,
    terra
}