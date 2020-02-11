using MPGameLib.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YCLib.Util.Editors;

namespace MPGameLib.UI.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScaleButton))]
    public class ScaleButtonEditor : Editor
    {
        [MenuItem("GameObject/MPGameLib/Button/ScaleButton", false, 2)]
        static void Create()
        {
            // ------- Create code ---------
            var scaleButtonTrs = EditorUtil.CreateRectTransformNewObject("ScaleButton", Selection.activeTransform);
            var scaleButtonObj = scaleButtonTrs.gameObject;

            var scaleButtonComponent = scaleButtonObj.AddComponent<ScaleButton>();
            var scaleButton = scaleButtonObj.GetComponent<Button>();
            
            scaleButton.targetGraphic = scaleButtonComponent;
            scaleButton.transition = Selectable.Transition.None;
            // ------------------------------

            Selection.activeGameObject = scaleButtonObj;
        }

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