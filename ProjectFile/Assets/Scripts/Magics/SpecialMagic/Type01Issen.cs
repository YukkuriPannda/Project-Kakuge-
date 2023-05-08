using System.Collections;
using UnityEngine;
public class Type01Senkou: PlayerMagicBase
{
    // スキル種別
    public override PlayerMagicFactory.PlayerFlameMagicKind SkillKind
    { 
        get {return PlayerMagicFactory.PlayerFlameMagicKind.Type01Senkou;}
    }
    public override IEnumerator ActivationFlameMagic(PlayerController plc)
    {
        yield break;
    }
    public override IEnumerator ActivationAquaMagic(PlayerController plc)
    {
        Debug.LogError("This Magic cannot be activated");
        yield break;
    }
    public override IEnumerator ActivationElectroMagic(PlayerController plc)
    {
        Vector2 ToPos = plc.drawShapePos;
        AttackBase unit = GameObject.Instantiate((GameObject)Resources.Load("Magics/Type01Senkou/ThunderUnit"),plc.transform.position,Quaternion.identity)
            .GetComponent<AttackBase>();
        GameObject Katana = GameObject.Instantiate((GameObject)Resources.Load("Magics/Type01Senkou/ThunderUnit"),plc.transform.position,Quaternion.identity);
        yield break;
    }
    public override IEnumerator ActivationTerraMagic(PlayerController plc)
    {
        yield break;
    }
}