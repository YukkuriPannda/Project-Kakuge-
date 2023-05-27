using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    public float damage;
    public MagicAttribute magicAttribute;
    public float hurtCoolTime;
    public float hitStopTime = 0.1f;
    public Vector2 knockBack;
    public bool onlyFirstHurt = false;
    public bool radicalHurt = false;
    [Header("Infos")]
    [ReadOnly,SerializeField] private float hurtTime = 0;
    private float knockBackPower;
    private void OnEnable() {
        if(radicalHurt){
            knockBackPower = knockBack.magnitude;
        }    
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<EntityBase>()){
            hurtTime = Time.deltaTime;
            Vector2 resKnockBack = knockBack;
            if(radicalHurt)resKnockBack = (other.transform.position - transform.position).normalized * knockBackPower;
            other.gameObject.GetComponent<EntityBase>().Hurt(damage,gameObject.tag,resKnockBack,magicAttribute);
            Debug.Log("Hit FirstAttack");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(!onlyFirstHurt){
            if(radicalHurt)knockBack = (other.transform.position - transform.position).normalized * knockBackPower;
            if(hurtTime == 0){
                if(other.gameObject.GetComponent<EntityBase>())other.gameObject.GetComponent<EntityBase>().Hurt(damage,gameObject.tag,knockBack,magicAttribute);
            }
            hurtTime += Time.deltaTime;
            if(hurtCoolTime <= hurtTime){
                hurtTime = 0;
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position,transform.localScale);
    }
}
