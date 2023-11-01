using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Type43Contoller : MonoBehaviour
{
    public GameObject target;
    public GameObject gearBit;
    public float detecitrRadius= 5.0f;
    public float boostSpeed;
    [System.Serializable]
    public class AttackCollider{
        public GameObject Collider;
        public Vector2 offset; 
    }
    public AttackCollider tuzigiriColli;
    public AttackCollider upSlashColli;

    public enum States{
        Stopping,
        StartUping,
        Waiting,
        Upslash,
        BeamCombo,
        VolleyBeam,
        TUZIGIRI,
        Deathing,
        Damaging,
        OverHeating
    }
    public States nowState;
    private States oldState;
    public int direction;
    private Animator animator;
    private Rigidbody2D rb2D;
    private EntityBase eBase;
    float oldHealth = 0;
    public List<GameObject> beams = new List<GameObject>();
    void Start()
    {
        nowState = States.Stopping;
        animator = gameObject.GetComponent<Animator>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        eBase = gameObject.GetComponent<EntityBase>();
    }

    void Update()
    {
        direction = (target.transform.position.x - transform.position.x > 0)?1:-1;
        if(oldHealth - eBase.Health > 0){
            StopAllCoroutines();
            if(eBase.Health <= 0)nowState = States.Deathing;
            else{
                nowState = States.Damaging;
                animator.Play("Hurt_" + ((direction == 1)?  "R":"L"),0,0);
                foreach(GameObject beam in beams){
                    beam.GetComponent<GearBitController>().nowState = GearBitController.States.Returning;
                }
            } 
                
        }
        switch(nowState){
            case States.Stopping :{
                if(Mathf.Abs(target.transform.position.x - transform.position.x) < detecitrRadius){
                    nowState = States.StartUping;
                    animator.Play("StartUp_" + ((direction ==1)?  "R":"L"),0,0);
                }
            }break;
        }
        oldHealth = eBase.Health;
        oldState = nowState;
    }
    public void EndAction(){
        Debug.Log($"End Action{nowState}");
        switch(nowState){
            case States.StartUping:{
                StartCoroutine(BeamCombo());
            }break;
            case States.BeamCombo:case States.Damaging:
            case States.VolleyBeam:case States.TUZIGIRI:
            case States.Upslash:{
                StartCoroutine(RelotteryBehavior());
            }break;
        }
    }
    IEnumerator RelotteryBehavior(){
        nowState = States.Waiting;
        int ram = Random.Range(0,3);
        while(beams.Count > 0)yield return null;
        Debug.Log($"Behavior is {ram} count is {beams.Count}");
        if(Mathf.Abs(target.transform.position.x - transform.position.x) < 2 && Random.value > 0.5f/*50%の確率*/){
            StartCoroutine(Upslash());
            yield break;
        }
        yield return new WaitForSeconds(Random.Range(0,3));
        switch(ram){
            case 0:{
                StartCoroutine(BeamCombo());
            }break;
            case 1:{
                StartCoroutine(VolleyBeam());
            }break;
            case 2:{
                StartCoroutine(TUZIGIRI());
            }break;
        }
        yield break;
    }
    IEnumerator BeamCombo(){
        nowState = States.BeamCombo;
        StartCoroutine(Shot4Beams());
        yield break;
    }
    IEnumerator Upslash(){
        animator.Play("Upslash_" + ((direction == 1)?  "R":"L"),0,0);
        nowState = States.Upslash;
        Vector3 pos = transform.position + new Vector3(upSlashColli.offset.x * direction,upSlashColli.offset.y,0);
        Destroy(Instantiate(upSlashColli.Collider,pos,Quaternion.identity),0.1f);
        yield break;
    }
    IEnumerator Shot4Beams(){
        animator.Play("BoostStart_" + ((direction == 1)?  "R":"L"),0,0);
        for(int i  = 0;i < 4;){
            for(int j = 0;j < 2;j++){
                beams.Add(Instantiate(gearBit,transform.position,Quaternion.identity));
                beams[i].GetComponent<GearBitController>().target = target;
                beams[i].GetComponent<GearBitController>().attackMode = GearBitController.AttackMode.Follow;
                beams[i].GetComponent<GearBitController>().returnObj = gameObject;
                beams[i].GetComponent<GearBitController>().distance = j + 1;
                beams[i].GetComponent<GearBitController>().height = (i+1)/3*1.5f+1;
                beams[i].GetComponent<GearBitController>().beamsIndex = i;
                i++;
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.3f);
            beams[i-2].GetComponent<GearBitController>().nowState = GearBitController.States.Following;
            beams[i-1].GetComponent<GearBitController>().nowState = GearBitController.States.Following;
        }

        bool allBitTargLock = false;
        while(allBitTargLock){
            foreach(GameObject beam in beams){
                if(beam.GetComponent<GearBitController>().nowState == GearBitController.States.TargetLock)allBitTargLock  = true;
                else{
                    allBitTargLock = false;
                    break;
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        beams[0].GetComponent<GearBitController>().ShotBeam();
        beams[1].GetComponent<GearBitController>().ShotBeam();

        yield return new WaitForSeconds(1f);
        
        beams[2].GetComponent<GearBitController>().ShotBeam();
        beams[3].GetComponent<GearBitController>().ShotBeam();
        StartCoroutine(TUZIGIRI());
        yield return new WaitForSeconds(2f);
        foreach(GameObject beam in beams){
            beam.GetComponent<GearBitController>().nowState = GearBitController.States.Returning;
        }
        yield break;
    }
    IEnumerator TUZIGIRI(){
        if(nowState != States.BeamCombo)nowState = States.TUZIGIRI;
        yield return new WaitForSeconds(0.6f);
        float t = Time.deltaTime;
        while(target.transform.position.x - transform.position.x > 0.5f && t < 1.5f){
            int StartDir = direction;
            if(StartDir == direction)rb2D.AddForce(new Vector2(rb2D.mass * (StartDir * boostSpeed - rb2D.velocity.x)/0.05f , 0));
            else yield break;
            yield return null;
        }
        rb2D.velocity = Vector2.zero;
        animator.Play("BoostSlash_" + ((direction == 1)?  "R":"L"),0,0);
        yield return new WaitForSeconds(0.2f);
        Destroy(Instantiate(tuzigiriColli.Collider,transform.position + (Vector3)tuzigiriColli.offset,Quaternion.identity),0.1f);
        transform.position = target.transform.position + new Vector3(2f*direction,0);
        yield return new WaitForSeconds(2f);
        yield break;
    }
    IEnumerator VolleyBeam(){
        nowState = States.VolleyBeam;
        animator.Play("VolleyStartUp_" + ((direction == 1)?  "R":"L"),0,0);
        for(int i  = 0;i < 4; i++){
            beams.Add(Instantiate(gearBit,transform.position,Quaternion.Euler(0,0,direction == 1?0:180)));
            beams[i].GetComponent<GearBitController>().target = target;
            beams[i].GetComponent<GearBitController>().attackMode = GearBitController.AttackMode.Volley;
            beams[i].GetComponent<GearBitController>().rotate = 90 * i;
            beams[i].GetComponent<GearBitController>().returnObj = gameObject;
            beams[i].GetComponent<GearBitController>().ShotPos = transform.position + new Vector3(0.67f*direction,0.05f);
            beams[i].GetComponent<GearBitController>().beamsIndex = i;
            yield return new WaitForSeconds(0.25f);
            beams[i].GetComponent<GearBitController>().nowState = GearBitController.States.Following;
        }

        bool allBitTargLock = false;
        while(allBitTargLock){
            foreach(GameObject beam in beams){
                if(beam.GetComponent<GearBitController>().nowState == GearBitController.States.TargetLock)allBitTargLock  = true;
                else{
                    allBitTargLock = false;
                    break;
                }
            }
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        animator.Play("Volley_" + ((direction == 1)?  "R":"L"),0,0);
        beams[0].GetComponent<GearBitController>().ShootBeamLong();
        beams[1].GetComponent<GearBitController>().ShootBeamLong();
        beams[2].GetComponent<GearBitController>().ShootBeamLong();
        beams[3].GetComponent<GearBitController>().ShootBeamLong();

        yield return new WaitForSeconds(2f);
        
        foreach(GameObject beam in beams){
            beam.GetComponent<GearBitController>().nowState = GearBitController.States.Returning;
        }
        yield break;
    }
}
