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
    [ReadOnly]public float timeFromEnchanted = 0;
    public float magicStones = 6;
    public float MaxMagicStones = 6;
    public GameObject weapon;
    private float playerHeight;
    [System.Serializable]
    public class MagicHolder{
        public PlayerMagicFactory.PlayerFlameMagicKind flameMagic;
        public PlayerMagicFactory.PlayerFlameMagicKind aquaMagic;
        public PlayerMagicFactory.PlayerFlameMagicKind electroMagic;
        public PlayerMagicFactory.PlayerFlameMagicKind terraMagic;
    }
    [SerializeField]public MagicHolder magicHolder;
    [System.Serializable]
    public class AttackColliderPrefabs{
        public GameObject UpSlash;
        public GameObject Thrust;
        public GameObject DownSlash;
        public GameObject Enchant;
        public GameObject Gard;
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
    public Vector2 gardVector = new Vector2(0,0);
    public Vector2 InputValueForMove;
    Vector2 oldInputValueForMove;

    [Header("Info")]
    [SerializeField,ReadOnly] Text devconsole;
    [SerializeField,ReadOnly] bool onGround = false;
    [SerializeField,ReadOnly] public bool lockOperation = false;
    [ReadOnly]public int direction = 1;
    [HideInInspector]public Rigidbody2D rb2D;
    public EntityBase eBase;
    [HideInInspector]public float oldHealth;
    [ReadOnly]public PlayerStates nowPlayerState = PlayerStates.Stay;
    private AttackBase gardObject;
    public enum PlayerStates{
        Stay,
        Walking,
        Runing,
        UpSlash,
        Thrust,
        DownSlash,
        ShotMagicBullet,
        Garding,
        EnchantMySelf,
        ActivateSpecialMagic,
        Hurt
    }
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
            if(drawShapeName != "Gard") drawShapeName = "None";
            nowPlayerState = PlayerStates.Hurt;
        }
        oldHealth = eBase.Health;
        if(nowPlayerState!=PlayerStates.Garding && gardObject)Destroy(gardObject.gameObject,0.1f);
    }
    
    public void Move(float input) { // 移動方向/強さ -1~1 として
        if(!lockOperation){
            if(InputValueForMove.x != 0) nowPlayerState = PlayerStates.Runing;
            else nowPlayerState = PlayerStates.Stay;
            if(InputValueForMove.x > 0)direction = 1;
            if(InputValueForMove.x < 0)direction = -1;
            if(onGround) {
                rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x)/0.02f ,0));//f=maの応用
            }else{
                rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x)/0.5f,0));
            }
        }else{
            if(drawShapeName == "Gard")rb2D.AddForce(new Vector2(rb2D.mass * (input * 0 - rb2D.velocity.x)/0.1f ,0));
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
        if(oldDrawShapeName == "None" && !lockOperation){
            lockOperation = true;
            switch(drawShapeName){
                case "StraightToRight":{
                    nowPlayerState = PlayerStates.Thrust;
                    direction = 1;
                    GameObject DMGObject = Instantiate(attackColliders.Thrust,new Vector2(transform.position.x + 2f,transform.position.y),transform.rotation);
                    DMGObject.transform.GetChild(0).eulerAngles = new Vector3(0,0,0);
                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(attackBase.knockBack.x,attackBase.knockBack.y);
                    Destroy(DMGObject.GetComponent<AttackBase>(),0.2f);
                    Destroy(DMGObject,1);
                    for(int i = 0;i < 5;i++){
                        transform.Translate(0.4f,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }break;
                case "StraightToLeft":{
                    nowPlayerState = PlayerStates.Thrust;
                    direction = -1;
                    GameObject DMGObject = Instantiate(attackColliders.Thrust,new Vector2(transform.position.x - 2f,transform.position.y),transform.rotation);
                    DMGObject.transform.GetChild(0).eulerAngles = new Vector3(0,180,0);
                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(-attackBase.knockBack.x,attackBase.knockBack.y);
                    attackBase.magicAttribute = eBase.myMagicAttribute;
                    drawMagicSymbols = new List<DrawMagicSymbol>();
                    Destroy(DMGObject.GetComponent<AttackBase>(),0.2f);
                    Destroy(DMGObject,1);
                    for(int i = 0;i < 5;i++){
                        transform.Translate(-0.4f,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }break;
                case  "StraightToUp":{
                    if(drawShapePos.x > 0) direction =1;
                    else direction = -1;
                    nowPlayerState = PlayerStates.UpSlash;
                    yield return new WaitForSeconds(0.1f);

                    GameObject DMGObject = Instantiate(attackColliders.UpSlash,new Vector2(transform.position.x + 1.5f * direction,transform.position.y),transform.rotation);
                    if(direction < 0)DMGObject.transform.GetChild(0).eulerAngles = new Vector3(30,180,0);
                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(attackBase.knockBack.x * direction,attackBase.knockBack.y);
                    attackBase.magicAttribute = eBase.myMagicAttribute;
                    drawMagicSymbols = new List<DrawMagicSymbol>();
                    Destroy(DMGObject.GetComponent<AttackBase>(),0.2f);
                    Destroy(DMGObject,1);
                    DMGObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",EffectColor(eBase.myMagicAttribute));
                    for(int i = 0;i < 10;i++){
                        transform.Translate(0.08f*direction,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }break;
                case  "StraightToDown":{
                    if(drawShapePos.x > 0) direction =1;
                    else direction = -1;
                    nowPlayerState = PlayerStates.DownSlash;
                    yield return new WaitForSeconds(0.1f);

                    GameObject DMGObject = Instantiate(attackColliders.DownSlash,new Vector2(transform.position.x + 1.3f * direction,transform.position.y),Quaternion.identity);
                    if(direction < 0)DMGObject.transform.GetChild(0).eulerAngles = new Vector3(0,180,0);
                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(attackBase.knockBack.x * direction,attackBase.knockBack.y);
                    attackBase.magicAttribute = eBase.myMagicAttribute;
                    drawMagicSymbols = new List<DrawMagicSymbol>();
                    Destroy(DMGObject.GetComponent<AttackBase>(),0.2f);
                    Destroy(DMGObject,1);
                    DMGObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",EffectColor(eBase.myMagicAttribute));
                    for(int i = 0;i < 10;i++){
                        transform.Translate(0.08f*direction,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }break;
                case "RegularTriangle": case"InvertedTriangle": case "Thunder":case "Grass":{
                    drawMagicSymbols.Add(new DrawMagicSymbol(drawShapeName,1));
                    drawShapeName = "None";
                    oldDrawShapeName ="None";
                    lockOperation = false;

                }break;
                case "tap":{
                    if(drawMagicSymbols.Count > 0 && magicStones>0){
                        if(drawShapePos.x > 0) direction =1;
                        else direction = -1;
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
                            if(Vector2.Distance(drawShapePos,new Vector2(0,0)) > enchantDetectionRadius) {
                                //Bullet
                                nowPlayerState = PlayerStates.ShotMagicBullet;
                                yield return new WaitForSeconds(0.2f);
                                string path = "";
                                switch(magicAttribute){
                                    case MagicAttribute.flame:{
                                        path = "Magics/FlameBall";
                                    }break;
                                    case MagicAttribute.aqua:{
                                        path = "Magics/AquaBall";
                                    }break;
                                    case MagicAttribute.electro:{
                                        path = "Magics/ElectroBall";
                                    }break;
                                    case MagicAttribute.terra:{
                                        path = "Magics/TerraBall";
                                    }break;
                                }
                                FireBall bullet = Instantiate((GameObject)Resources.Load(path),transform.position + new Vector3(0.3f * direction,0.3f,0),transform.rotation).GetComponent<FireBall>();
                                bullet.speed *= direction;
                                bullet.gameObject.tag = "Player";
                                magicStones --;
                            }
                        }else{
                            //SpecialMagic
                            nowPlayerState = PlayerStates.ActivateSpecialMagic;
                            PlayerMagicFactory playerMagicFactory = new PlayerMagicFactory();
                            switch(drawMagicSymbols[0].magicSymbol){
                                case "RegularTriangle":{
                                    PlayerMagicBase zakoEnemySkillBase = playerMagicFactory.Create(magicHolder.flameMagic);
                                    StartCoroutine(zakoEnemySkillBase.ActivationFlameMagic(this));
                                }break;
                                case "InvertedTriangle":{
                                    PlayerMagicBase zakoEnemySkillBase = playerMagicFactory.Create(magicHolder.aquaMagic);
                                    StartCoroutine(zakoEnemySkillBase.ActivationAquaMagic(this));
                                }break;
                                case "Thunder":{
                                    PlayerMagicBase zakoEnemySkillBase = playerMagicFactory.Create(magicHolder.electroMagic);
                                    StartCoroutine(zakoEnemySkillBase.ActivationElectroMagic(this));
                                }break;
                                case "Grass":{
                                    PlayerMagicBase zakoEnemySkillBase = playerMagicFactory.Create(magicHolder.terraMagic);
                                    StartCoroutine(zakoEnemySkillBase.ActivationTerraMagic(this));
                                }break;
                            }
                            magicStones --;
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
                    }
                    drawShapeName = "None";
                    oldDrawShapeName ="None";
                    lockOperation = false;
                }
                break;
                case "Gard":{
                    if(drawMagicSymbols.Count > 0){
                        if(drawMagicSymbols[drawMagicSymbols.Count -1].magicSymbol != "Circle"){
                            //Enchant
                            nowPlayerState = PlayerStates.EnchantMySelf;
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
                            gameObject.GetComponent<EntityBase>().myMagicAttribute = magicAttribute;
                            AttackBase attackBase = Instantiate(attackColliders.Enchant,transform.position,Quaternion.identity).GetComponent<AttackBase>();
                            attackBase.magicAttribute = magicAttribute;
                            attackBase.tag = gameObject.tag;
                            attackBase.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color",MagicColorManager.GetColorFromMagicArticle(magicAttribute));
                            magicStones --;
                            timeFromEnchanted += Time.deltaTime;
                        }
                    }else{
                        //Gard
                        nowPlayerState = PlayerStates.Garding;
                        eBase.gard = true;
                        gardObject = Instantiate(attackColliders.Gard,transform.position,Quaternion.identity,transform).GetComponent<AttackBase>();
                        gardObject.magicAttribute = eBase.myMagicAttribute;
                        gardObject.tag = gameObject.tag;
                        gardObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color",MagicColorManager.GetColorFromMagicArticle(eBase.myMagicAttribute));
                        drawMagicSymbols = new List<DrawMagicSymbol>();
                        drawShapeName = "None";
                        oldDrawShapeName ="None";
                    }

                }break;
            }
            
        }else if(oldDrawShapeName == "Gard"){
            lockOperation = false;
            eBase.gard = false;
        }
    }
    public void UnLockOperation(){
        drawShapeName = "None";
        drawMagicSymbols = new List<DrawMagicSymbol>();
        lockOperation = false;
        Debug.Log("UnLocked operation");
    }
    Color EffectColor(MagicAttribute magicAttribute){
        Color res =Color.white;
        switch(magicAttribute){
            case MagicAttribute.flame:{
                res = MagicColorManager.flame;
            }break;
            case MagicAttribute.aqua:{
                res = MagicColorManager.aqua;
            }break;
            case MagicAttribute.electro:{
                res = MagicColorManager.electro;
            }break;
            case MagicAttribute.terra:{
                res = MagicColorManager.terra;
            }break;
        }
        return res;
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