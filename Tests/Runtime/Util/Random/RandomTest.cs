using System;
using MPGameLib.Extensions;
using MPGameLib.Util.Random;
using NUnit.Framework;
using UnityEngine;

namespace MPGameLib.Tests
{
    public class RandomTest
    {
        public enum RandomTypeEnum : int
        {
            type1,
            type2,
        }

        [Test, Order(0)]
        public void RandomAllTest()
        {
            RandomUtil.InitRandoms(1000, Enum.GetValues(typeof(RandomTypeEnum)));

            for (var i = 0; i < 10; ++i)
                PrintRandom(RandomTypeEnum.type1);
            
            Debug.Log("----------");
            
            RandomUtil.SetNextSeed(RandomTypeEnum.type1.IntValue());
            Debug.Log("---Set New Seed Type1!");

            for (var i = 0; i < 10; ++i)
                PrintRandom(RandomTypeEnum.type1);
            
            Debug.Log("----------");
            
            for (var i = 0; i < 10; ++i)
                PrintRandom(RandomTypeEnum.type2);
        }

        private void PrintRandom(RandomTypeEnum randomIdx)
        {
            Debug.Log($"{randomIdx}: {RandomUtil.randomDic[randomIdx.IntValue()].Next(0, 100)}");
        }
    }
}