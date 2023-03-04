using System;
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
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2D=GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");
        StartPos=transform.position;
    }

    void FixedUpdate()
    {
        if(nowState == "following"){
            Following();
        }
        if(nowState == "finding"){
            Finding();
        }
    }
    void Update(){
        if(Mathf.Abs(target.transform.position.x - transform.position.x)<=attackRadius)
        {
            if(nowState != "attacking"){
                rb2D.velocity = new Vector2(0,rb2D.velocity.y);
                nowState = "attacking";
                //攻撃
                ZakoEnemySkillFactory zakoEnemySkillFactory = new ZakoEnemySkillFactory();
                ZakoEnemySkillBase zakoEnemySkillBase = zakoEnemySkillFactory.Create(skillKind);
                StartCoroutine(zakoEnemySkillBase.Attack(this));
            }
        }else{
            if(Mathf.Abs(target.transform.position.x - transform.position.x) < detecitrRadius)
            {
                nowState = "following";
            }else{
                nowState = "finding";
            }
        }
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
        rb2D.AddForce(new Vector2(rb2D.mass * (direction * normalMovementSpeed - rb2D.velocity.x) , 0));
    }
    void Following(){
        //追尾
        if (target.transform.position.x > transform.position.x)
        {
            rb2D.AddForce(new Vector2(rb2D.mass * (1 * spotMovementSpeed - rb2D.velocity.x) , 0));
        }
        else
        {
            rb2D.AddForce(new Vector2(rb2D.mass * (-1 * spotMovementSpeed - rb2D.velocity.x) , 0));
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,detecitrRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRadius);
    }
} 