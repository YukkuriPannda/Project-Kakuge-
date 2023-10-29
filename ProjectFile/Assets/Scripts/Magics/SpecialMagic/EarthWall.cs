using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class EarthWall: PlayerMagicBase
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
    int layermask = 1 << LayerMask.NameToLayer("Ground");
    Vector3 playerPosition = plc.transform.position;
    RaycastHit2D hit;
    RaycastHit2D hitObject = Physics2D.CircleCast(plc.drawShapePos + (Vector2)plc.transform.position,0.3f,Vector2.down,Mathf.Infinity,layermask);
    if (hitObject.collider)
    {
        Vector3 targetPosition = hitObject.point;
        
        if (Mathf.Abs(plc.drawShapePos.x) <= 3.0f)
        { 
            Debug.Log ("Earth");
            Debug.Log(hitObject.point);
            Vector3 wallSpawnPosition = hitObject.point;
            GameObject.Destroy(GameObject.Instantiate((GameObject)Resources.Load("Magics/EarthWallPrefab"),wallSpawnPosition + new Vector3(0,0.7f), plc.transform.rotation), 10f);
        }
        else
        {
            Debug.Log("out");
            Vector3 directionoTarget = (targetPosition - playerPosition).normalized;
            Vector3 wallSpawnPosition = directionoTarget * 3.0f;
            GameObject.Destroy(GameObject.Instantiate((GameObject)Resources.Load("Magics/EarthWallPrefab"),wallSpawnPosition + new Vector3(0,0.7f), plc.transform.rotation), 10f);
        }
    }
    yield break;
}

}
