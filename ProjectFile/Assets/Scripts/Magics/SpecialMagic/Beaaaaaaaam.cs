using System.Collections;
using UnityEngine;
/// <summary>
/// スキル「ライトニング」の具象クラス
/// </summary>
public class Beaaaaaaaam: PlayerMagicBase
{
    // スキル種別
    public override PlayerMagicFactory.PlayerFlameMagicKind SkillKind
    { 
        get {return PlayerMagicFactory.PlayerFlameMagicKind.Beaaaaaaaaaaam;}
    }
    public override IEnumerator ActivationFlameMagic(PlayerController plc)
    {
        int direction = plc.direction;
        Debug.Log(plc.gameObject.GetComponent<VisualControler>().playerAnimator.GetInteger("Orientation") + "," + direction);
        AttackBase beaaaam = GameObject.Instantiate((GameObject)Resources.Load("Magics/BeaaaaaamPrefab"),plc.transform.position + new Vector3(1f * direction,0.3f,0),plc.transform.rotation)
            .GetComponent<AttackBase>();
        beaaaam.tag = "Player";
        if(direction == -1)beaaaam.gameObject.transform.eulerAngles = new Vector3(0,180,0);
        yield return new WaitForSeconds(2.7f);
        GameObject.Destroy(beaaaam.gameObject);
        yield break;
    }
}