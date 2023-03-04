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

    // スキル「ライトニング」の実行
    public override IEnumerator Attack(ZakoEnemyController zec)
    {
        Debug.Log("KnockDown!");
        yield return null;
    }
}