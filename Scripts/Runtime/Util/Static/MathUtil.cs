using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MPGameLib.Util
{
    public class MathUtil
    {
        public static float GetDivideStartPos(float targetSize, float targetMargin, float totalCnt, bool isFirstMinus = true)
        {
            return (isFirstMinus ? -1 : 1) * ((targetSize / 2) + (targetMargin / 2)) * Mathf.Max(totalCnt - 1, 0);
        }
        public static float GetDividePos(float targetSize, float targetMargin, int totalCnt, int curIdx, bool isFirstMinus = true)
        {
            var first = (isFirstMinus ? -1 : 1) * ((targetSize / 2) + (targetMargin / 2)) * (totalCnt - 1);
            
//            if(isFirstMinus == false)
                first += (targetSize + targetMargin) * curIdx * (isFirstMinus ? 1 : -1);
//            else
//                first += (targetSize + targetMargin) * curIdx;
            
            return first;
        }
        
        
        
//        public static float GetBoardLocalPos(int cellIndexPos, float targetSize, float targetMargin,
//            int totalSize)
//        {
//            var retX = GetDivideStartPos(targetSize, targetMargin, totalSize) +
//                       (targetSize + targetMargin) * cellIndexPos;
//
//            return retX;
//        }
        public static Vector2 GetBoardLocalPos(Vector2Int cellIndexPos, Vector2 targetSize, Vector2 targetMargin,
            Vector2Int boardSize)
        {
            var retX = GetDivideStartPos(targetSize.x, targetMargin.x, boardSize.x) +
                       (targetSize.x + targetMargin.x) * cellIndexPos.x;
            var retY = GetDivideStartPos(targetSize.y, targetMargin.y, boardSize.y, false) -
                       (targetSize.y + targetMargin.y) * cellIndexPos.y;
            
            return new Vector2(retX, retY);
        }
        
        // BoardUtil
        public static Vector2 GetBoardLocalPos(Vector2Int cellIndexPos, float targetSize, float targetMargin,
            Vector2Int boardSize)
        {
            var retX = GetDivideStartPos(targetSize, targetMargin, boardSize.x) +
                       (targetSize + targetMargin) * cellIndexPos.x;
            var retY = GetDivideStartPos(targetSize, targetMargin, boardSize.y, false) -
                       (targetSize + targetMargin) * cellIndexPos.y;
            
            return new Vector2(retX, retY);
        }

        public static float GetVector2Distance(Vector2 a, Vector2 b)
        {
            return (new Vector2(Mathf.Abs((a.x - b.x)), Mathf.Abs(a.y - b.y))).sqrMagnitude;
        }
        
        public static int GetNormalizedWidth()
        {
            return (int)((float)Screen.width / (float)Screen.height * 1920f);
        }

        public static int GetNormalizedHeight()
        {
            return (int)((float)Screen.height / (float)Screen.width * 1080f);
        }
        
        
        // Screen Util
        public static float GetScaleMatch()
        {
            return (float)Screen.width / (float)Screen.height >= 0.5625f ? 1f : 0f;
        }

        public static bool IsIphoneX()
        {
            return (Screen.width == 1125 && Screen.height == 2436);
        }
        
        // Time Util
        public static string UnixTimeNowToString()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return ((long) timeSpan.TotalSeconds).ToString();
        }
        public static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long) timeSpan.TotalSeconds;
        }
        
        // RandomUtil
        public static float GetRandomRangeEachSide(float min, float max)
        {
            if (min < 0 || max < 0)
            {
                Debug.LogError($"Min or max is not positive number, It's will be change to positive number.. min: {min}, max: {max}");
                min = Mathf.Abs(min);
                max = Mathf.Abs(max);
            }
            
            return UnityEngine.Random.Range(min, max) * (UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1);
        }
    }
}