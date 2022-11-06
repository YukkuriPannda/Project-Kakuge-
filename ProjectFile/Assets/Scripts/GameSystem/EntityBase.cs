using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    //HP管理や被攻撃処理をする。HPを持つオブジェクト全部にアタッチ
    [SerializeField] ContactFilter2D filter2d;

    #region [Public Parameters]
    public float   Health = 65535;
    public MagicAttribute myMagicAttribute = MagicAttribute.none;
    public bool gard = false;
    #endregion

    public bool     Invulnerable = false;
    public bool     NoGravity = false;
    #region [Private Parameters]
    public bool     OnGround = false;
    //private List<effect> Effects = new List<effect>();
    #endregion
    public void Hurt(float DMG,MagicAttribute magicAttribute = MagicAttribute.none){
        switch(magicAttribute){
            case MagicAttribute m when(
                   (m == MagicAttribute.aqua && myMagicAttribute == MagicAttribute.flame)
                || (m == MagicAttribute.flame && myMagicAttribute == MagicAttribute.terra)
                || (m == MagicAttribute.terra && myMagicAttribute == MagicAttribute.electro)
                || (m == MagicAttribute.electro && myMagicAttribute == MagicAttribute.aqua)
            ):
                if(gard)Health -= DMG;
                else Health-= DMG*2f;
            break;
            
            case MagicAttribute m when(
                   (m == MagicAttribute.flame && myMagicAttribute == MagicAttribute.aqua)
                || (m == MagicAttribute.terra && myMagicAttribute == MagicAttribute.flame)
                || (m == MagicAttribute.electro && myMagicAttribute == MagicAttribute.terra)
                || (m == MagicAttribute.aqua && myMagicAttribute == MagicAttribute.electro)
            ):
                if(!gard)Health -= DMG*0.5f;
            break;
            default:
                if(gard)Health -= DMG*0.5f;
                else Health -= DMG;
            break;
        }
    }
}
public enum MagicAttribute{
    none,
    flame,
    aqua,
    electro,
    terra
}