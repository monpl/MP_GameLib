using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YCLib.Util.Editors;

namespace MPGameLib.UI.Editors
{
    public class ScreenBaseCreateEditor : Editor
    {
        [MenuItem("GameObject/MPGameLib/Screen/ScreenBase", false, 2)]
        static void CreateScreenBase()
        {
            // AddSortingLayer("UI");
            
            var screenRectTrs = EditorUtil.CreateRectTransformNewObject("NewScreen", Selection.activeTransform);
            var screenObj = screenRectTrs.gameObject;

            var screenCanvas = screenObj.AddComponent<Canvas>();
            var screenScaler = screenObj.AddComponent<CanvasScaler>();
            screenObj.AddComponent<GraphicRaycaster>();
            screenObj.AddComponent<ScreenBase>();

            screenCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            screenScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            screenScaler.referenceResolution = new Vector2(1080f, 1920f);
            screenScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            screenScaler.matchWidthOrHeight = 1f;
            
            Selection.activeGameObject = screenObj;
            
            Undo.RegisterCreatedObjectUndo(screenObj, "Create Screen");
            Undo.RecordObject(screenObj, "Create Screen");
        }

        private static void AddSortingLayer(string addSortString)
        {
            var tagsAndLayersManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var sortingLayers = tagsAndLayersManager.FindProperty("m_SortingLayers");
            sortingLayers.InsertArrayElementAtIndex(sortingLayers.arraySize);
            
            var layerName = addSortString;
            for (int i = 0; i < sortingLayers.arraySize; i++)
            {
                if (sortingLayers.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue.Equals(layerName))
                    return;
            }
            
            sortingLayers.InsertArrayElementAtIndex(sortingLayers.arraySize);
            var newLayer = sortingLayers.GetArrayElementAtIndex(sortingLayers.arraySize - 1);
            newLayer.FindPropertyRelative("name").stringValue = layerName;
            newLayer.FindPropertyRelative("uniqueID").intValue = layerName.GetHashCode();
            
            tagsAndLayersManager.ApplyModifiedProperties();
        }
    }
}