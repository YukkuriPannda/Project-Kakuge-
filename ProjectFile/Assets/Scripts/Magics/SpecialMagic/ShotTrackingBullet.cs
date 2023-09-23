using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTrackingBullet : PlayerMagicBase
{
    public override PlayerMagicFactory.MagicKind SkillKind
    { 
        get {return PlayerMagicFactory.MagicKind.ShotTrackingBullet;}
    }
    public override IEnumerator ActivationFlameMagic(PlayerController plc)
    {
        GameObject module = GameObject.Instantiate((GameObject)Resources.Load("Magics/TrackingBullet/TrackingBulletModule"),plc.transform.position + new Vector3(0.45f,0.45f,0),Quaternion.identity);
        module.tag = "Player";
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
