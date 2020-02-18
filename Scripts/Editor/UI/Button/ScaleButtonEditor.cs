using MPGameLib.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MPGameLib.UI.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScaleButton))]
    public class ScaleButtonEditor : Editor
    { 
        private void OnEnable()
        {
            var targetTrs = ((ScaleButton)target).GetComponent<RectTransform>();
            if (targetTrs.FindRecursively("AnimationObject") == null)
            {
                var animObj = new GameObject("AnimationObject", typeof(RectTransform));
                animObj.transform.SetParent(targetTrs);
                animObj.transform.localPosition = Vector3.zero;

                var image = new GameObject("MainImage", typeof(RectTransform), typeof(Image));
                var imageRectTrs = image.GetComponent<RectTransform>();
                
                imageRectTrs.SetStretchAll();
                image.transform.SetParent(animObj.transform, false);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("pressingScale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("animationType"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}