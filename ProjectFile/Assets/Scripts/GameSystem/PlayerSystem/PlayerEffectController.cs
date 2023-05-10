using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    public MeshRenderer blade;
    Material bladeMaterial;
    void Start()
    {
        bladeMaterial = blade.material;
    }
    IEnumerator ActivationNormalParticle(MagicAttribute magicAttribute){
        switch(magicAttribute){
            case MagicAttribute.flame:{
            }break;
        }
        yield break;
    }
}
