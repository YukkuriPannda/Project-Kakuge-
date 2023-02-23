using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MITEI : MonoBehaviour
{
    CharacterController Controller;
    Transform Target;
    GameObject Player;

    [SerializeField]
    float MoveSpeed = 2.0f;
    int DetecDist = 8;
    bool InArea = false;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Target = Player.transform;
 
        Controller = GetComponent<CharacterController>();
    }
    void Update()
    {
         if (InArea)
       {
            this.transform.LookAt(Target.transform);
 
            Vector2 direction = Target.position - this.transform.position;
            direction = direction.normalized;
 
            
            Vector2 velocity = direction * MoveSpeed;
             
            velocity.y = 0.0f;
 
           
            Controller.Move(velocity * Time.deltaTime);
        }
 
        
        Vector2 Apos = this.transform.position;
        Vector2 Bpos = Target.transform.position;
        float distance = Vector2.Distance(Apos, Bpos);
 
        
        if (distance > DetecDist)
        {
            InArea = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InArea = true;
    }
    
}
