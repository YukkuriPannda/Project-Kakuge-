using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{   
    [System.Serializable]
    public class MagicColors{
        [ColorUsage(true, true)]public Color flame;
        [ColorUsage(true, true)]public Color aqua;
        [ColorUsage(true, true)]public Color electro;
        [ColorUsage(true, true)]public Color terra;
    }
    public MagicColors magicColors;
    public Material auraMat;
    public List<ParticleSystem> auraParSyss  = new List<ParticleSystem>();
    public ParticleSystem auraParSys_particle;
    public ParticleSystem auraParSys_smoke;

    void Start()
    {
    }
    public IEnumerator EnableNormalParticle(MagicAttribute magicAttribute){
        auraParSys_particle.Play();
        auraParSys_smoke.Play();
        Debug.Log("AAA");
        switch(magicAttribute){
            case MagicAttribute.flame:{
                auraMat.SetColor("_EmissionColor",magicColors.flame);
            }break;
            case MagicAttribute.aqua:{
                auraMat.SetColor("_EmissionColor",magicColors.aqua);
            }break;
            case MagicAttribute.electro:{
                auraMat.SetColor("_EmissionColor",magicColors.electro);
            }break;
            case MagicAttribute.terra:{
                auraMat.SetColor("_EmissionColor",magicColors.terra);
            }break;
        }
        yield break;
    }
    public void DisableNormalParticle(){
        auraParSys_particle.Stop(); 
        auraParSys_smoke.Stop();
    }
}
