using UnityEngine;
using UnityEngine.UI;

namespace MPGameLib.Extensions
{
    public static  class UIGraphicExtensions
    {
        public static void SetRgb(this Graphic graphic, Color rgb)
        {
            graphic.color = new Color(rgb.r, rgb.g, rgb.b, graphic.color.a);
        }

        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            var rgb = graphic.color;
            graphic.color = new Color(rgb.r, rgb.g, rgb.b, alpha);
        }

        public static void SetSpriteAndSetNative(this Image image, Sprite sprite)
        {
            image.sprite = sprite;
            image.SetNativeSize();
        }
    }
}