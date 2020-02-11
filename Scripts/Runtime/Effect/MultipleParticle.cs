using System;
using UnityEngine;

namespace MPGameLib.Effect
{
    public class MultipleParticle : EffectBase
    {
        [SerializeField] public ParticleSystem[] particleSystems;

        private float _longestLifeTime;

        public override void PreInit()
        {
            base.PreInit();
            foreach (var particle in particleSystems)
            {
                var lifeMaxTime = particle.main.startLifetime.constantMax;
                
                if (lifeMaxTime > _longestLifeTime)
                    _longestLifeTime = lifeMaxTime;
            }
        }

        public override void PlayEffect(Vector2 spawnPos, Quaternion rotation, Action<EffectInfo> OnComplete, string effectType)
        {
            base.PlayEffect(spawnPos, rotation, OnComplete, effectType);
            
            foreach(var particle in particleSystems)
                particle.Play();
        }

        public override float GetEffectingTime() => _longestLifeTime;
    }
}