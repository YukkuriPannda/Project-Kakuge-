using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoEnemyController2 : MonoBehaviour
{
    private float detecitrRadius=3.0f;
    private float attackRadius = 1.0f;
    private Vector2 pos;
    private int num=1;
    private int count;
    public Rigidbody2D rb2D;
    public GameObject target;//(target=Player)
    public float movementSpeed=1.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2D=GetComponent<Rigidbody2D>();
        // if(PlayerがdetectirRadius以内&&attackRadius以内なら)
        if(gameObject.CompareTag("Player")==(detecitrRadius<=3.0f)&&(attackRadius<=1.0f))
        {
            GameObject.FindGameObjectsWithTag("Player");
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 3 * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //常に往復運動
        pos=transform.position;
        rb2D.AddForce(new Vector2(rb2D.mass * (num * movementSpeed - rb2D.velocity.x) , 0));

        if(pos.x>2)
        {
            num=-1;
        }
        else if(pos.x<-2)
        {
            num=1;
        }
    }
} 