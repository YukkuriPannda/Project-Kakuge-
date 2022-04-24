using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("InputField")]
    public float maxMoveSpeed = 10;
    public float maxJumpForce;
    public float maxJumpSpeed;
    int afterJumpingCount;
    [Space(10)]
    public Vector2 inputMoveInfo;
    public string nowState;
    public bool onGraund;
    Rigidbody2D rb2D;
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        
    }
    private void FixedUpdate() {
        moveCon();
    }
    void moveCon()
    {
        rb2D.AddForce(new Vector2(rb2D.mass * (maxMoveSpeed * inputMoveInfo.x - rb2D.velocity.x) / 0.2f,0));//F=ma
        if(inputMoveInfo.y >= 0.2f && afterJumpingCount <= 5 && rb2D.velocity.y <= maxJumpSpeed){
            rb2D.AddForce(transform.up * maxJumpForce * inputMoveInfo.y);
            afterJumpingCount ++;
        }    
        if(inputMoveInfo.y <= 0.2f && inputMoveInfo.y >= -0.2f && onGraund) afterJumpingCount = 0;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Stage") onGraund = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Stage") onGraund = false;
    }
}