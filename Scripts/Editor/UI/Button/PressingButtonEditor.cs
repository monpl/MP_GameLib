using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MPGameLib.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PressingButton))]
    public class PressingButtonEditor : Editor
    {
        private void OnEnable()
        {
            var targetTrs = ((PressingButton)target).GetComponent<RectTransform>();
            
            if (targetTrs.Find("ShadowImage") == null)
            {
                var mainObj = new GameObject("ShadowImage", typeof(RectTransform), typeof(Image));
                mainObj.GetComponent<Image>().raycastTarget = false;
                mainObj.transform.SetParent(targetTrs, false);
                mainObj.transform.localPosition = new Vector2(0, -50);
            }

            if (targetTrs.Find("MainImage") == null)
            {
                var mainObj = new GameObject("MainImage", typeof(RectTransform), typeof(Image));
                mainObj.GetComponent<Image>().raycastTarget = false;
                mainObj.transform.SetParent(targetTrs, false);
                mainObj.transform.localPosition = Vector3.zero;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("pressingDown"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("buttonSprite"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shadowColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isSmooth"));
            
            if (GUILayout.Button("Accept Image"))
            {
                var button = ((PressingButton)target);
                button.AcceptImages(false);
            }

            if (GUILayout.Button("Accept Image With Set Native"))
            {
                var button = ((PressingButton) target);
                button.AcceptImages(true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}