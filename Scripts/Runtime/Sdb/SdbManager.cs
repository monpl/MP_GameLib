using System;
using System.Collections.Generic;
using UnityEngine;

namespace MPGameLib.Sdb
{
    public static class SdbManager
    {
        private static readonly Dictionary<Type, ScriptableObject> SdbDictionary = new Dictionary<Type, ScriptableObject>();
        private static bool _isInitialized;
        private const string _defaultPath = "Sdb/";

        public static void InitSdbManager(string sdbResourcePath = _defaultPath)
        {
            if (_isInitialized)
                return;

            var scriptableObjects = Resources.LoadAll<ScriptableObject>(sdbResourcePath);
            if (scriptableObjects == null || scriptableObjects.Length == 0)
            {
                Debug.LogError("Can't find sdb!");
                return;
            }

            foreach (var scriptObject in scriptableObjects)
            {
                var scriptObjectType = scriptObject.GetType();
                if (SdbDictionary.ContainsKey(scriptObjectType))
                {
                    Debug.LogError($"Overlap this type ( ignoring ): {scriptObjectType}");
                    continue;
                }
                
                SdbDictionary.Add(scriptObjectType, scriptObject);
            }

            _isInitialized = true;
        }

        public static T GetSdbOrNull<T>()  where T : ScriptableObject
        {
            if (_isInitialized == false)
                InitSdbManager();            
            
            var sdbType = typeof(T);

            if (SdbDictionary.ContainsKey(sdbType) == false)
                return null;

            return (T)SdbDictionary[sdbType];
        }
    }
}