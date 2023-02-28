using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoEnemyController2 : MonoBehaviour
{
    public float movementSpeed=1.0f;
    public float detecitrRadius=10.0f;
    public float attackRadius = 1.0f;
    public GameObject attackCollider;
    [ReadOnly]public GameObject target;
    private Rigidbody2D rb2D;
    private Vector2 StartPos;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2D=GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");
        StartPos=transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Mathf.Abs(target.transform.position.x - transform.position.x)<=detecitrRadius)
        {
            if(Mathf.Abs(target.transform.position.x - transform.position.x)<=attackRadius)
            {
                //攻撃
            }else{
                //追尾
                if (target.transform.position.x > transform.position.x)
                {
                    rb2D.AddForce(new Vector2(rb2D.mass * (1 * movementSpeed - rb2D.velocity.x) , 0));
                }
                else
                {
                    rb2D.AddForce(new Vector2(rb2D.mass * (-1 * movementSpeed - rb2D.velocity.x) , 0));
                }
            }

        }else{
            int num=1;
            //常に往復運動

            if(transform.position.x - StartPos.x>2)
            {
                num=-1;
            }
            else if(transform.position.x - StartPos.x<-2)
            {
                num=1;
            }
            rb2D.AddForce(new Vector2(rb2D.mass * (num * movementSpeed - rb2D.velocity.x) , 0));
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