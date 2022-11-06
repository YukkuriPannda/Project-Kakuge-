using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("InputField")]
    public float movementSpeed = 5; //movementSpeed * 入力(最大値1) = velocity.x
    public float JumpForce = 5; //JumpForce * 入力(最大値1) = velocity.y
    bool onGround = false;
    [HideInInspector]public Rigidbody2D rb2D;
    private EntityBase eBase;
    [SerializeField] Text devconsole;
    public Vector2 InputValueForMove;


    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        eBase = this.gameObject.GetComponent<EntityBase>();
    }
    void Update(){
        Move(InputValueForMove.x);
        Jump(InputValueForMove.y);
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ground")onGround = true;
    }
    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ground")onGround = false;
    }
    public void Move(float input) { // 移動方向/強さ -1~1 として
        rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x)/0.2f ,0));
        InputValueForMove = new Vector2(input,InputValueForMove.y);
    }

    public void Jump(float input) { // 最大値1,
        if (onGround) {
            input *= JumpForce;
            rb2D.velocity = new Vector2(rb2D.velocity.x,rb2D.velocity.y/4 + (input / rb2D.mass));
            InputValueForMove = new Vector2(InputValueForMove.x,input);
        }
    }
}