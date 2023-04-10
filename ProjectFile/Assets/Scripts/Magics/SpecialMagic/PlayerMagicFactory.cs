using System;
using System.Collections;
using System.Linq;
public class PlayerMagicFactory
{
    // スキル一覧
    static readonly PlayerMagicBase[] skills = {
        new Beaaaaaaaam()
    };

    /// スキルのenum
    public enum PlayerFlameMagicKind
    {
        Beaaaaaaaaaaam
    }

    // SkillKindを引数に、それに応じたスキルを返す
    public PlayerMagicBase Create(PlayerFlameMagicKind skillKind) 
    {
        return skills.SingleOrDefault(skill => skill.SkillKind == skillKind);
    }
}
abstract public class PlayerMagicBase{
    public abstract PlayerMagicFactory.PlayerFlameMagicKind SkillKind { get; }
    public abstract IEnumerator ActivationFlameMagic(PlayerController plc);
    public abstract IEnumerator ActivationAquaMagic(PlayerController plc);
    public abstract IEnumerator ActivationElectroMagic(PlayerController plc);
    public abstract IEnumerator ActivationTerraMagic(PlayerController plc);
}
