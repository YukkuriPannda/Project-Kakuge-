using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : MonoBehaviour
{
    public Transform targetTrf;
    public float rotateSpeed;
    public float speed;
    public float t;
    private Rigidbody2D rb2D;
    public GameObject hitParticlePrefab;
    private Vector2 startpos;
    [ReadOnly]Vector3 targpos;
    [ReadOnly]bool LockOn = false;
    IEnumerator Onenable()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rb2D.AddForce(transform.right*50);
        startpos = transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector2 sp = rb2D.velocity;
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        if(targets.Length > 0){
            GameObject target = targets[0];//仮で代入
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
        
        while(true){
            float myRad = (transform.localEulerAngles.z + 180) * Mathf.Deg2Rad;
            Vector3 mypos = transform.position;
            transform.Rotate(0, 0, Mathf.Clamp(Vector2.SignedAngle(new Vector2((float)Mathf.Cos(myRad), (float)Mathf.Sin(myRad)), (mypos - targpos).normalized),-1,1) * rotateSpeed * Time.deltaTime);
            
            if(t <= 1)transform.Translate((speed * t + 2f )* Time.deltaTime,0,0);
            else transform.Translate(speed * Time.deltaTime,0,0);
            t += Time.deltaTime;
            yield return null;
        }
    }
    private void OnEnable() {
        StartCoroutine(Onenable());
    }
    void OnTriggerEnter2D(Collider2D other){
        if(!other.gameObject.CompareTag(this.gameObject.tag) && !other.gameObject.CompareTag("NPC")){
            Destroy(Instantiate(hitParticlePrefab,transform.position,transform.rotation),2);
            Destroy(this.gameObject);
        }
    }
}
