using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    public PlayerController plc;
    public Animator plAnim;
    public Transform model;
    public Transform rightHand;
    public Transform leftHand;
    public Transform back;
    enum AnimMotions :int {
        Stay,
        Walk,
        Run,
        Jump,
        Damage = 10,
        SwordAttack=50,
        MagicAttack=70,
        Enchant,
        Gard,
        Doyaa = 100
    }
    private PlayerController.PlayerStates oldPlcState;
    private int oldDire;
    void Start()
    {
        
    }

    void Update()
    {
        switch(plc.nowPlayerState){
            case PlayerController.PlayerStates.Stay:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.Stay);
            }break;
            case PlayerController.PlayerStates.Runing:{
                plAnim.SetInteger("AnimNum",(int)AnimMotions.Run);
            }break;
        }
        if(plc.nowPlayerState != oldPlcState){
            switch (plc.nowPlayerState){
                case PlayerController.PlayerStates.Stay:{
                    plc.weapon.transform.parent = back.transform;
                    plc.weapon.transform.localPosition = new Vector3(0,0,0);
                    plc.weapon.transform.localEulerAngles = new Vector3(0,0,0);
                    model.transform.localEulerAngles = new Vector3(0,180,0);
                }break;
                case PlayerController.PlayerStates.Runing:{
                    plc.weapon.transform.parent = back.transform;
                    plc.weapon.transform.localPosition = new Vector3(0,0,0);
                    plc.weapon.transform.localEulerAngles = new Vector3(0,0,0);
                    if(plc.direction < 0) model.transform.localEulerAngles = new Vector3(0,180,0);
                    else model.transform.localEulerAngles = new Vector3(0,0,0);
                }break;
                case PlayerController.PlayerStates.UpSlash:{
                    plAnim.Play("UpSlash",0,0);
                    plc.weapon.transform.parent = rightHand.transform;
                    plc.weapon.transform.localPosition = new Vector3(0,0,0);
                    if(plc.direction < 0) model.transform.localEulerAngles = new Vector3(0,180,0);
                    else model.transform.localEulerAngles = new Vector3(0,0,0);
                }break;
                case PlayerController.PlayerStates.Thrust:{
                    plAnim.Play("Thrust",0,0);
                    plc.weapon.transform.parent = rightHand.transform;
                    plc.weapon.transform.localPosition = new Vector3(0,0,0);
                    if(plc.direction < 0) model.transform.localEulerAngles = new Vector3(0,180,0);
                    else model.transform.localEulerAngles = new Vector3(0,0,0);
                }break;
                case PlayerController.PlayerStates.DownSlash:{
                    plAnim.Play("DownSlash",0,0);
                    plc.weapon.transform.parent = rightHand.transform;
                    plc.weapon.transform.localPosition = new Vector3(0,0,0);
                    if(plc.direction < 0) model.transform.localEulerAngles = new Vector3(0,180,0);
                    else model.transform.localEulerAngles = new Vector3(0,0,0);
                }break;
                case PlayerController.PlayerStates.ShotMagicBullet:{
                    plAnim.Play("ShotMagic",0,0);
                }break;
                case PlayerController.PlayerStates.Garding:{
                    plAnim.Play("ShotMagic",0,0);
                }break;
                case PlayerController.PlayerStates.EnchantMySelf:{
                    plAnim.Play("Enchant",0,0);
                }break;
                case PlayerController.PlayerStates.ActivateSpecialMagic:{
                    switch(plc.drawMagicSymbols[plc.drawMagicSymbols.Count - 2].magicSymbol){
                        case "RegularTriangle":{

                        }break;
                        case "InvertedTriangle":{

                        }break;
                        case "Thunder":{

                        }break;
                        case "Grass":{

                        }break;

                    }
                    plAnim.Play("Enchant",0,0);
                }break;
            }
        }
        if(oldDire != plc.direction){
            plAnim.SetInteger("Direction",plc.direction);
        }
        oldPlcState = plc.nowPlayerState;
        oldDire = plc.direction;
    }
}
