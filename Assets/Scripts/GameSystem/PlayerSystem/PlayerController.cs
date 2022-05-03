using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("InputField")]
    public float movementSpeed = 5; //movementSpeed * 入力(最大値1) = velocity.x
    public float JumpForce = 200; //JumpForce * 入力(最大値1) = velocity.y

    public Rigidbody2D rb2D;
    private EntityBase eBase;
    [SerializeField] Text debconsole;


    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        eBase = this.gameObject.GetComponent<EntityBase>();
    }

    public void Move(float force) { // 移動方向/強さ -1~1 として
        float forcePower;
        if (eBase.OnGround) {
            forcePower = 0.2f;
        }
        else {
            forcePower = 0.2f;
        }
        rb2D.AddForce(new Vector2(rb2D.mass * (force * movementSpeed - rb2D.velocity.x)/forcePower ,0));
    }

    public void Jump(float force) { // 最大値1,
        if (eBase.OnGround) {
            force *= JumpForce;
            rb2D.velocity = new Vector2(0,rb2D.velocity.y/4 + (force / rb2D.mass));
        }
    }
}
