using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("InputField")]
    public float movementSpeed = 5; //movementSpeed * 入力(最大値1) = velocity.x
    public float JumpForce = 5; //JumpForce * 入力(最大値1) = velocity.y
    public float jumpInputArrowableTime = 0.5f;
    [SerializeField]bool onGround = false;
    [HideInInspector]public Rigidbody2D rb2D;
    private EntityBase eBase;
    private float playerHeight;
    [SerializeField] Text devconsole;
    public Vector2 InputValueForMove;
    Vector2 oldInputValueForMove;


    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        eBase = this.gameObject.GetComponent<EntityBase>();
        playerHeight = gameObject.GetComponent<BoxCollider2D>().size.y + gameObject.GetComponent<BoxCollider2D>().edgeRadius*2;
    }
    void FixedUpdate(){
        Move(InputValueForMove.x);
        Jump(InputValueForMove.y);
        isOnground();
        oldInputValueForMove = InputValueForMove;
    }
    
    public void Move(float input) { // 移動方向/強さ -1~1 として
        rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x)/0.1f ,0));//f=maの応用
        //rb2D.velocity = new Vector2(input * movementSpeed,0);
    }
    [SerializeField]float addingforceInJumping = 0;
    [SerializeField]bool jumping;
    public void Jump(float input) { // 最大値1,
        if (onGround && oldInputValueForMove.y == 0 && input !=0) {//接地していてかつ押した瞬間
            jumping = true;
            rb2D.AddForce(new Vector2(0,JumpForce * input * 5));
            addingforceInJumping = 0;
        }
        if(jumping)addingforceInJumping += 0.02f;
        else addingforceInJumping = 0;
        if(addingforceInJumping > 0&&addingforceInJumping <= jumpInputArrowableTime){
            rb2D.AddForce(new Vector2(0,JumpForce * input * (-addingforceInJumping *addingforceInJumping + jumpInputArrowableTime)/jumpInputArrowableTime));
        }

    }
    void isOnground(){
        int layermask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hitObject = Physics2D.CircleCast(transform.position,0.1f,Vector2.down,playerHeight/2,layermask);
        Debug.Log(layermask);
        if(hitObject.collider){
            onGround = true;
            /*onGround = (hitObject.transform.gameObject.CompareTag("Ground"));
            if(hitObject.transform.gameObject.CompareTag("Ground") && addingforceInJumping >0.06f){
                jumping = false;
            }*/
        }
        else onGround = false;
    }
    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(new Vector2(transform.position.x,transform.position.y-playerHeight/2),0.1f);
    }
}