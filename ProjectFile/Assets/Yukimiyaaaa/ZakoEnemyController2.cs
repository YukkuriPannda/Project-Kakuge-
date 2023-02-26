using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoEnemyController2 : MonoBehaviour
{
    private float detecitrRadius=3.0f;
    private float attackRadius = 1.0f;
    private Vector2 StartPos;
    private int num=1;
    private int count;
    private Vector2 PlayerPosition;
    private Vector2 EnemyPosition;
    private GameObject playerObject;
    public Rigidbody2D rb2D;
    public GameObject target;//(target=Player)
    public float movementSpeed=1.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2D=GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindWithTag("Player");
        StartPos=transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Mathf.Abs(playerObject.transform.position.x - transform.position.x)<=detecitrRadius)
        {
            //追尾
        PlayerPosition = playerObject.transform.position;
        EnemyPosition = transform.position;
        
        if (PlayerPosition.x > EnemyPosition.x)
        {
            rb2D.AddForce(new Vector2(rb2D.mass * (1 * movementSpeed - rb2D.velocity.x) , 0));
        }
        else if (PlayerPosition.x < EnemyPosition.x)
        {
            rb2D.AddForce(new Vector2(rb2D.mass * (-1 * movementSpeed - rb2D.velocity.x) , 0));
        }

            //攻撃
        if(Mathf.Abs(playerObject.transform.position.x - transform.position.x)<=attackRadius)
        {

        }else{
            //追尾再び
            if (PlayerPosition.x > EnemyPosition.x)
            {
                  rb2D.AddForce(new Vector2(rb2D.mass * (1 * movementSpeed - rb2D.velocity.x) , 0));
            }
            else if (PlayerPosition.x < EnemyPosition.x)
            {
                  rb2D.AddForce(new Vector2(rb2D.mass * (-1 * movementSpeed - rb2D.velocity.x) , 0));
            }

        }

        }else{
        //常に往復運動
        rb2D.AddForce(new Vector2(rb2D.mass * (num * movementSpeed - rb2D.velocity.x) , 0));

        if(transform.position.x - StartPos.x>2)
        {
            num=-1;
        }
        else if(transform.position.x - StartPos.x<-2)
        {
            num=1;
        }
        }
        
    }
} 