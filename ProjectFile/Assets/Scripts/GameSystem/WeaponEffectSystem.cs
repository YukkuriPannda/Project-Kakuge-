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
            case MagicAttribute.Flame:
                particleSystems = normalParticles.flames;
            break;
            case MagicAttribute.Aqua:
                particleSystems = normalParticles.aquas;
            break;
            case MagicAttribute.Electro:
                particleSystems = normalParticles.electros;
            break;
            case MagicAttribute.Terra:
                particleSystems = normalParticles.terras;
            break;
        }
        foreach(ParticleSystem particle in particleSystems){
            particle.Play();
        }
    }
    public void DisableNormalParticle(){
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
            case MagicAttribute.Flame:
                particleSystems = attackParticles.flames;
            break;
            case MagicAttribute.Aqua:
                particleSystems = attackParticles.aquas;
            break;
            case MagicAttribute.Electro:
                particleSystems = attackParticles.electros;
            break;
            case MagicAttribute.Terra:
                particleSystems = attackParticles.terras;
            break;
        }
        foreach(ParticleSystem particle in particleSystems){
            particle.Play();
        }
    }
}
