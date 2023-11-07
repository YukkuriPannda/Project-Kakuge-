using System.Collections;
using UnityEngine;
/// <summary>
/// スキル「ライトニング」の具象クラス
/// </summary>
public class Rush : ZakoEnemySkillBase
{
    // スキル種別
    public override ZakoEnemySkillFactory.ZakoEnemySkillKind SkillKind
    { 
        get {return ZakoEnemySkillFactory.ZakoEnemySkillKind.Rush;}
    }
    public override IEnumerator Attack(ZakoEnemyController zec)
    {
        yield return new WaitForSeconds(0.2f);
        GameObject attackColli 
        = GameObject.Instantiate(Resources.Load<GameObject>("EnemysPrefabs/Rush")
            ,zec.transform.position+new Vector3((zec.direction == 1) ? 0.4f:-0.4f,0.4f,0)
            ,Quaternion.identity);
        attackColli.gameObject.tag = "Enemy";
        attackColli.gameObject.GetComponent<AttackBase>().knockBack *= new Vector2((zec.direction == 1) ? 1:-1,1);
        GameObject.Destroy(attackColli,0.1f);
        zec.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(100,10) * ((zec.direction == 1) ?-1:1));
        yield break;
    }
}