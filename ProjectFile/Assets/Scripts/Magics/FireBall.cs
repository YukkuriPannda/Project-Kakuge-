using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float magicEfficiency;
    public GameObject hitParticlePrefab;
    [ReadOnly]public float age;
    private bool exprosed = false;
    void Start(){
        if(speed < 0)gameObject.GetComponent<AttackBase>().knockBack *= new Vector2(-1,1);
    }
    void Update()
    {
        transform.Translate(speed *Time.deltaTime,0,0);
        age += Time.deltaTime;
        if(age > lifeTime){
            if(!exprosed)Bakuhatu();
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(!other.gameObject.CompareTag(this.gameObject.tag) && !other.gameObject.CompareTag("NPC")){
            if(!exprosed)Bakuhatu();
            exprosed = true;
        }
    }
    void Bakuhatu(){
        GameObject hitParticle = Instantiate(hitParticlePrefab,transform.position,transform.rotation);
        hitParticle.GetComponent<AttackBase>().damage *= magicEfficiency/50;
        Destroy(hitParticle,2);
        Destroy(hitParticle.GetComponent<AttackBase>(),0.2f);
        Destroy(gameObject);
    }
}
