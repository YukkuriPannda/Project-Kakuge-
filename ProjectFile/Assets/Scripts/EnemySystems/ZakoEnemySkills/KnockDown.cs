using System.Collections;
using UnityEngine;
/// <summary>
/// スキル「ライトニング」の具象クラス
/// </summary>
public class KnockDown : ZakoEnemySkillBase
{
    // スキル種別
    public override ZakoEnemySkillFactory.ZakoEnemySkillKind SkillKind
    { 
        get {return ZakoEnemySkillFactory.ZakoEnemySkillKind.KnockDown;}
    }
    public override IEnumerator Attack(ZakoEnemyController zec)
    {
        yield return new WaitForSeconds(0.5f); 
        GameObject DMGObject = GameObject.Instantiate(zec.attackCollider,new Vector2(zec.transform.position.x + 0.5f * zec.direction,zec.transform.position.y),zec.transform.rotation);
        AttackBase attackBase = DMGObject.GetComponent<AttackBase>();
        attackBase.damage *= 1;
        DMGObject.tag = "Enemy";
        attackBase.knockBack = new Vector2(attackBase.knockBack.x * zec.direction,attackBase.knockBack.y);
        GameObject.Destroy(DMGObject,0.2f);
        yield return null;
    }
}