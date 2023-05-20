using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{    public MeshRenderer blade;
    Material bladeMaterial;
    [System.Serializable]
    public class BladeParticles{
        public ParticleSystem flame;
        public ParticleSystem aqua;
        public ParticleSystem electro;
        public ParticleSystem terra;
    }
    public BladeParticles normalParticle;
    public BladeParticles attackParticle;

    void Start()
    {
        bladeMaterial = blade.material;
    }
    public IEnumerator ActivationNormalParticle(MagicAttribute magicAttribute,float t){
        switch(magicAttribute){
            case MagicAttribute.flame:{
                normalParticle.flame.Play(true);
                bladeMaterial.SetColor("_Color",MagicColorManager.flame);
            }break;
            case MagicAttribute.aqua:{
                normalParticle.aqua.Play(true);
                bladeMaterial.SetColor("_Color",MagicColorManager.aqua);
            }break;
            case MagicAttribute.electro:{
                normalParticle.electro.Play(true);
                bladeMaterial.SetColor("_Color",MagicColorManager.electro);
            }break;
            case MagicAttribute.terra:{
                normalParticle.terra.Play(true);
                
                bladeMaterial.SetColor("_Color",MagicColorManager.terra);
            }break;
        }
        yield return new WaitForSeconds(t);
        normalParticle.flame.Stop();
        normalParticle.aqua.Stop();
        normalParticle.electro.Stop();
        normalParticle.terra.Stop();
        bladeMaterial.SetColor("_Color",Color.white * 0.5f);
        yield break;
    }
    public IEnumerator ActivationAttackParticle(MagicAttribute magicAttribute){
        switch(magicAttribute){
            case MagicAttribute.flame:{
                attackParticle.flame.Play(true);
                bladeMaterial.SetColor("_Color",MagicColorManager.flame);
            }break;
            case MagicAttribute.aqua:{
                attackParticle.aqua.Play(true);
                bladeMaterial.SetColor("_Color",MagicColorManager.aqua);
            }break;
            case MagicAttribute.electro:{
                attackParticle.electro.Play(true);
                bladeMaterial.SetColor("_Color",MagicColorManager.electro);
            }break;
            case MagicAttribute.terra:{
                attackParticle.terra.Play(true);
                bladeMaterial.SetColor("_Color",MagicColorManager.terra);
            }break;
        }
        yield break;
    }
}
