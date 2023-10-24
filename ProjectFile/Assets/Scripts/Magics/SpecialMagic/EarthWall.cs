using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : PlayerMagicBase
{
    // スキル種別
    public override PlayerMagicFactory.MagicKind SkillKind
    { 
        get {return PlayerMagicFactory.MagicKind.EarthWall;}
    }
    public override IEnumerator ActivationFlameMagic(PlayerController plc)
    {   
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

        GameObject.Destroy(GameObject.Instantiate((GameObject)Resources.Load("Magics/EarthWallPrefab"),plc.transform.position + new Vector3(0,0.3f,0),plc.transform.rotation), 10f);
        
        yield break;
    }

    
}
