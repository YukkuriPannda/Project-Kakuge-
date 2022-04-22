using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float maxMoveSpeed = 10;
    public float maxJumpForce;
    public int afterJumpingCount;
    public Vector2 inputMoveInfo;

    Rigidbody2D rb2D;
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        moveCon();
    }
    void moveCon()
    {
        rb2D.AddForce(new Vector2(rb2D.mass * (maxMoveSpeed * inputMoveInfo.x - rb2D.velocity.x) * 6 ,0));
        if(inputMoveInfo.y >= 0.2f && afterJumpingCount <= 5){
            rb2D.AddForce(transform.up * maxJumpForce * inputMoveInfo.y);
            afterJumpingCount ++;
        }    
        if(inputMoveInfo.y <= 0.2f) afterJumpingCount = 0;
    }
}
