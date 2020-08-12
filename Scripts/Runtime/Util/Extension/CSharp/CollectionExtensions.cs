using System.Collections.Generic;
using System.Linq;
using UMT;
using UnityEngine;

namespace MPGameLib.Extensions
{
    public static class CollectionExtensions
    {
        #region List<T>

        public static T GetLast<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");

            return list[Random.Range(0, list.Count)];
        }

        public static T GetRandomAndRemove<T>(this List<T> list)
        {
            var randomIndex = Random.Range(0, list.Count);
            var ret = list[randomIndex];

            list.RemoveAt(randomIndex);

            return ret;
        }

        public static void RemoveLast<T>(this List<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        public static T GetLastAndRemove<T>(this List<T> list)
        {
            var ret = list.GetLast();

            list.RemoveLast();

            return ret;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new System.Random();
            var n = list.Count;

            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Shuffle<T>(this IList<T> list, MersenneTwister mersenneRandom)
        {
            var n = list.Count;

            while (n > 1)
            {
                n--;
                var k = mersenneRandom.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        #endregion

        #region Dictionary<T>

        public static TKey GetRandomKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var existKeyList = dictionary.Select(div => div.Key).ToList();

            return existKeyList.GetRandom();
        }

        public static TValue GetRandomValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var existKeyList = new List<TValue>();

            foreach (var div in dictionary)
                existKeyList.Add(div.Value);

            return existKeyList.GetRandom();
        }

        #endregion
    }
}