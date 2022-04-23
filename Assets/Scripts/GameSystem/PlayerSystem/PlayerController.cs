using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("InputField")]
    public float movementSpeed = 200; //movementSpeed * 入力(最大値1) = velocity.x
    public float JumpForce = 200; //JumpForce * 入力(最大値1) = velocity.y

    private Rigidbody2D rb2D;
    private EntityBase eBase;
    [SerializeField] Text debconsole;


    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        eBase = this.gameObject.GetComponent<EntityBase>();
    }

    public void Move(float force) { // 移動方向/強さ -1~1 として
        force *= movementSpeed;
        debconsole.text += "\n" + force;
        rb2D.velocity = new Vector2(Mathf.Lerp(rb2D.velocity.x,force,0.9f)*Time.deltaTime ,rb2D.velocity.y);
    }

    public void Jump(float force) { // 最大値1,
        if (eBase.OnGround) {
            force *= JumpForce;
            rb2D.velocity = new Vector2(0,rb2D.velocity.y/4 + (force / rb2D.mass));
        }
    }
}