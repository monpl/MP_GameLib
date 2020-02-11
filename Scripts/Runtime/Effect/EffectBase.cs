using System;
using MP_Patterns;
using UnityEngine;

namespace MPGameLib.Effect
{
    public abstract class EffectBase : MonoBehaviour, IManagedObject
    {
        public int effectCount;
        public bool isOnPopupEffect;
        public bool isActiveOnOff;

        protected Action<EffectInfo> _onComplete; 

        protected string _effectType;

        public virtual void PreInit() { }

        public virtual void Release()
        {
            if(isActiveOnOff)
                gameObject.SetActive(false);
        }
        
        public virtual void PlayEffect(Vector2 spawnPos, Quaternion rotation, Action<EffectInfo> OnComplete, string effectType)
        {
            if (isActiveOnOff)
                gameObject.SetActive(true);
            
            transform.rotation = rotation;
            transform.position = spawnPos;
            
            _onComplete = OnComplete;
            _effectType = effectType;
            Invoke(nameof(PlayedEffect), GetEffectingTime());
        }

        protected virtual void PlayedEffect()
        {
            _onComplete?.Invoke(new EffectInfo {effect = this, effectType = _effectType});
        }
        
        public abstract float GetEffectingTime();
    }
}