using UnityEngine;

namespace MPGameLib.Util
{
    public static class DeviceUtil
    {
        /// <summary>
        /// Get Unity canvas Width / Height Match 
        /// </summary>
        /// <returns></returns>
        public static float GetScaleMatch()
        {
            var isPortrait = Screen.height > Screen.width;
            
            if(isPortrait)
                return (float) Screen.width / (float) Screen.height >= 0.55f ? 1f : 0f;
            
            return (float) Screen.height / (float) Screen.width >= 0.55f ? 1f : 0f;
        }
        
        /// <summary>
        /// Check the IphoneX
        /// </summary>
        /// <returns></returns>
        public static bool IsIphoneXorXSorXR()
        {
            // iPhoneX & XS
            if ((Screen.width == 1125 && Screen.height == 2436))
                return true;

            // iPhoneXR
            if ((Screen.width == 828 && Screen.height == 1792))
                return true;

            // XsMax
            if ((Screen.width == 1242 && Screen.height == 2688))
                return true;

            return false;
        }

        public static bool IsGalaxy8()
        {
            return Screen.width == 1440 && Screen.height == 2960;
        }

        /// <summary>
        /// 캔버스 스케일을 가져온다.
        /// </summary>
        /// <param name="targetResolutionSize">타겟 해상도</param>
        /// <param name="matchWidthOrHeight"> 0 or 1</param>
        /// <returns></returns>
        public static float GetCanvasScale(Vector2 targetResolutionSize, float matchWidthOrHeight = -1f)
        {
            if (matchWidthOrHeight < 0f)
                matchWidthOrHeight = GetScaleMatch();

            return Mathf.Pow(2f, Mathf.Lerp(
                Mathf.Log(Screen.width / targetResolutionSize.x, 2f),
                Mathf.Log(Screen.height / targetResolutionSize.y, 2f), matchWidthOrHeight));
        }

        public static Vector2 GetResolutionCanvasSize(Vector2 targetResolutionSize, float matchWidthOrHeight = -1f)
        {
            var canvasScale = GetCanvasScale(targetResolutionSize, matchWidthOrHeight);
            return new Vector2(Screen.width / canvasScale, Screen.height / canvasScale);
        }


        public static float GetOrthographicSize(Vector2 targetResolution)
        {
            return Screen.height * 0.01f / 2f / GetCanvasScale(targetResolution);
        }
    }
}