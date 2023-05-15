using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public GameObject hitParticlePrefab;
    [ReadOnly]public float age;
    void Start(){
        if(speed < 0)gameObject.GetComponent<AttackBase>().knockBack *= new Vector2(-1,1);
    }
    void Update()
    {
        transform.Translate(speed *Time.deltaTime,0,0);
        age += Time.deltaTime;
        if(age > lifeTime){
            Bakuhatu();
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        Debug.Log(other.gameObject.CompareTag(this.gameObject.tag));
        if(!other.gameObject.CompareTag(this.gameObject.tag)){
            Bakuhatu();
        }
    }
    void Bakuhatu(){
        GameObject hitParticle = Instantiate(hitParticlePrefab,transform.position,transform.rotation);
        Destroy(hitParticle,2);
        Destroy(hitParticle.GetComponent<AttackBase>(),0.2f);
        Destroy(this.gameObject);
    }
}
