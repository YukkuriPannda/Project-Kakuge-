using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTrackingBullet : PlayerMagicBase
{
    public override PlayerMagicFactory.PlayerFlameMagicKind SkillKind
    { 
        get {return PlayerMagicFactory.PlayerFlameMagicKind.ShotTrackingBullet;}
    }
    public override IEnumerator ActivationFlameMagic(PlayerController plc)
    {
        int direction = plc.direction;
        for(int i = 0;i< 5;i++){
            AttackBase bullet = GameObject.Instantiate((GameObject)Resources.Load("Magics/TrackingBullet/FlameTrackingBulletPrefab"),plc.transform.position + new Vector3(1f * direction,0.3f,0),Quaternion.Euler(0,0,Random.Range(30,90)))
                .GetComponent<AttackBase>();
            bullet.gameObject.tag = "Player";
            if(direction == -1){
                bullet.gameObject.transform.eulerAngles = new Vector3(0,180,0);
                bullet.knockBack *= new Vector2(-1,1);
            }
            GameObject.Destroy(bullet.gameObject,2.7f);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
    public override IEnumerator ActivationAquaMagic(PlayerController plc)
    {
        
        yield break;
    }
    public override IEnumerator ActivationElectroMagic(PlayerController plc)
    {
        
        yield break;
    }
    public override IEnumerator ActivationTerraMagic(PlayerController plc)
    {
        
        yield break;
    }
}
