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
            AttackBase bullet;
            if(direction < 0)bullet = GameObject.Instantiate((GameObject)Resources.Load("Magics/TrackingBullet/FlameTrackingBulletPrefab"),plc.transform.position + new Vector3(-1f * direction,0.3f,0),Quaternion.Euler(0,0,Random.Range(30,120)))
                .GetComponent<AttackBase>();
            else bullet = GameObject.Instantiate((GameObject)Resources.Load("Magics/TrackingBullet/FlameTrackingBulletPrefab"),plc.transform.position + new Vector3(-1f * direction,0.3f,0),Quaternion.Euler(0,0,Random.Range(120,210)))
                .GetComponent<AttackBase>();
            bullet.gameObject.tag = "Player";
            bullet.gameObject.GetComponent<TrackingBullet>().speed *= Random.Range(0.5f,2f);
            bullet.gameObject.GetComponent<TrackingBullet>().rotateSpeed *= Random.Range(0.5f,3f);
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
