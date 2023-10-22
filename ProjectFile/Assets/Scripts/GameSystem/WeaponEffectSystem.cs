using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffectSystem : MonoBehaviour
{
    [System.Serializable]
    public class Particles{
        public List<ParticleSystem> flames = new List<ParticleSystem>();
        public List<ParticleSystem> aquas = new List<ParticleSystem>();
        public List<ParticleSystem> electros = new List<ParticleSystem>();
        public List<ParticleSystem> terras = new List<ParticleSystem>();
    }
    public Particles attackParticles;
    public Particles normalParticles;
    public ItemCategory category;
    public void EnableNormalParticle(MagicAttribute magicAttribute){
        Debug.Log(gameObject.name);
        List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        switch(magicAttribute){
            case MagicAttribute.flame:
                particleSystems = normalParticles.flames;
            break;
            case MagicAttribute.aqua:
                particleSystems = normalParticles.aquas;
            break;
            case MagicAttribute.electro:
                particleSystems = normalParticles.electros;
            break;
            case MagicAttribute.terra:
                particleSystems = normalParticles.terras;
            break;
        }
        foreach(ParticleSystem particle in particleSystems){
            particle.Play();
        }
    }
    public void UnEnableNormalParticle(){
        foreach(ParticleSystem particle in normalParticles.flames){
            particle.Stop();
        }
        foreach(ParticleSystem particle in normalParticles.aquas){
            particle.Stop();
        }
        foreach(ParticleSystem particle in normalParticles.electros){
            particle.Stop();
        }
        foreach(ParticleSystem particle in normalParticles.terras){
            particle.Stop();
        }
    }
    public void PlayAttackParticle(MagicAttribute magicAttribute){
        List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        switch(magicAttribute){
            case MagicAttribute.flame:
                particleSystems = attackParticles.flames;
            break;
            case MagicAttribute.aqua:
                particleSystems = attackParticles.aquas;
            break;
            case MagicAttribute.electro:
                particleSystems = attackParticles.electros;
            break;
            case MagicAttribute.terra:
                particleSystems = attackParticles.terras;
            break;
        }
        foreach(ParticleSystem particle in particleSystems){
            particle.Play();
        }
    }
}
