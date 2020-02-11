using System.Collections;
using System.Collections.Generic;
using MP_Patterns;
using UnityEngine;

namespace MPGameLib.UI
{
    public class ScreenManagerSettings
    {
        public float defaultChangeTime;
        public bool changeScreenBoth;
    }
    
    public class ScreenManager : Singleton<ScreenManager>
    {
        private Dictionary<string, ScreenBase> _screenDictionary;
        private ScreenManagerSettings _settings;
        
        public string CurScreenType { get; private set; }
        public bool IsTransitioning { get; private set; }
        
        public void PreInit(Transform screenRoot, ScreenManagerSettings settings)
        {
            _screenDictionary = new Dictionary<string, ScreenBase>();
            _settings = settings;
            
            FindAndAddScreens(screenRoot);

            CurScreenType = string.Empty;
            IsTransitioning = false;
        }
        
        private void FindAndAddScreens(Component screenRoot)
        {
            _screenDictionary.Clear();
            foreach (var child in screenRoot.GetComponentsInChildren<ScreenBase>())
            {
                if (_screenDictionary.ContainsKey(child.name)) 
                    continue;
                
                child.PreInit();
                child.gameObject.SetActive(true);
                    
                _screenDictionary.Add(child.name, child);
            }
        }

        public void ChangeScreen(string newScreenType, float changeTime = -1f, float delay = 0.0f)
        {
            if (IsTransitioning)
                return;

            if (string.IsNullOrEmpty(newScreenType) || !_screenDictionary.ContainsKey(newScreenType))
                return;

            if (newScreenType == CurScreenType || string.IsNullOrEmpty(CurScreenType))
                StartCoroutine(ShowFirstScreenRoutine(newScreenType));
            else
                StartCoroutine(ChangeScreenRoutine(newScreenType, changeTime < 0f ? _settings.defaultChangeTime : changeTime, delay));
        }
        
        private IEnumerator ShowFirstScreenRoutine(string firstScreenType)
        {
            IsTransitioning = true;
            var firstScreen = _screenDictionary[firstScreenType];
            
            if (string.IsNullOrEmpty(CurScreenType))
                firstScreen.gameObject.SetActive(true);
            
            CurScreenType = firstScreenType;

            firstScreen.SetGoodsArea();
            firstScreen.ShowWill();
            
            yield return firstScreen.ShowRoutine();
            IsTransitioning = false;
            firstScreen.ShowDone();
        }
        
        private IEnumerator ChangeScreenRoutine(string newScreenType, float changeTime, float delay)
        {
            IsTransitioning = true;
            
            var curScreen = _screenDictionary[CurScreenType];
            var newScreen = _screenDictionary[newScreenType];

            CurScreenType = newScreenType;

            yield return new WaitForSeconds(delay);
            newScreen.SetGoodsArea();
            curScreen.DismissWill();

            if (_settings.changeScreenBoth)
            {
                StartCoroutine(curScreen.DismissRoutine(changeTime));
                newScreen.ShowWill();
                
                yield return newScreen.ShowRoutine(changeTime);
                curScreen.DismissDone();
            }
            else
            {
                yield return curScreen.DismissRoutine(changeTime);
                curScreen.DismissDone();
                
                newScreen.ShowWill();
                yield return newScreen.ShowRoutine(changeTime);                
            }

            IsTransitioning = false;
            newScreen.ShowDone();
        }

        public ScreenBase GetCurrScreen()
        {
            return _screenDictionary[CurScreenType];
        }
    }
}