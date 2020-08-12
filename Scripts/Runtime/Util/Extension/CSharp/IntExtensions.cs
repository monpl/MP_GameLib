namespace MPGameLib.Extensions
{
    public static class IntExtensions
    {
        public static bool IsEvenNumber(this int number)
        {
            return number % 2 == 0;
        }
        
        /// <summary>
        /// 확률에 대한 참/거짓을 반환한다.
        /// </summary>
        /// <param name="probability">0~1사이의 값</param>
        /// <returns></returns>
        public static bool GetValueWithProbability(this float probability)
        {
            probability = UnityEngine.Mathf.Clamp(probability, 0f, 1f);

            if (probability >= 1f)
                return true;
            
            if (probability <= 0f)
                return false;

            return UnityEngine.Random.value <= probability;
        }
    }
}