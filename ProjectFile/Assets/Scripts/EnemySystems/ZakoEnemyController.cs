using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoEnemyController : MonoBehaviour
{
    public float normalMovementSpeed=5.0f;
    public float spotMovementSpeed = 10.0f;
    public float detecitrRadius=10.0f;
    public float attackRadius = 1.0f;
    public GameObject attackCollider;
    public ZakoEnemySkillFactory.ZakoEnemySkillKind skillKind;
    [ReadOnly]public GameObject target;
    [ReadOnly]public string nowState = "finding";
    private string oldState;
    private Rigidbody2D rb2D;
    Vector2 StartPos;
    [SerializeField,ReadOnly]private float staycount = 0;
    [ReadOnly]public int direction = 1;
    [ReadOnly]public bool onGround = false;
    [ReadOnly]public bool lockOperation = false;
    int stayAnimNum = 0;
    float bottom;
    [HideInInspector]public EntityBase entityBase;
    [HideInInspector]public float oldHealth;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2D=GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");
        StartPos=transform.position;
        bottom = gameObject.GetComponent<BoxCollider2D>().size.y/2 - gameObject.GetComponent<BoxCollider2D>().offset.y;
        entityBase = gameObject.GetComponent<EntityBase>();
    }

    void FixedUpdate()
    {
        if(!lockOperation){
            if(nowState == "following"){
                Following();
            }
            if(nowState == "finding"){
                Finding();
            }
        }
    }
    void Update(){
        if(!lockOperation){
            if(Mathf.Abs(target.transform.position.x - transform.position.x)<=attackRadius && nowState != "attacking")
            {
                rb2D.velocity = new Vector2(0,rb2D.velocity.y);
                nowState = "attacking";
                //攻撃
                ZakoEnemySkillFactory zakoEnemySkillFactory = new ZakoEnemySkillFactory();
                ZakoEnemySkillBase zakoEnemySkillBase = zakoEnemySkillFactory.Create(skillKind);
                StartCoroutine(zakoEnemySkillBase.Attack(this));
            }else{
                if(nowState != "attacking"){
                    if(Mathf.Abs(target.transform.position.x - transform.position.x) < detecitrRadius)
                    {
                        nowState = "following";
                    }else{
                        if(LotteryStayAnim()==0) nowState = "finding";
                        else nowState = "stopping";
                    }
                }
            }
        }
        if(entityBase.Health < oldHealth){
            lockOperation = true;
            if(entityBase.Health <= 0)nowState = "deathing";
            else nowState = "damaging";
        }
        isOnground();
        oldHealth = entityBase.Health;
    }
    void Finding(){
        if(transform.position.x - StartPos.x>2)
        {
            direction=-1;
        }
        else if(transform.position.x - StartPos.x < -2)
        {
            direction=1;
        }
        if(onGround)rb2D.AddForce(new Vector2(rb2D.mass * (direction * normalMovementSpeed - rb2D.velocity.x)/0.02f , 0));
    }
    void Following(){
        //追尾
        if (target.transform.position.x > transform.position.x)direction = 1;
        else direction = -1;
        if(onGround)rb2D.AddForce(new Vector2(rb2D.mass * (direction * spotMovementSpeed - rb2D.velocity.x)/0.05f , 0));
    }
    public void OnFinishAttack(){
        if(nowState == "deathing")Destroy(gameObject);
        nowState = "following";
        lockOperation = false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,detecitrRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRadius);
        Gizmos.DrawWireSphere(new Vector2(transform.position.x,transform.position.y-bottom),0.1f);
    }
    int LotteryStayAnim(){
        staycount += Time.deltaTime;
        if(staycount > 3){
            stayAnimNum = Random.Range(0,2);
            staycount = 0;
        }
        return stayAnimNum;
    }
    void isOnground(){
        int layermask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hitObject = Physics2D.BoxCast(transform.position,new Vector2(1,0.1f),0,Vector2.down,bottom,layermask);
        if(hitObject.collider){
            onGround = true;
        }
        else onGround = false;
    }
}
