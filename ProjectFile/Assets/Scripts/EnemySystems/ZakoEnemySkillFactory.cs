using System;
using System.Collections;
using System.Linq;

/// <summary>
/// Skillを管理し、作ってくれるFactoryクラス
/// </summary>
public class ZakoEnemySkillFactory 
{
    // スキル一覧
    static readonly ZakoEnemySkillBase[] skills = {
        new KnockDown()
    };

    /// スキルのenum
    public enum ZakoEnemySkillKind
    {
        KnockDown
    }

    // SkillKindを引数に、それに応じたスキルを返す
    public ZakoEnemySkillBase Create(ZakoEnemySkillKind skillKind) 
    {
        return skills.SingleOrDefault(skill => skill.SkillKind == skillKind);
    }

}
abstract public class ZakoEnemySkillBase{
    public abstract ZakoEnemySkillFactory.ZakoEnemySkillKind SkillKind { get; }
    public abstract IEnumerator Attack(ZakoEnemyController zec);
}