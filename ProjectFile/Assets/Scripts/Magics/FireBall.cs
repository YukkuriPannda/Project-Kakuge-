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
            Destroy(Instantiate(hitParticlePrefab,transform.position,transform.rotation),2);
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(!other.gameObject.CompareTag(this.gameObject.tag)){
            Destroy(Instantiate(hitParticlePrefab,transform.position,transform.rotation),2);
            Destroy(this.gameObject);
            Debug.Log("Hit");
        }
    }
}
