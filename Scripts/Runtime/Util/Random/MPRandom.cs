using System;
using System.Collections.Generic;
using UMT;
using UnityEngine;

namespace MPGameLib.Util.Random
{
    public static class RandomUtil
    {
        public static Dictionary<int, MersenneTwister> randomDic;

        private static bool _isInit;
        public static int curBaseSeed { get; private set; }
        
        public static void InitRandoms(int seed, Array randomEnumArray, int randInterval = 1000)
        {
            curBaseSeed = seed;
            randomDic = new Dictionary<int, MersenneTwister>();

            var idx = 0;

            foreach (var curRandomIdxObj in randomEnumArray)
            {
                var newSeed = seed + (randInterval * idx++);
                var randomIdx = (int) curRandomIdxObj;

                if (randomDic.ContainsKey(randomIdx))
                {
                    Debug.Log("RandomEnumArray is wrong..!");
                    continue;
                }
                
                randomDic.Add(randomIdx, new MersenneTwister(newSeed));
            }

            _isInit = true;
        }

        public static int GetRange(int randomIdx, int min, int max)
        {
            if (_isInit == false)
                return 0;

            return randomDic[randomIdx].Next(min, max);
        }

        public static void SetNextSeed(int randomIdx)
        {
            var curRandom = randomDic[randomIdx];
            var nextSeed = curRandom.GetSeed() + 1;

            randomDic[randomIdx] = new MersenneTwister(nextSeed);
        }
    }
}