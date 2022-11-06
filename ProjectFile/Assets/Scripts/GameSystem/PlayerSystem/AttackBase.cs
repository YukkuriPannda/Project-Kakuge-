using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    public float damage;
    public MagicAttribute magicAttribute;
    public float hurtTime = 0;
    public float hurtCoolTime;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<EntityBase>()){
            hurtTime = 0;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<EntityBase>()){
            if(hurtTime == 0)other.gameObject.GetComponent<EntityBase>().Hurt(damage,magicAttribute);
            hurtTime += Time.deltaTime;
            if(hurtCoolTime <= hurtTime)hurtTime = 0;
        }
    }
}
