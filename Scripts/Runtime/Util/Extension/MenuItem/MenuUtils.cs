#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MPGameLib.Extensions
{
    public static class MenuUtils
    {
        [MenuItem("MPGameLib/Clear PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
#endif