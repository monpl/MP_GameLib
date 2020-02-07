using System;
using System.Collections.Generic;
using MPGameLib.Data;
using NUnit.Framework;
using UnityEngine;

namespace MPGameLib.Tests
{
    [Serializable]
    public class MPTestDataInfo : IDataBase
    {
        public SaveType<int> coin;
        public SaveType<int> testInt;
        public SaveType<int> testInt2;
        public SaveType<float> floatTest3;
        public List<int> list2;

        public MPTestDataInfo() {}
        public MPTestDataInfo(int startCoin)
        {
            coin = new SaveType<int>(startCoin);
            testInt = new SaveType<int>(222);
            testInt2 = new SaveType<int>(333333);
            floatTest3 = new SaveType<float>(1.33333f);
            
        }
    }
    
    public class GameDataManager_Test
    {
        [Test, Order(0)]
        public void GameDataManager_SaveAndLoadTest()
        {
            PlayerPrefs.DeleteAll();
            
            var startCoin = 1000;
            
            GameDataManager.PreInit(new MPTestDataInfo(startCoin));
            
            var testDataInfo = GameDataManager.GetData<MPTestDataInfo>();
            
            Assert.IsNotNull(testDataInfo);
            Assert.True(testDataInfo.coin.val == startCoin);
            
            Debug.Log($"[LOAD] TestData.Coin: {testDataInfo.coin.val}, startCoin: {startCoin}");

            testDataInfo.coin.val = 333;

            GameDataManager.SaveData<MPTestDataInfo>(false);

            Debug.Log("Test Load / Save Done!");
        }
    }
}