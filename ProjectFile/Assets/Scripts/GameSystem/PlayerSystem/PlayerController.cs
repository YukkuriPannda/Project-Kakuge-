using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("States")]
    public float movementSpeed = 5; //movementSpeed * 入力(最大値1) = velocity.x
    public float JumpForce = 5; //JumpForce * 入力(最大値1) = velocity.y
    public float jumpInputArrowableTime = 0.5f;
    public float enchantDuraction = 5;
    public float enchantDetectionRadius = 2;
    private float timeFromEnchanted = 0;
    public GameObject weapon;
    private float playerHeight;
    [System.Serializable]
    public class MagicHolder{
        public PlayerMagicFactory.PlayerFlameMagicKind flameMagic;
    }
    [SerializeField]public MagicHolder magicHolder;
    [System.Serializable]
    public class AttackColliderPrefabs{
        public GameObject UpSlash;
        public GameObject Thrust;
        public GameObject DownSlash;
    }
    public AttackColliderPrefabs attackColliders;

    [System.Serializable]
    public class DrawMagicSymbol{
        public DrawMagicSymbol(string magicSymbol,float accuracy){
            this.magicSymbol = magicSymbol;
            this.accuracy = accuracy;
        } 
        public string magicSymbol;
        public float accuracy;
    }
    [SerializeField] public List<DrawMagicSymbol> drawMagicSymbols = new List<DrawMagicSymbol>();
    
    

    [Header("InputField")]
    public string drawShapeName = "None";
    private string oldDrawShapeName ="None";
    public Vector2 drawShapePos = new Vector2(0,0);
    public Vector2 InputValueForMove;
    Vector2 oldInputValueForMove;

    [Header("Info")]
    [SerializeField,ReadOnly] Text devconsole;
    [SerializeField,ReadOnly] bool onGround = false;
    [SerializeField,ReadOnly] public bool lockOperation = false;
    [HideInInspector]public Rigidbody2D rb2D;
    private EntityBase eBase;
    [HideInInspector]public float oldHealth;
    
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        eBase = this.gameObject.GetComponent<EntityBase>();
        playerHeight = gameObject.GetComponent<BoxCollider2D>().size.y + gameObject.GetComponent<BoxCollider2D>().edgeRadius*2;
    }
    void FixedUpdate(){
        Move(InputValueForMove.x);
        Jump(InputValueForMove.y);
        isOnground();
        oldInputValueForMove = InputValueForMove;
    }
    void Update() {
        if(oldDrawShapeName != drawShapeName){
            StartCoroutine(onChangeDrawShapeName());
        }
        oldDrawShapeName = drawShapeName;
        if(oldHealth > eBase.Health){
            lockOperation = true;
            drawShapeName = "None";
        }
        oldHealth = eBase.Health;
    }
    
    public void Move(float input) { // 移動方向/強さ -1~1 として
        if(!lockOperation)if(onGround) {
            rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x)/0.02f ,0));//f=maの応用
        }else{
            rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x)/0.5f,0));
        }
        if(timeFromEnchanted > 0){
            timeFromEnchanted += Time.deltaTime;
        }
        if(timeFromEnchanted > enchantDuraction){
            gameObject.GetComponent<EntityBase>().myMagicAttribute = MagicAttribute.none;
            timeFromEnchanted = 0;
        }
    }
    [SerializeField]float addingforceInJumping = 0;
    [SerializeField]bool jumping;
    public void Jump(float input) { // 最大値1,
        if (onGround && oldInputValueForMove.y == 0 && input !=0) {//接地していてかつ押した瞬間
            jumping = true;
            rb2D.AddForce(new Vector2(0,JumpForce * input * 5));
            addingforceInJumping = 0;
        }
        if(jumping)addingforceInJumping += 0.02f;
        else addingforceInJumping = 0;
        if(addingforceInJumping > 0 && addingforceInJumping <= jumpInputArrowableTime){
            rb2D.AddForce(new Vector2(0,JumpForce * input * (-addingforceInJumping *addingforceInJumping + jumpInputArrowableTime)/jumpInputArrowableTime));
        }

    }
    public IEnumerator onChangeDrawShapeName(){
        if(oldDrawShapeName == "None"){
            lockOperation = true;
            int direction = 0;
            if(drawShapePos.x > transform.position.x) direction =1;
            else direction = -1;
            switch(drawShapeName){
                case "StraightToRight":{

                    GameObject DMGObject = Instantiate(attackColliders.Thrust,new Vector2(transform.position.x + 2f,transform.position.y),transform.rotation);
                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(attackBase.knockBack.x,attackBase.knockBack.y);
                    Destroy(DMGObject,0.2f);
                    for(int i = 0;i < 5;i++){
                        transform.Translate(0.4f,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }break;
                case "StraightToLeft":{
                    
                    GameObject DMGObject = Instantiate(attackColliders.Thrust,new Vector2(transform.position.x - 2f,transform.position.y),transform.rotation);
                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(-attackBase.knockBack.x,attackBase.knockBack.y);
                    drawMagicSymbols = new List<DrawMagicSymbol>();
                    Destroy(DMGObject,0.2f);
                    for(int i = 0;i < 5;i++){
                        transform.Translate(-0.4f,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }break;
                case  "StraightToUp":{
                    yield return new WaitForSeconds(0.1f);

                    GameObject DMGObject = Instantiate(attackColliders.UpSlash,new Vector2(transform.position.x + 1.5f * direction,transform.position.y),transform.rotation);
                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(attackBase.knockBack.x * direction,attackBase.knockBack.y);
                    drawMagicSymbols = new List<DrawMagicSymbol>();
                    Destroy(DMGObject,0.2f);
                    for(int i = 0;i < 10;i++){
                        transform.Translate(0.08f*direction,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }break;
                case  "StraightToDown":{
                    yield return new WaitForSeconds(0.1f);

                    GameObject DMGObject = Instantiate(attackColliders.DownSlash,new Vector2(transform.position.x + 1.5f * direction,transform.position.y),transform.rotation);
                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(attackBase.knockBack.x * direction,attackBase.knockBack.y);
                    drawMagicSymbols = new List<DrawMagicSymbol>();
                    Destroy(DMGObject,0.2f);
                    for(int i = 0;i < 10;i++){
                        transform.Translate(0.08f*direction,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }break;
                case "RegularTriangle":{
                    drawMagicSymbols.Add(new DrawMagicSymbol("RegularTriangle",1));
                    drawShapeName = "None";
                    oldDrawShapeName ="None";
                    lockOperation = false;
                }break;
                case "InvertedTriangle":{
                    drawMagicSymbols.Add(new DrawMagicSymbol("InvertedTriangle",1));
                    drawShapeName = "None";
                    oldDrawShapeName ="None";
                    lockOperation = false;
                }break;
                case "Thunder":{
                    drawMagicSymbols.Add(new DrawMagicSymbol("Thunder",1));
                    drawShapeName = "None";
                    oldDrawShapeName ="None";
                    lockOperation = false;
                }break;
                case "Grass":{
                    drawMagicSymbols.Add(new DrawMagicSymbol("Grass",1));
                    drawShapeName = "None";
                    oldDrawShapeName ="None";
                    lockOperation = false;
                }break;
                case "tap":{
                    if(drawMagicSymbols.Count > 0){
                        if(drawMagicSymbols[drawMagicSymbols.Count-1].magicSymbol != "Circle"){
                            //NormalMagic
                            MagicAttribute magicAttribute = 0;
                            switch(drawMagicSymbols[0].magicSymbol){
                                case "RegularTriangle":{
                                    magicAttribute = MagicAttribute.flame;
                                }break;
                                case "InvertedTriangle":{
                                    magicAttribute = MagicAttribute.aqua;
                                }break;
                                case "Thunder":{
                                    magicAttribute = MagicAttribute.electro;
                                }break;
                                case "Grass":{
                                    magicAttribute = MagicAttribute.terra;
                                }break;
                            }
                            if(Vector2.Distance(drawShapePos,transform.position) < enchantDetectionRadius) {
                                //Enchant
                                gameObject.GetComponent<EntityBase>().myMagicAttribute = magicAttribute;
                            }
                            else {
                                //Bullet
                                yield return new WaitForSeconds(0.2f);
                                FireBall bullet = Instantiate((GameObject)Resources.Load("Magics/FireBall"),transform.position + new Vector3(0.3f * direction,0.3f,0),transform.rotation).GetComponent<FireBall>();
                                bullet.speed *= direction;
                                bullet.gameObject.tag = "Player";
                            }
                            timeFromEnchanted += Time.deltaTime;
                        }else{
                            //SpecialMagic
                            PlayerMagicFactory playerMagicFactory = new PlayerMagicFactory();
                            PlayerMagicBase zakoEnemySkillBase = playerMagicFactory.Create(magicHolder.flameMagic);
                            StartCoroutine(zakoEnemySkillBase.ActivationFlameMagic(this));
                        }
                    }else {     
                        drawShapeName = "None";
                        oldDrawShapeName ="None";
                        lockOperation = false;
                    }
                }break;
                case "Circle":{
                    if(drawMagicSymbols.Count >0){
                        drawMagicSymbols.Add(new DrawMagicSymbol("Circle",1));
                        drawShapeName = "None";
                        oldDrawShapeName ="None";
                        lockOperation = false;
                    }else{
                        drawShapeName = "None";
                        oldDrawShapeName ="None";
                        lockOperation = false;
                    }
                }
                break;
            }
        }

    }
    public void OnFinishAttack(){
        drawShapeName = "None";
        drawMagicSymbols = new List<DrawMagicSymbol>();
        lockOperation = false;
        Debug.Log("UnLocked operation");
    }
    void isOnground(){
        int layermask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hitObject = Physics2D.CircleCast(transform.position,0.1f,Vector2.down,playerHeight/2,layermask);
        if(hitObject.collider){
            onGround = true;
        }
        else onGround = false;
    }
    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(new Vector2(transform.position.x,transform.position.y-playerHeight/2),0.1f);
        Gizmos.DrawWireSphere(transform.position,enchantDetectionRadius);
    }
}