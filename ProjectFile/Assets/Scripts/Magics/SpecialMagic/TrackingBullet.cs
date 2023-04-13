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
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rotateSpeed = Random.Range(150,210);
    }
    void Update()
    {
        float myRad = (transform.localEulerAngles.z + 180) * Mathf.Deg2Rad;
        if(targetTrf) {
            Vector3 mypos = transform.position;
            Vector3 targpos = targetTrf.position;
            transform.Rotate(0, 0, Mathf.Clamp(Vector2.SignedAngle(new Vector2((float)Mathf.Cos(myRad), (float)Mathf.Sin(myRad)), (mypos - targpos).normalized),-1,1) * rotateSpeed * Time.deltaTime);
        }
        else{
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject target = targets[0];//仮で代入
            foreach(GameObject candidateTarget in targets){
                if(Vector2.Distance(candidateTarget.transform.position,transform.position) < Vector2.Distance(target.transform.position,transform.position))target = candidateTarget;
            }
            targetTrf = target.transform;
        }
        if(t <= 3 - speed *0.5f)transform.Translate(speed * t * Time.deltaTime + speed *0.5f* Time.deltaTime,0,0);
        else transform.Translate(speed * Time.deltaTime,0,0);
        t += Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other){
        Debug.Log(other.gameObject.CompareTag(this.gameObject.tag));
        if(!other.gameObject.CompareTag(this.gameObject.tag)){
            Destroy(Instantiate(hitParticlePrefab,transform.position,transform.rotation),2);
            Destroy(this.gameObject);
        }
    }
}
