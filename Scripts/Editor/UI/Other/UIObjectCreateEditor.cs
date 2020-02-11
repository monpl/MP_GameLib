using UnityEditor;
using UnityEngine.UI;
using YCLib.Util.Editors;

namespace MPGameLib.UI.Editors
{
    public class UIObjectCreateEditor : Editor
    {
        [MenuItem("GameObject/UI/Image_NoneRay", false, 2002)]
        static void CreateImage_NoneRay()
        {
            var imageRectTrs = EditorUtil.CreateRectTransformNewObject("Image", Selection.activeTransform);
            var imageObj = imageRectTrs.gameObject;
            var image = imageObj.AddComponent<Image>();

            image.raycastTarget = false;

            Selection.activeGameObject = imageObj;
            Undo.RegisterCreatedObjectUndo(imageObj, "Create Image");
            Undo.RecordObject(imageObj, "Create Image");
        }

        [MenuItem("GameObject/UI/Text_NoneRay", false, 2000)]
        static void CreateText_NoneRay()
        {
            var textRectTrs = EditorUtil.CreateRectTransformNewObject("Text", Selection.activeTransform);
            var textObj = textRectTrs.gameObject;
            var text = textObj.AddComponent<Text>();

            text.raycastTarget = false;

            Selection.activeGameObject = textObj;
            Undo.RegisterCreatedObjectUndo(textObj, "Create Text");
            Undo.RecordObject(textObj, "Create Text");
        }
    }
}