using System.Collections;
using UnityEngine;
/// <summary>
/// スキル「ライトニング」の具象クラス
/// </summary>
public class ShotMissile : ZakoEnemySkillBase
{
    // スキル種別
    public override ZakoEnemySkillFactory.ZakoEnemySkillKind SkillKind
    { 
        get {return ZakoEnemySkillFactory.ZakoEnemySkillKind.ShotMissile;}
    }
    public override IEnumerator Attack(ZakoEnemyController zec)
    {
        yield return new WaitForSeconds(1.1f); 
        
        GameObject missile 
        = GameObject.Instantiate(Resources.Load<GameObject>("EnemysPrefabs/NormalMissile")
            ,zec.transform.position+new Vector3((zec.direction == 1) ? 1.4f:-0.5f,0.4f,0)
            ,Quaternion.Euler(0,0,(zec.direction == 1) ? 30 : 150));
        missile.gameObject.tag = "Enemy";
        GameObject.Destroy(missile,5f);
        yield break;
    }
}