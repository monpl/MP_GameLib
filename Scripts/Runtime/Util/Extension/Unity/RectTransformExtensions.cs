using UnityEngine;

namespace MPGameLib.Extensions
{
    public static class RectTransformExtensions
    {
        public static RectTransform ChangeToAllStretch(this RectTransform rt)
        {
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            return rt;
        }

        public static RectTransform SetLeft(this RectTransform rt, float x)
        {
            rt.offsetMin = new Vector2(x, rt.offsetMin.y);
            return rt;
        }

        public static RectTransform SetRight(this RectTransform rt, float x)
        {
            rt.offsetMax = new Vector2(-x, rt.offsetMax.y);
            return rt;
        }

        public static RectTransform SetBottom(this RectTransform rt, float y)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, y);
            return rt;
        }

        public static RectTransform SetTop(this RectTransform rt, float y)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -y);
            return rt;
        }


        // Creation Codes..
        public static RectTransform SetStretchAll(this RectTransform rt)
        {
            rt.localPosition = Vector3.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;

            return rt;
        }
    }
}