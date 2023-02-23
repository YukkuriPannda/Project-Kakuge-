using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakoEnemyController : MonoBehaviour

{
    private float detecitrRadius=3.0f;
    private float attackRadius = 1.0f;
    [HideInInspector] public Rigidbody2D rb2D;
    public float movementSpeed = 5.0f;
    [SerializeField] GameObject Player;
    private readonly float speed = 5.0f;
    
    public Vector2 InputValueForMove;
    Vector2 oldInputValueForMove;
        class SkillFactory
    {
        public static readonly string[] skills = {
        "LightningSkill",
        "HealSkill"
    };
        public enum SkillKind
        {
            Lightning,
            Heal
        }
    }


        ZakoEnemyController tes;
        ZakoEnemyController tos;
     void Moving(){
        oldInputValueForMove = InputValueForMove;
        Move(InputValueForMove.x);
     }

        public void Move(float input)
        {

            rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x) / 0.1f, 0));
            // 往復運動
        }
        [SerializeField]float walkingspeed=1.0f;

    public object Heal { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        tes=Player.GetComponent<ZakoEnemyController>();
        tes.detecitrRadius=GetAxisRaw("Player");
        tos=Player.GetComponent <ZakoEnemyController >();
        tos.attackRadius=GetAxisRaw("Player");
        // if(PlayerがdetectirRadius以内かつ&&attackRadius以内なら)
        if((detecitrRadius<=3.0f)&&(attackRadius<=1.0f))
        {
            //敵(Player)に向かって移動
            GameObject.Find("Player");
            transform.position = Vector2.MoveTowards(
          transform.position,
          Player.transform.position,
          speed * Time.deltaTime);
        }
        // else if(PlayerがdetectirRadiusでありAttackRadiusでないなら)
        
        else if((detecitrRadius<=3.0f)&&(attackRadius>1.0f))
        {
           
        }
        else{
        walkingspeed=1.0f; //往復運動
        }
    }

    private void WriteLine(string v)
    {
        throw new NotImplementedException();
    }

    private float GetAxisRaw(string v)
    {
        throw new NotImplementedException();
    }
}




