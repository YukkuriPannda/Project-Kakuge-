using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    public float damage;
    public MagicAttribute magicAttribute;
    public float hurtCoolTime;
    public bool hitStop = true;
    public float hitStopTime = 0.1f;
    public Vector2 knockBack;
    public bool onlyFirstHurt = false;
    public bool radicalHurt = false;
    [Header("Infos")]
    [ReadOnly,SerializeField] private float hurtTime = 0;
    private float knockBackPower;
    [System.Serializable]
    class HurtObjectInfo{
        public EntityBase eBase;
        public float time = 0;
        public HurtObjectInfo(EntityBase eBase,float time = 0){
            this.eBase = eBase;
            this.time = time;
        }
    }
    [ReadOnly,SerializeField] private List<HurtObjectInfo> hurtObjectInfos = new List<HurtObjectInfo>();
    private void OnEnable() {
        if(radicalHurt){
            knockBackPower = knockBack.magnitude;
        }    
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<EntityBase>()){
            hurtObjectInfos.Add(new HurtObjectInfo(other.gameObject.GetComponent<EntityBase>()));
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        foreach(HurtObjectInfo hurtObjectInfo in hurtObjectInfos){
            if(hurtObjectInfo.time == 0){
                hurtObjectInfo.eBase.Hurt(damage,gameObject.tag,knockBack,magicAttribute,hitStop);
            }
            hurtObjectInfo.time += Time.deltaTime;
            if(hurtObjectInfo.time >= hurtCoolTime)hurtObjectInfo.time = 0;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        EntityBase eBase = other.gameObject.GetComponent<EntityBase>();
        if(eBase){
            for(int i = 0;i < hurtObjectInfos.Count;i++){
                if(hurtObjectInfos[i].eBase == eBase)hurtObjectInfos.Remove(hurtObjectInfos[i]);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position,transform.localScale);
    }
}
