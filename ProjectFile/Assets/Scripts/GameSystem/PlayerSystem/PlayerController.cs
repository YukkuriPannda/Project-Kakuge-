using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("States")]
    public float movementSpeed = 5; //movementSpeed * 入力(最大値1) = velocity.x
    public float JumpForce = 5; //JumpForce * 入力(最大値1) = velocity.y
    public float jumpInputArrowableTime = 0.5f;
    public float enchantDuraction = 5;
    public float enchantDetectionRadius = 2;
    public float magicStones = 6;
    public float MaxMagicStones = 6;
    public GameObject weapon;
    private float playerHeight;
    private GameObject GameManeger;
    [System.Serializable]
    public class MagicHolder{
        public PlayerMagicFactory.MagicKind flameMagic;
        public PlayerMagicFactory.MagicKind aquaMagic;
        public PlayerMagicFactory.MagicKind electroMagic;
        public PlayerMagicFactory.MagicKind terraMagic;
    }
    [SerializeField]public MagicHolder magicHolder;
    [System.Serializable]
    public class AttackColliderPrefabs{
        public GameObject Up;
        public GameObject Thrust;
        public GameObject Down;
        public GameObject Enchant;
        public GameObject Gard;
        public GameObject counterAttack;
    }
    public AttackColliderPrefabs attackColliders;
    public GameObject InventryObj;

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

    [Header("Infos")]
    [SerializeField,ReadOnly] Text devconsole;
    [SerializeField,ReadOnly] bool onGround = false;
    [SerializeField,ReadOnly] public bool lockOperation = false;
    [ReadOnly]public int direction = 1;
    [ReadOnly]public bool openingInventry;
    [HideInInspector]public Rigidbody2D rb2D;
    [HideInInspector]public EntityBase eBase;
    [HideInInspector]public float oldHealth;
    [HideInInspector]public float CounterDMG;
    [ReadOnly]public PlayerStates nowPlayerState = PlayerStates.Stay;
    private GameObject gardObject;
    [ReadOnly]public float timeFromEnchanted = 0;
    [ReadOnly]public bool CounterReception = false;

    [ReadOnly]public float upForwardDistance;
    [ReadOnly]public float thrustForwardDistance;
    [ReadOnly]public float downForwardDistance;
    [ReadOnly]public float counterAttackForwardDistance;
    public enum PlayerStates{
        Stay,
        Walking,
        Runing,
        Up,
        Thrust,
        Down,
        ShotMagicBullet,
        Garding,
        CounterAttack,
        EnchantMySelf,
        ActivateSpecialMagic,
        Hurt
    }
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        eBase = this.gameObject.GetComponent<EntityBase>();
        playerHeight = gameObject.GetComponent<BoxCollider2D>().size.y + gameObject.GetComponent<BoxCollider2D>().edgeRadius*2;
        InventryObj.SetActive(false);
        GameManeger = GameObject.Find("GameManager");
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
            if(!eBase.ParryReception && nowPlayerState != PlayerStates.CounterAttack && CounterReception){
                Debug.Log("HUrt!!");
                lockOperation = true;
                nowPlayerState = PlayerStates.Hurt;
            }
        }
        oldHealth = eBase.Health;
        if(nowPlayerState!=PlayerStates.Garding && gardObject)Destroy(gardObject.gameObject,0.1f);
    }
    
    public void Move(float input) { // 移動方向/強さ -1~1 として
        if(!lockOperation && !CounterReception && nowPlayerState != PlayerStates.CounterAttack ){
            if(InputValueForMove.x != 0) nowPlayerState = PlayerStates.Runing;
            else nowPlayerState = PlayerStates.Stay;
            if(InputValueForMove.x > 0)direction = 1;
            if(InputValueForMove.x < 0)direction = -1;
            if(onGround) {
                rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x)/0.02f ,0));//f=maの応用
            }else{
                rb2D.AddForce(new Vector2(rb2D.mass * (input * movementSpeed - rb2D.velocity.x)/0.04f ,0));//f=maの応用
            }
        }else{
            if(drawShapeName == "ButtonDown")rb2D.AddForce(new Vector2(rb2D.mass * (input * 0 - rb2D.velocity.x)/0.1f ,0));
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
            rb2D.AddForce(new Vector2(rb2D.velocity.x * rb2D.velocity.x * rb2D.mass,JumpForce * input * 5));
            addingforceInJumping = 0;
        }
        if(jumping)addingforceInJumping += 0.02f;
        else addingforceInJumping = 0;
        if(addingforceInJumping > 0 && addingforceInJumping <= jumpInputArrowableTime){
            rb2D.AddForce(new Vector2(0,JumpForce * input * (-addingforceInJumping *addingforceInJumping + jumpInputArrowableTime)/jumpInputArrowableTime));
        }
    }
    public void OpenInventry(){
        InventryObj.SetActive(true);
        lockOperation = true;
        openingInventry =true;
    }
    public void CloseInventry(){
        InventryObj.SetActive(false);
        lockOperation = false;
        openingInventry =false;
        drawShapeName = "None";
    }
    public IEnumerator onChangeDrawShapeName(){
        if(oldDrawShapeName == "None" && !lockOperation && !(nowPlayerState == PlayerStates.CounterAttack || CounterReception)){
            direction = (drawShapePos.x > 0) ? 1 : -1;
            lockOperation = true;
            switch(drawShapeName){
                case "StraightToRight":{
                    direction = 1;
                    nowPlayerState = PlayerStates.Thrust;
                }break;
                case "StraightToLeft":{
                    direction = -1;
                    nowPlayerState = PlayerStates.Thrust;
                }break;
                case  "StraightToUp":{
                    nowPlayerState = PlayerStates.Up;

                }break;
                case  "StraightToDown":{
                    nowPlayerState = PlayerStates.Down;
                }break;
                case "RegularTriangle": case"InvertedTriangle": case "Thunder":case "Grass":{
                    drawMagicSymbols.Add(new DrawMagicSymbol(drawShapeName,1));
                    drawShapeName = "None";
                    oldDrawShapeName ="None";
                    lockOperation = false;
                }break;
                case "tap":{
                    if(drawMagicSymbols.Count > 0 && magicStones>0){
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
                            ActivisionSpecialMagic();
                        }
                    }else if(eBase.myMagicAttribute != MagicAttribute.none){
                        
                        //WeakBullet
                        nowPlayerState = PlayerStates.ShotMagicBullet;
                        yield return new WaitForSeconds(0.2f);
                        string path = "";
                        switch(eBase.myMagicAttribute){
                            case MagicAttribute.flame:{
                                path = "Magics/WeakMagicBalls/FlameBall";
                            }break;
                            case MagicAttribute.aqua:{
                                path = "Magics/WeakMagicBalls/AquaBall";
                            }break;
                            case MagicAttribute.electro:{
                                path = "Magics/WeakMagicBalls/ElectroBall";
                            }break;
                            case MagicAttribute.terra:{
                                path = "Magics/WeakMagicBalls/TerraBall";
                            }break;
                        }
                        Debug.Log($"direction:{drawShapePos.x}");
                        FireBall bullet = Instantiate((GameObject)Resources.Load(path),transform.position + new Vector3(0.3f * direction,0.3f,0),transform.rotation).GetComponent<FireBall>();
                        bullet.speed *= direction;
                        bullet.gameObject.tag = "Player";
                    }
                    else{     
                        drawShapeName = "None";
                        oldDrawShapeName ="None";
                        lockOperation = false;
                    }
                }break;
                case "Circle":{
                    if(drawMagicSymbols.Count >0){
                        drawMagicSymbols.Add(new DrawMagicSymbol("Circle",1));
                    }else  drawMagicSymbols.Clear();
                    drawShapeName = "None";
                    oldDrawShapeName ="None";
                    lockOperation = false;
                }
                break;
                case "ButtonDown":{
                    if(drawMagicSymbols.Count > 1)if(drawMagicSymbols[drawMagicSymbols.Count -1].magicSymbol == "Circle"){
                        ActivisionSpecialMagic();
                        break;
                    }
                    if(drawMagicSymbols.Count > 0){
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
                        drawMagicSymbols.Clear();
                    }else{
                        //Gard
                        nowPlayerState = PlayerStates.Garding;
                        eBase.gard = true;
                        eBase.ParryReception = true;
                        eBase.acceptDamage = false;
                        gardObject = Instantiate(attackColliders.Gard,transform.position,Quaternion.identity,transform);
                        gardObject.tag = gameObject.tag;
                        gardObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color",MagicColorManager.GetColorFromMagicArticle(eBase.myMagicAttribute));
                        drawMagicSymbols = new List<DrawMagicSymbol>();
                        drawShapeName = "None";
                        oldDrawShapeName ="None";
                        yield return new WaitForSeconds(0.2f);
                        eBase.ParryReception = false;
                        eBase.acceptDamage = true;
                    }

                }break;
                case "ButtonUp":{
                    lockOperation = false;
                    eBase.gard = false;
                }break;
            }
            
        }else {
            if(drawShapeName == "ButtonUp" && !CounterReception && !openingInventry){
                lockOperation = false;
                eBase.gard = false;
                drawShapeName = "None";
            }
        }
    }
    public IEnumerator Parry(int DMG){
        Debug.Log("Parry!!");
        bool counterAttack = false;
        eBase.ParryReception = false;
        CounterReception = true;
        Time.timeScale = 0.1f;
        for(float t = 0;t < 1.0f;t += Time.deltaTime){
            if(drawShapeName != "None" && drawShapeName != "ButtonDown" && drawShapeName != "ButtonUp"){
                counterAttack = true;
                nowPlayerState = PlayerStates.CounterAttack;
                Debug.Log("Counter!!");
                break;
            }
            yield return null;
        }
        CounterReception = false;
        Time.timeScale = 1f;
        if(counterAttack){
            //counter
            CounterDMG = DMG;
        }else{
            nowPlayerState = PlayerStates.Stay;
            lockOperation = false;
            eBase.acceptDamage = true;
        }
        yield break;
    }
    public void UnLockOperation(){
        drawShapeName = "None";
        drawMagicSymbols = new List<DrawMagicSymbol>();
        lockOperation = false;
        Debug.Log("UnLocked operation");
        Time.timeScale =1f;
        nowPlayerState = PlayerStates.Stay;
        eBase.acceptDamage = true;
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

    public void SlowMotionStart(float mulutiply){
        Time.timeScale = mulutiply;
        Debug.Log("Slow!");
    }
    public void SlowMotionEnd(){Time.timeScale = 1;}
    public void GenerateAttackCollier(){StartCoroutine(IEGenerateAttackCollier());}
    private void ActivisionSpecialMagic(){//SpecialMagic
        nowPlayerState = PlayerStates.ActivateSpecialMagic;
        PlayerMagicFactory playerMagicFactory = new PlayerMagicFactory();
        switch(drawMagicSymbols[0].magicSymbol){
            case "RegularTriangle":{
                if(magicHolder.flameMagic != PlayerMagicFactory.MagicKind.none){
                    PlayerMagicBase plMagicBase = playerMagicFactory.Create(magicHolder.flameMagic);
                    StartCoroutine(plMagicBase.ActivationFlameMagic(this));
                }
            }break;
            case "InvertedTriangle":{
                if(magicHolder.aquaMagic != PlayerMagicFactory.MagicKind.none){
                    PlayerMagicBase plMagicBase = playerMagicFactory.Create(magicHolder.aquaMagic);
                    StartCoroutine(plMagicBase.ActivationAquaMagic(this));
                }
            }break;
            case "Thunder":{
                if(magicHolder.electroMagic != PlayerMagicFactory.MagicKind.none){
                    PlayerMagicBase plMagicBase = playerMagicFactory.Create(magicHolder.electroMagic);
                    StartCoroutine(plMagicBase.ActivationElectroMagic(this));
                }
            }break;
            case "Grass":{
                if(magicHolder.terraMagic != PlayerMagicFactory.MagicKind.none){
                    PlayerMagicBase plMagicBase = playerMagicFactory.Create(magicHolder.terraMagic);
                    StartCoroutine(plMagicBase.ActivationTerraMagic(this));
                }
            }break;
        }
        magicStones --;
        lockOperation = false;
        
    }
    private void EnchantMyself(MagicAttribute magicAttribute){
    }
    private IEnumerator IEGenerateAttackCollier(){
        switch(nowPlayerState){
            case PlayerStates.Up:{
                if(drawShapePos.x > 0) direction =1;
                else direction = -1;

                GameObject DMGObject = Instantiate(attackColliders.Up,new Vector2(transform.position.x + 0.75f * direction,transform.position.y),transform.rotation);
                if(direction < 0){
                    DMGObject.transform.GetChild(0).eulerAngles = new Vector3(30,180,0);
                    DMGObject.transform.GetChild(0).localPosition = Vector3.Scale(new Vector3(-1,1,1),DMGObject.transform.GetChild(0).localPosition);
                }
                DMGObject.GetComponent<BoxCollider2D>().offset *= new Vector2(direction,1);

                AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                attackBase.damage *= 1;
                DMGObject.tag = "Player";
                attackBase.knockBack = new Vector2(attackBase.knockBack.x * direction,attackBase.knockBack.y);
                attackBase.magicAttribute = eBase.myMagicAttribute;
                drawMagicSymbols = new List<DrawMagicSymbol>();
                Destroy(DMGObject.GetComponent<AttackBase>(),0.2f);
                Destroy(DMGObject,1);
                for(int i = 0;i < 10;i++){
                    transform.Translate(upForwardDistance*0.1f*direction,0,0);
                    yield return new WaitForEndOfFrame();
                }
                if(DMGObject.GetComponentInChildren<SpriteRenderer>())DMGObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",EffectColor(eBase.myMagicAttribute));

            }break;
            case PlayerStates.Thrust:{
                if(direction > 0){
                    GameObject DMGObject = Instantiate(attackColliders.Thrust,transform.position,transform.rotation);
                    DMGObject.transform.GetChild(0).eulerAngles = new Vector3(0,0,0);
                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(attackBase.knockBack.x,attackBase.knockBack.y);
                    Destroy(DMGObject.GetComponent<AttackBase>(),0.2f);
                    Destroy(DMGObject,1);
                    for(int i = 0;i < 5;i++){
                        transform.Translate(thrustForwardDistance*0.2f,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }else{
                    GameObject DMGObject = Instantiate(attackColliders.Thrust,transform.position,transform.rotation);
                    DMGObject.transform.GetChild(0).eulerAngles = new Vector3(0,180,0);
                    DMGObject.GetComponent<BoxCollider2D>().offset *= new Vector2(-1,1);
                    DMGObject.transform.GetChild(0).localPosition = Vector3.Scale(new Vector3(-1,1,1),DMGObject.transform.GetChild(0).localPosition);

                    AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                    attackBase.damage *= 1;
                    DMGObject.tag = "Player";
                    attackBase.knockBack = new Vector2(-attackBase.knockBack.x,attackBase.knockBack.y);
                    attackBase.magicAttribute = eBase.myMagicAttribute;
                    drawMagicSymbols = new List<DrawMagicSymbol>();
                    Destroy(DMGObject.GetComponent<AttackBase>(),0.2f);
                    Destroy(DMGObject,1);
                    for(int i = 0;i < 5;i++){
                        transform.Translate(-thrustForwardDistance*0.2f,0,0);
                        yield return new WaitForEndOfFrame();
                    }
                }

            }break;
            case PlayerStates.Down:{
                    
                if(drawShapePos.x > 0) direction =1;
                else direction = -1;

                GameObject DMGObject = Instantiate(attackColliders.Down,new Vector2(transform.position.x + 0.75f * direction,transform.position.y),Quaternion.identity);
                if(direction < 0){
                    DMGObject.transform.GetChild(0).eulerAngles = new Vector3(0,180,0);
                    DMGObject.transform.GetChild(0).localPosition = Vector3.Scale(new Vector3(-1,1,1),DMGObject.transform.GetChild(0).localPosition);
                }
                DMGObject.GetComponent<BoxCollider2D>().offset *= new Vector2(direction,1);

                AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                attackBase.damage *= 1;
                DMGObject.tag = "Player";
                attackBase.knockBack = new Vector2(attackBase.knockBack.x * direction,attackBase.knockBack.y);
                attackBase.magicAttribute = eBase.myMagicAttribute;
                drawMagicSymbols = new List<DrawMagicSymbol>();
                Destroy(DMGObject.GetComponent<AttackBase>(),0.2f);
                Destroy(DMGObject,1);
                for(int i = 0;i < 10;i++){
                    transform.Translate(downForwardDistance*0.1f*direction,0,0);
                    yield return new WaitForEndOfFrame();
                }
                if(DMGObject.GetComponentInChildren<SpriteRenderer>())DMGObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",EffectColor(eBase.myMagicAttribute));
            }break;
            case PlayerStates.CounterAttack:{
                if(drawShapePos.x > 0) direction =1;
                else direction = -1;
                GameObject DMGObject = Instantiate(attackColliders.counterAttack,new Vector2(transform.position.x + 0.75f * direction,transform.position.y),Quaternion.identity);
                if(direction < 0){
                    DMGObject.transform.GetChild(0).eulerAngles = new Vector3(-120,180,0);
                    DMGObject.transform.GetChild(0).localPosition = Vector3.Scale(new Vector3(-1,1,1),DMGObject.transform.GetChild(0).localPosition);
                }
                DMGObject.GetComponent<BoxCollider2D>().offset *= new Vector2(direction,1);

                AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
                attackBase.damage = CounterDMG *2;
                DMGObject.tag = "Player";
                attackBase.knockBack = new Vector2(attackBase.knockBack.x * direction,attackBase.knockBack.y);
                attackBase.magicAttribute = eBase.myMagicAttribute;
                drawMagicSymbols = new List<DrawMagicSymbol>();
                Destroy(DMGObject.GetComponent<AttackBase>(),0.2f);
                Destroy(DMGObject,1);
                for(int i = 0;i < 10;i++){
                    transform.Translate(counterAttackForwardDistance*0.1f*direction,0,0);
                    yield return new WaitForEndOfFrame();
                }
                if(DMGObject.GetComponentInChildren<SpriteRenderer>())DMGObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",EffectColor(eBase.myMagicAttribute));
            }break;
        }
        yield break;
    }
    public int DirectionFromDrawShapePos(){
        return (drawShapePos.x > 0) ? 1 : -1;
    }
}