using System.Collections;
using UnityEngine;
public class Beaaaaaaaam: PlayerMagicBase
{
    // スキル種別
    public override PlayerMagicFactory.MagicKind SkillKind
    { 
        get {return PlayerMagicFactory.MagicKind.Beaaaaaaaaaaam;}
    }
    public override IEnumerator ActivationFlameMagic(PlayerController plc)
    {   
        plc.StartCoroutine(ActivationBeaaam(plc,MagicAttribute.Flame));
        yield break;
    }
    public override IEnumerator ActivationAquaMagic(PlayerController plc)
    {
        plc.StartCoroutine(ActivationBeaaam(plc,MagicAttribute.Aqua));
        yield break;
    }
    public override IEnumerator ActivationElectroMagic(PlayerController plc)
    {
        plc.StartCoroutine(ActivationBeaaam(plc,MagicAttribute.Electro));
        yield break;
    }
    public override IEnumerator ActivationTerraMagic(PlayerController plc)
    {
        plc.StartCoroutine(ActivationBeaaam(plc,MagicAttribute.Terra));
        yield break;
    }
    IEnumerator ActivationBeaaam(PlayerController plc,MagicAttribute magicAttribute){
        int direction = plc.direction;
        AttackBase beaaaam = GameObject.Instantiate((GameObject)Resources.Load("Magics/BeaaaaaamPrefab"),plc.transform.position + new Vector3(0,0.3f,0),plc.transform.rotation)
            .GetComponent<AttackBase>();
        BoxCollider2D[] boxColliders2D = beaaaam.gameObject.GetComponents<BoxCollider2D>();
        beaaaam.tag = "Player";
        beaaaam.magicAttribute = MagicAttribute.Terra;
        beaaaam.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",MagicColorManager.GetColorFromMagicArticle(magicAttribute));
        if(direction == -1){
            beaaaam.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
            beaaaam.knockBack *= new Vector2(-1,1);
            boxColliders2D[0].offset = new Vector2(-boxColliders2D[0].offset.x,boxColliders2D[0].offset.y);
            boxColliders2D[1].offset = new Vector2(-boxColliders2D[1].offset.x,boxColliders2D[1].offset.y);
        }
        GameObject.Destroy(beaaaam.gameObject,2.7f);
        yield return new WaitForSeconds(0.6f);
        boxColliders2D[0].enabled = true;
        boxColliders2D[1].enabled = true;
        yield return new WaitForSeconds(1f);
        boxColliders2D[0].enabled = false;
        boxColliders2D[1].enabled = false;
        yield break;
    }
}