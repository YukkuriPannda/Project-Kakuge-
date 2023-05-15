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
        int direction = plc.direction;
        AttackBase beaaaam = GameObject.Instantiate((GameObject)Resources.Load("Magics/BeaaaaaamPrefab"),plc.transform.position + new Vector3(1f * direction,0.3f,0),plc.transform.rotation)
            .GetComponent<AttackBase>();
        BoxCollider2D[] boxColliders2D = beaaaam.gameObject.GetComponents<BoxCollider2D>();
        beaaaam.tag = "Player";
        beaaaam.magicAttribute = MagicAttribute.flame;
        beaaaam.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",MagicColorManager.flame);
        if(direction == -1){
            beaaaam.gameObject.transform.localScale = new Vector3(-1,1,1);
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
    public override IEnumerator ActivationAquaMagic(PlayerController plc)
    {
        int direction = plc.direction;
        AttackBase beaaaam = GameObject.Instantiate((GameObject)Resources.Load("Magics/BeaaaaaamPrefab"),plc.transform.position + new Vector3(1f * direction,0.3f,0),plc.transform.rotation)
            .GetComponent<AttackBase>();
        BoxCollider2D[] boxColliders2D = beaaaam.gameObject.GetComponents<BoxCollider2D>();
        beaaaam.tag = "Player";
        beaaaam.magicAttribute = MagicAttribute.aqua;
        beaaaam.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",MagicColorManager.aqua);
        if(direction == -1){
            beaaaam.gameObject.transform.localScale = new Vector3(-1,1,1);
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
    public override IEnumerator ActivationElectroMagic(PlayerController plc)
    {
        int direction = plc.direction;
        AttackBase beaaaam = GameObject.Instantiate((GameObject)Resources.Load("Magics/BeaaaaaamPrefab"),plc.transform.position + new Vector3(1f * direction,0.3f,0),plc.transform.rotation)
            .GetComponent<AttackBase>();
        BoxCollider2D[] boxColliders2D = beaaaam.gameObject.GetComponents<BoxCollider2D>();
        beaaaam.tag = "Player";
        beaaaam.magicAttribute = MagicAttribute.electro;
        beaaaam.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",MagicColorManager.electro);
        if(direction == -1){
            beaaaam.gameObject.transform.localScale = new Vector3(-1,1,1);
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
    public override IEnumerator ActivationTerraMagic(PlayerController plc)
    {
        int direction = plc.direction;
        AttackBase beaaaam = GameObject.Instantiate((GameObject)Resources.Load("Magics/BeaaaaaamPrefab"),plc.transform.position + new Vector3(1f * direction,0.3f,0),plc.transform.rotation)
            .GetComponent<AttackBase>();
        BoxCollider2D[] boxColliders2D = beaaaam.gameObject.GetComponents<BoxCollider2D>();
        beaaaam.tag = "Player";
        beaaaam.magicAttribute = MagicAttribute.terra;
        beaaaam.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",MagicColorManager.terra);
        if(direction == -1){
            beaaaam.gameObject.transform.localScale = new Vector3(-1,1,1);
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