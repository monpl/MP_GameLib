using MPGameLib.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YCLib.Util.Editors;

namespace MPGameLib.UI.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PressingButton))]
    public class PressingButtonEditor : Editor
    {
        [MenuItem("GameObject/MPGameLib/Button/PressingButton", false, 2)]
        static void Create()
        {
            // ------- Create code ---------
            var pressingButtonTrs = EditorUtil.CreateRectTransformNewObject("PressingButton", Selection.activeTransform);
            var pressingButtonObj = pressingButtonTrs.gameObject;
            var pressingButtonComponent = pressingButtonObj.AddComponent<PressingButton>();

            var scaleButton = pressingButtonObj.GetComponent<Button>();
            scaleButton.targetGraphic = pressingButtonComponent;
            scaleButton.transition = Selectable.Transition.None;
            // ------------------------------

            Selection.activeGameObject = pressingButtonObj;
            Undo.RegisterCreatedObjectUndo(pressingButtonObj, "Create Pressing Button");
            Undo.RecordObject(pressingButtonObj, "Create Pressing Button");
        }
        
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
                button.AcceptImages();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}