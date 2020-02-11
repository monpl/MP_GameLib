using MPGameLib.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YCLib.Util.Editors;

namespace MPGameLib.UI.Editors
{
    public class PopupBaseCreateEditor : Editor
    {
        [MenuItem("GameObject/MPGameLib/Popups/PopupBase", false, 2)]
        static void CreateNewEmptyPopup()
        {
            // -----------------
            // Base Popup
            // Trs
            var popupBaseRectTrs = EditorUtil.CreateRectTransformNewObject("NewPopup", Selection.activeTransform);
            var popupBaseObj = popupBaseRectTrs.gameObject;
            popupBaseRectTrs.SetStretchAll();
            popupBaseObj.AddComponent<PopupBase>();

            // -----------------
            // Dimming
            // Trs
            var dimmingRectTrs = EditorUtil.CreateRectTransformNewObject("Dimming", popupBaseRectTrs);
            dimmingRectTrs.SetStretchAll();
            
            var dimmingImg = dimmingRectTrs.gameObject.AddComponent<Image>();
            dimmingImg.color = new Color(0, 0, 0, 0.5f);
            dimmingImg.enabled = false;
            
            // -----------------
            // Popup Child
            var popupRectTrs = EditorUtil.CreateRectTransformNewObject("Popup", popupBaseRectTrs);
            popupRectTrs.ResetLocalPosition();
            popupRectTrs.sizeDelta = new Vector2(800, 800);
            popupRectTrs.gameObject.AddComponent<CanvasRootObject>();
            
            // Bg
            var bg = EditorUtil.CreateRectTransformNewObject("PopupBg", popupRectTrs);
            bg.SetStretchAll();
            bg.gameObject.AddComponent<Image>();

            Selection.activeGameObject = popupBaseObj;
            Undo.RegisterCreatedObjectUndo(popupBaseObj, "Create Popup");
            Undo.RecordObject(popupBaseObj, "Create Popup");
        }
    }
}