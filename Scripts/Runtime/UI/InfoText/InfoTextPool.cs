using System;
using System.Collections.Generic;
using MP_Patterns;
using UnityEngine;

namespace MPGameLib.UI
{
 
    [Serializable]
    public class InfoTextDic : SerializableDictionaryBase<string, InfoText> {}
    
    public class InfoTextPool : MonoBehaviour
    {
        [SerializeField] private RectTransform textPoolRoot;
        [SerializeField] private InfoTextDic infoTextPrefabDic;
     
        private Dictionary<string, ObjectPool<InfoText>> _infoTextPoolDic;
        public Func<bool> IsIgnoreFuncAtShow;

        public void PreInit()
        {
            InitDic();
        }

        public void Init()
        {
            transform.SetAsLastSibling();
        }

        public void Release()
        {
            PushAll();
        }

        private void InitDic()
        {
            _infoTextPoolDic = new Dictionary<string, ObjectPool<InfoText>>();

            foreach (var infoTextKVP in infoTextPrefabDic)
            {
                var infoTextKey = infoTextKVP.Key;
                var prefab = infoTextPrefabDic[infoTextKey];
                _infoTextPoolDic[infoTextKey] = new ObjectPool<InfoText>(prefab, infoTextKey, textPoolRoot,
                    Mathf.Max(prefab.textCount, 1), Mathf.Max(prefab.textCount / 2, 1)); // 최소 1개 이상
            }
        }

        public void ShowInfoText(string infoTextType, InfoTextShowData data, bool pushAllOthers = false)
        {
            if (IsContainInDic(infoTextType) == false)
                return;
            
            if(pushAllOthers)
                PushAll();
            
            if (IsIgnoreFuncAtShow?.Invoke() == true)
            {
                Debug.Log("Ignore show!!");
                return;
            }

            var newInfoText = _infoTextPoolDic[infoTextType].Pop();
            newInfoText.ShowInfoText(data, infoTextType, this);
        }
        
        public void PushText(InfoText infoText, string infoTextType)
        {
            if (IsContainInDic(infoTextType) == false)
                return;
            
            _infoTextPoolDic[infoTextType].Push(infoText);
        }

        public int GetInfoTextCount(string infoTextType)
        {
            if (IsContainInDic(infoTextType) == false)
                return -1;
            
            return _infoTextPoolDic[infoTextType].PopList.Count;
        }
        
        public void PushAll()
        {
            foreach (var poolKVP in _infoTextPoolDic)
            {
                poolKVP.Value.PushAll(false);
            }
        }

        private bool IsContainInDic(string infoTextType)
        {
            return _infoTextPoolDic.ContainsKey(infoTextType);
        }

        public void PushAllSpecific(string infoTextType)
        {
            if (IsContainInDic(infoTextType) == false)
                return;
            
            _infoTextPoolDic[infoTextType].PushAll(false);
        }
    }
}