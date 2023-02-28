using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    public float damage;
    public MagicAttribute magicAttribute;
    public float hurtCoolTime;
    public Vector2 knockBack;
    public TeamBelonged teamBelonged;
    [Header("Infos")]
    [ReadOnly,SerializeField] private float hurtTime = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<EntityBase>()){
            hurtTime = 0;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<EntityBase>()){
            if(hurtTime == 0)other.gameObject.GetComponent<EntityBase>().Hurt(damage,teamBelonged,knockBack,magicAttribute);
            hurtTime += Time.deltaTime;
            if(hurtCoolTime <= hurtTime)hurtTime = 0;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position,transform.localScale);
    }
}
