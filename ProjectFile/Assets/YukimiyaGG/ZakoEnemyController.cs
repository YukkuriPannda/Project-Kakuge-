using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoEnemyController : MonoBehaviour
{
    private float detecitrRadius=3.0f;
    private float attackRadius = 1.0f;
    private Vector2 pos;
    public Rigidbody2D rb2D;
    public GameObject target;//(target=Player)
    public float movementSpeed=1.5f;
    private bool flag;

    void Start()
    {
        if(gameObject.CompareTag("Player")==(detecitrRadius<=3.0f)&&(attackRadius<=1.0f))
        {
            GameObject.FindGameObjectsWithTag("Player");
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 3 * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x >= 3)
        {
            flag = true;
        }
          
 
        else if(transform.position.x <= -3)
        {
          flag = false;
        }
        
        if(flag)
        {
          transform.position = 
          Vector2.MoveTowards(transform.position, new Vector2(-3,0), movementSpeed * Time.deltaTime);
        }
        else if(!flag)
        {
          transform.position =
          Vector2.MoveTowards(transform.position, new Vector2(3,0), movementSpeed * Time.deltaTime);
        }
    }
}
