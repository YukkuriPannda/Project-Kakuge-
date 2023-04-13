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
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float myRad = (transform.localEulerAngles.z + 180) * Mathf.Deg2Rad;
        Vector3 mypos = transform.position;
        Vector3 targpos = targetTrf.position;
        transform.Rotate(0, 0, Mathf.Clamp(Vector2.SignedAngle(new Vector2((float)Mathf.Cos(myRad), (float)Mathf.Sin(myRad)), (mypos - targpos).normalized),-1,1) * rotateSpeed * Time.deltaTime);
        if(t <= 3)transform.Translate(speed * t * Time.deltaTime,0,0);
        else transform.Translate(speed * Time.deltaTime,0,0);
        t += Time.deltaTime;
    }
}
