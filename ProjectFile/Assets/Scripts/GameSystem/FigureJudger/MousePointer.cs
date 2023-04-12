using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MousePointer : MonoBehaviour {
    Transform mytrf;
    public EntityBase targetEntityBase;
    private float oldMagicStone;
    private VisualControler visualctl;
    private PlayerController plc;
    public SpriteRenderer core;
    public Animator shell;
    public SpriteRenderer symbol;
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
        mytrf = transform;

        visualctl = targetEntityBase.gameObject.GetComponent<VisualControler>();
        plc = targetEntityBase.gameObject.GetComponent<PlayerController>();
    }
    void Update(){
        mytrf.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        switch(targetEntityBase.myMagicAttribute){
            case(MagicAttribute.flame):{
                core.color = coreColors.flame;
                symbol.sprite = coreSymbols.flame;
            }break;
            case(MagicAttribute.aqua):{
                core.color = coreColors.aqua;
                symbol.sprite = coreSymbols.aqua;
            }break;
            case(MagicAttribute.electro):{
                core.color = coreColors.electro;
                symbol.sprite = coreSymbols.electro;
            }break;
            case(MagicAttribute.terra):{
                core.color = coreColors.terra;
                symbol.sprite = coreSymbols.terra;
            }break;
            case(MagicAttribute.none):{
                core.color = Color.white;
                symbol.sprite = null;
            }break;
        }
        if(oldMagicStone !=plc.magicStones){
            shell.Play("main",0,(6-plc.magicStones)/7);
        }
        oldMagicStone = plc.magicStones;
    }   
}
