using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : MonoBehaviour
{
    public Transform targetTrf;
    public float rotateSpeed;
    public float speed;
    void Start()
    {
        
    }
    void Update()
    {
        float myRad = (transform.localEulerAngles.z + 180) * Mathf.Deg2Rad;
        Vector3 mypos = transform.position;
        Vector3 targpos = targetTrf.position;
        transform.Rotate(0, 0, Mathf.Clamp(Vector2.SignedAngle(new Vector2((float)Mathf.Cos(myRad), (float)Mathf.Sin(myRad)), (mypos - targpos).normalized),-1,1) * rotateSpeed * Time.deltaTime);
        transform.Translate(speed * Time.deltaTime,0,0);
    }
}
