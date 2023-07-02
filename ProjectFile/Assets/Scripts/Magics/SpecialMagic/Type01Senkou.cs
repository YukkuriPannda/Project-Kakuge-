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
        Vector3 ToPos = plc.drawShapePos;
        PlayerVisualController plvcl = plc.gameObject.GetComponent<PlayerVisualController>();
        float angle = Vector2.SignedAngle(new Vector2(1,0),ToPos);
        AttackBase unit = GameObject.Instantiate((GameObject)Resources.Load("Magics/Type01Senkou/ThunderUnit"),plc.transform.position,Quaternion.Euler(0,0,angle))
            .GetComponent<AttackBase>();
        BoxCollider2D thunderCollider = unit.gameObject.GetComponent<BoxCollider2D>();
        unit.gameObject.tag = "Player";
        Transform leftHandTrf = plvcl.leftHand;
        GameObject Katana = GameObject.Instantiate((GameObject)Resources.Load("Magics/Type01Senkou/KATANA"),leftHandTrf);
        GameObject.Destroy(unit.gameObject,3f);
        GameObject.Destroy(Katana,3f);
        plc.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.05f;
        yield return new WaitForSeconds(1.1f);
        plvcl.model.localScale = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        plc.transform.position = ToPos + plc.transform.position;
        plvcl.model.localScale = Vector3.one;
        yield return new WaitForSeconds(1.1f);
        thunderCollider.enabled = true;
        GameObject.Destroy(thunderCollider,0.25f);
        yield return new WaitForSeconds(0.5f);
        plc.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        yield break;
    }
    public override IEnumerator ActivationTerraMagic(PlayerController plc)
    {
        yield break;
    }
}