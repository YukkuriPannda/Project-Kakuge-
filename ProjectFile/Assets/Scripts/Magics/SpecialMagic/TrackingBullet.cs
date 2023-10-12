using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed;
    public float speed;
    public float lifeTime;
    public float t;
    public float force = 50;
    public float delay = 0.5f;
    private Rigidbody2D rb2D;
    public GameObject hitParticlePrefab;
    public Vector2 offsetPos;
    private Vector2 startpos;
    [ReadOnly,SerializeField]Vector3 targpos;
    [ReadOnly]bool LockOn = false;
    IEnumerator Onenable()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rb2D.AddForce(transform.right*force);
        startpos = transform.position;
        yield return new WaitForSeconds(delay);
        Vector2 sp = rb2D.velocity;
        GameObject[] targets = gameObject.CompareTag("Enemy") ? GameObject.FindGameObjectsWithTag("Player"):GameObject.FindGameObjectsWithTag("Enemy");
        if(targets.Length > 0){
            target = targets[0];//仮で代入
            foreach(GameObject candidateTarget in targets){
                if(Vector2.Distance(candidateTarget.transform.position,transform.position) < Vector2.Distance(target.transform.position,transform.position))target = candidateTarget;
            }
            targpos = target.transform.position;
        }else {
            targpos = (Vector3)(startpos + new Vector2(5,0));
        }
        for(float time = 0;time < 0.2f;time += Time.deltaTime){
            float myRad = (transform.localEulerAngles.z + 180) * Mathf.Deg2Rad;
            Vector3 mypos = transform.position;
            transform.Rotate(0, 0, Mathf.Clamp(Vector2.SignedAngle(new Vector2((float)Mathf.Cos(myRad), (float)Mathf.Sin(myRad)), (mypos - targpos).normalized),-1,1) * rotateSpeed *4 * Time.deltaTime);
            
            rb2D.velocity =sp*(0.5f-time*0.25f);
            yield return null;
        }
        rb2D.bodyType = RigidbodyType2D.Static;
        
        while(t<lifeTime){
            targpos = target.transform.position + (Vector3)offsetPos;

            float myRad = (transform.localEulerAngles.z + 180) * Mathf.Deg2Rad;
            Vector3 mypos = transform.position;
            transform.Rotate(0, 0, Mathf.Clamp(Vector2.SignedAngle(new Vector2((float)Mathf.Cos(myRad), (float)Mathf.Sin(myRad)), (mypos - targpos).normalized),-1,1) * rotateSpeed * Time.deltaTime);
            
            if(t <= 1)transform.Translate((speed * t + 2f )* Time.deltaTime,0,0);
            else transform.Translate(speed * Time.deltaTime,0,0);
            t += Time.deltaTime;
            yield return null;
        }
        Exprosion();
    }
    private void OnEnable() {
        StartCoroutine(Onenable());
    }
    void OnTriggerEnter2D(Collider2D other){
        if(!other.gameObject.CompareTag(this.gameObject.tag) && !other.gameObject.CompareTag("NPC")){
            Exprosion();
        }
    }
    void Exprosion(){
        GameObject exprosion = Instantiate(hitParticlePrefab,transform.position,transform.rotation);
        exprosion.tag = gameObject.CompareTag("Enemy") ?  "Enemy":"Player";
        Destroy(exprosion,2);
        Destroy(exprosion.GetComponent<AttackBase>(),0.1f);
        Destroy(this.gameObject);
    }
}
