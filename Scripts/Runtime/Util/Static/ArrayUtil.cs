namespace YCLib.Util
{
    public static class ArrayUtil
    {
        public static void NewArrayAndFullCopy<T>(T[] sourceArray, out T[] copyArray)
        {
            copyArray = new T[sourceArray.Length];
            sourceArray.CopyTo(copyArray, 0);
        }
    }
}