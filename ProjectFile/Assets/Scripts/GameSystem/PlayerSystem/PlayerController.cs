using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("States")]
    public float movementSpeed = 5; //movementSpeed * 入力(最大値1) = velocity.x
    public float JumpForce = 5; //JumpForce * 入力(最大値1) = velocity.y
    public float jumpInputArrowableTime = 0.5f;
    public GameObject weapon;
    private float playerHeight;
    public AttackColliderPrefabs attackColliders;
    [System.Serializable]
    public class AttackColliderPrefabs{
        public GameObject UpSlash;
        public GameObject Thrust;
        public GameObject DownSlash;
    }

    [Header("InputField")]
    public string drawShapeName = "None";
    private string oldDrawShapeName ="None";
    public Vector2 drawShapePos = new Vector2(0,0);
    public Vector2 InputValueForMove;
    Vector2 oldInputValueForMove;

    [Header("Info")]
    [SerializeField] Text devconsole;
    [SerializeField] bool onGround = false;
    [SerializeField] public bool lockOperation = false;
    [HideInInspector]public Rigidbody2D rb2D;
    private EntityBase eBase;
    
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
    void Update() {
        if(oldDrawShapeName != drawShapeName){
            StartCoroutine(onChangeDrawShapeName());
        }
        oldDrawShapeName = drawShapeName;
    }
    
    public void Move(float input) { // 移動方向/強さ -1~1 として
        if(!lockOperation)rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x) ,0));//f=maの応用
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
    public IEnumerator onChangeDrawShapeName(){
        if(oldDrawShapeName == "None"){
            lockOperation = true;
            int direction = 0;
            if(drawShapePos.x > transform.position.x) direction =-1;
            else direction = 1;
            switch(drawShapeName){
                case "StraightToRight":
                    for(int i = 0;i < 10;i++){
                        transform.Translate(-0.15f,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                break;
                case "StraightToLeft":
                    for(int i = 0;i < 10;i++){
                        transform.Translate(0.15f,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                break;
                case  "StraightToUp":
                yield return new WaitForSeconds(0.1f);
                    Destroy(Instantiate(attackColliders.UpSlash,new Vector2(transform.position.x + 1.7f,transform.position.y),transform.rotation),0.1f);
                    for(int i = 0;i < 10;i++){
                        transform.Translate(0.08f*direction,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                break;
                case  "StraightToDown":
                    for(int i = 0;i < 10;i++){
                        transform.Translate(0.08f*direction,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                break;
            }
        }
    }
    public void OnFinishAttack(){
        drawShapeName = "None";
        lockOperation = false;
    }
    void isOnground(){
        int layermask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hitObject = Physics2D.CircleCast(transform.position,0.1f,Vector2.down,playerHeight/2,layermask);
        if(hitObject.collider){
            onGround = true;
        }
        else onGround = false;
    }
    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(new Vector2(transform.position.x,transform.position.y-playerHeight/2),0.1f);
    }
}