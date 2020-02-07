using UnityEngine;

namespace MPGameLib.Extensions
{
    public static class MaterialExtensions
    {
        public static void SetAlpha(this Material mat, float alpha)
        {
            var oriColor = mat.color;
            mat.color = new Color(oriColor.r, oriColor.g, oriColor.b, alpha);
        }
    }
}