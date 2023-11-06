using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreUIController : MonoBehaviour
{
     public EntityBase targetEntityBase;
    private float oldMagicStone;
    private PlayerController plc;
    public Image core;
    public Animator shell;
    public Image symbol;
    [System.Serializable]
    public class CoreColors{
        public Color flame;
        public Color aqua;
        public Color electro;
        public Color terra;
    }
    public CoreColors coreColors = new CoreColors();
    [System.Serializable]
    public class CoreSymbols{
        public Sprite flame;
        public Sprite aqua;
        public Sprite electro;
        public Sprite terra;
    }
    public CoreSymbols coreSymbols = new CoreSymbols();
    void Start(){
        plc = targetEntityBase.gameObject.GetComponent<PlayerController>();
    }
    void Update(){
        switch(targetEntityBase.myMagicAttribute){
            case(MagicAttribute.Flame):{
                core.color = coreColors.flame;
                symbol.sprite = coreSymbols.flame;
            }break;
            case(MagicAttribute.Aqua):{
                core.color = coreColors.aqua;
                symbol.sprite = coreSymbols.aqua;
            }break;
            case(MagicAttribute.Electro):{
                core.color = coreColors.electro;
                symbol.sprite = coreSymbols.electro;
            }break;
            case(MagicAttribute.Terra):{
                core.color = coreColors.terra;
                symbol.sprite = coreSymbols.terra;
            }break;
            case(MagicAttribute.none):{
                core.color = Color.white;
                symbol.sprite = null;
            }break;
        }
        symbol.enabled = (targetEntityBase.myMagicAttribute != MagicAttribute.none);

        if(oldMagicStone !=plc.magicStones){
            shell.Play("main",0,(4-plc.magicStones)/4);
        }
        oldMagicStone = plc.magicStones;
    } 
}
