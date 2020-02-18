using UnityEditor;
using UnityEngine;

namespace MPGameLib.Util
{
    public static class EditorUtil
    {
        public static RectTransform CreateRectTransformNewObject(string objectName, Transform parent)
        {
            var newObject = new GameObject(objectName);
            var newObjectRectTrs = newObject.AddComponent<RectTransform>();
            
            newObjectRectTrs.SetParent(parent, false);

            return newObjectRectTrs;
        }
    }
}