using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicColorManager : MonoBehaviour
{
    [ColorUsage(false, true)]public Color inputFlame;
    [ColorUsage(false, true)]public Color inputAqua;
    [ColorUsage(false, true)]public Color inputElectro;
    [ColorUsage(false, true)]public Color inputTerra;
    static public Color flame;
    static public Color aqua;
    static public Color electro;
    static public Color terra;
    void Awake()
    {
        flame = inputFlame;
        aqua = inputAqua;
        electro = inputElectro;
        terra = inputTerra;
    }
    public static Color GetColorFromMagicArticle(MagicAttribute magicAttribute){
        Color res;
        switch(magicAttribute){
            case MagicAttribute.Flame:
                res=flame;
            break;
            case MagicAttribute.Aqua:
                res = aqua;
            break;
            case MagicAttribute.Electro:
                res = electro;
            break;
            case MagicAttribute.Terra:
                res = terra;
            break;
            default:
                res = Color.white;
            break;
        }
        return res;
    }
}
public enum MagicAttribute{
    none,
    Flame,
    Aqua,
    Electro,
    Terra
}