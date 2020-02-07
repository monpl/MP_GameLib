using UnityEngine;

namespace MPGameLib.Extensions
{
    public static class ColorExtensions
    {
        public static Color Opaque(this Color color)
        {
            return new Color(color.r, color.g, color.b);
        }

        public static Color Invert(this Color color)
        {
            return new Color(1 - color.r, 1 - color.g, 1 - color.b, color.a);
        }

        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}