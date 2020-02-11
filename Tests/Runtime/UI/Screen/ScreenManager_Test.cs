using System.Collections;
using MPGameLib.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace MPGameLib.Tests
{
    public class ScreenManager_Test
    {
        [Test, Order(0)]
        public void ScreenManager_PreInitTest()
        {
            SceneManager.LoadScene("ScreenTestScene", LoadSceneMode.Single);
        }

        [UnityTest, Order(1)]
        public IEnumerator ChangeScreenTest()
        {
            ScreenManager.Instance.PreInit(GameObject.Find("ScreenRoot").transform, 
                new ScreenManagerSettings 
                {
                    defaultChangeTime = 1f,
                    changeScreenBoth = true
                });
            
            ScreenManager.Instance.ChangeScreen("TestScreen1");
            yield return new WaitForSeconds(1.5f);
            
            ScreenManager.Instance.ChangeScreen("TestScreen2");
            yield return new WaitForSeconds(1.5f);

            ScreenManager.Instance.ChangeScreen("TestScreen1");
            yield return new WaitForSeconds(1.5f);
        }
    }
}