using System.Collections.Generic;
using MP_Patterns;
using UnityEngine;
using UnityEngine.UI;
using YCLib.Util;

namespace MPGameLib.Effect
{
    public class EffectInfo
    {
        public EffectBase effect;
        public string effectType;
    }

    public class EffectManager : Singleton<EffectManager>
    {
        private Transform _effectParent;
        private Transform _effectOnPopupParent;
        
        private Dictionary<string, ObjectPool<EffectBase>> _effectDictionary = new Dictionary<string,  ObjectPool<EffectBase>>();

        public void PreInit(Transform effectParent, Transform effectOnPopupParent, string effectsPath)
        {
            _effectParent = effectParent;
            _effectOnPopupParent = effectOnPopupParent;
            
            if(effectOnPopupParent != null)
                effectOnPopupParent.GetComponent<CanvasScaler>().matchWidthOrHeight = DeviceUtil.GetScaleMatch();
            
            var allEffects = Resources.LoadAll<EffectBase>(effectsPath);
            
            _effectDictionary.Clear();
            foreach (var effect in allEffects)
            {
                _effectDictionary.Add(effect.name, new ObjectPool<EffectBase>(effect, effect.name,
                    effect.isOnPopupEffect ? _effectOnPopupParent : _effectParent, effect.effectCount,
                    effect.effectCount / 2, !effect.isActiveOnOff));
            }
        }

        public void PlayEffect(string effectName, Vector2 spawnPos, Quaternion rotation = default)
        {
            if (_effectDictionary.ContainsKey(effectName))
                return;

            var effect = _effectDictionary[effectName].Pop();
            effect.PlayEffect(spawnPos, rotation, CollectEffect, effectName);
        }

        private void CollectEffect(EffectInfo effectInfo)
        {
            _effectDictionary[effectInfo.effectType]
                .Push(effectInfo.effect, !effectInfo.effect.isActiveOnOff);
        }
    }
}