using MPGameLib.Sdb;
using NUnit.Framework;
using UnityEngine;

namespace MPGameLib.Tests
{
    public class SdbManager_Test
    {
        [Test, Order(0)]
        public void SdbManager_LoadTestSdb()
        {
            SdbManager.InitSdbManager("MP_Test_Sdb");

            var testSdb = SdbManager.GetSdbOrNull<TestSdb>();

            Assert.IsNotNull(testSdb);

            Debug.Log($"TestSdb's testNum is {testSdb.testNum}");
        }
    }
}