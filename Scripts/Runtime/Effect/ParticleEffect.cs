using System;
using UnityEngine;

namespace MPGameLib.Effect
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleEffect : EffectBase
    {
        public ParticleSystem ParticleSystem { get; private set;}

        public override void PreInit()
        {
            base.PreInit();
            ParticleSystem = GetComponent<ParticleSystem>();
        }

        public override void PlayEffect(Vector2 spawnPos, Quaternion rotation, Action<EffectInfo> OnComplete, string effectType)
        {
            base.PlayEffect(spawnPos, rotation, OnComplete, effectType);
            ParticleSystem.Play();
        }

        public override float GetEffectingTime()
        {
            return ParticleSystem.main.startLifetime.constantMax;
        }
    }
}