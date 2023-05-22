using System.Collections;
using UnityEngine;
public class Beaaaaaaaam: PlayerMagicBase
{
    // スキル種別
    public override PlayerMagicFactory.PlayerFlameMagicKind SkillKind
    { 
        get {return PlayerMagicFactory.PlayerFlameMagicKind.Beaaaaaaaaaaam;}
    }
    public override IEnumerator ActivationFlameMagic(PlayerController plc)
    {   
        plc.StartCoroutine(ActivationBeaaam(plc,MagicAttribute.flame));
        yield break;
    }
    public override IEnumerator ActivationAquaMagic(PlayerController plc)
    {
        plc.StartCoroutine(ActivationBeaaam(plc,MagicAttribute.aqua));
        yield break;
    }
    public override IEnumerator ActivationElectroMagic(PlayerController plc)
    {
        plc.StartCoroutine(ActivationBeaaam(plc,MagicAttribute.electro));
        yield break;
    }
    public override IEnumerator ActivationTerraMagic(PlayerController plc)
    {
        plc.StartCoroutine(ActivationBeaaam(plc,MagicAttribute.terra));
        yield break;
    }
    IEnumerator ActivationBeaaam(PlayerController plc,MagicAttribute magicAttribute){
        int direction = plc.direction;
        AttackBase beaaaam = GameObject.Instantiate((GameObject)Resources.Load("Magics/BeaaaaaamPrefab"),plc.transform.position + new Vector3(0,0.3f,0),plc.transform.rotation)
            .GetComponent<AttackBase>();
        BoxCollider2D[] boxColliders2D = beaaaam.gameObject.GetComponents<BoxCollider2D>();
        beaaaam.tag = "Player";
        beaaaam.magicAttribute = MagicAttribute.terra;
        beaaaam.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",MagicColorManager.GetColorFromMagicArticle(magicAttribute));
        if(direction == -1){
            beaaaam.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
            beaaaam.knockBack *= new Vector2(-1,1);
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