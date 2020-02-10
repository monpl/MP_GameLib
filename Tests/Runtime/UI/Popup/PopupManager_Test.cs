using System.Collections;
using MPGameLib.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace MPGameLib.Tests
{
    public class PopupManager_Test_OnlyOneFalse
    {
        [Test, Order(0)]
        public void LoadTestScene()
        {
            SceneManager.LoadScene("PopupTestScene", LoadSceneMode.Single);
        }

        [UnityTest, Order(1)]
        public IEnumerator PopupTest_OnlyOneFalse()
        {
            PopupManager.Instance.PreInit(
                new[] {GameObject.Find("PopupCanvas").GetComponent<RectTransform>()},
                new PopupManagerSettings {isOnlyOnePopup = false});

            yield return new WaitForSeconds(0.5f);

            PopupManager.Instance.ShowPopup("TestPopup1");

            yield return new WaitForSeconds(1);

            PopupManager.Instance.PopHidePopup();

            yield return new WaitForSeconds(1);

            PopupManager.Instance.ShowPopup("TestPopup2");
            PopupManager.Instance.ShowPopup("TestPopup3");

            yield return new WaitForSeconds(2);

            PopupManager.Instance.HidePopup("TestPopup2");
            PopupManager.Instance.HidePopup("TestPopup3");

            yield return new WaitForSeconds(3);
        }
    }

    public class PopupManager_Test_OnlyOne
    {
        [Test, Order(0)]
        public void LoadTestScene()
        {
            SceneManager.LoadScene("PopupTestScene", LoadSceneMode.Single);
        }

        [UnityTest, Order(1)]
        public IEnumerator PopupTest_OnlyOne()
        {
            PopupManager.Instance.PreInit(
                new[] {GameObject.Find("PopupCanvas").GetComponent<RectTransform>()},
                new PopupManagerSettings {isOnlyOnePopup = true});

            yield return new WaitForSeconds(0.5f);

            PopupManager.Instance.ShowPopup("TestPopup1");
            PopupManager.Instance.ShowPopup("TestPopup2");
            PopupManager.Instance.ShowPopup("TestPopup3");
            
            yield return new WaitForSeconds(5);
            
            PopupManager.Instance.HidePopup("TestPopup3");
            PopupManager.Instance.HidePopup("TestPopup1");
            PopupManager.Instance.HidePopup("TestPopup2");
            
            yield return new WaitForSeconds(2);

            PopupManager.Instance.ShowPopup("TestPopup2");
            PopupManager.Instance.AddWaitingPopupQueue("TestPopup3");
            PopupManager.Instance.AddWaitingPopupQueue("TestPopup1");

            yield return new WaitForSeconds(3);
            
            PopupManager.Instance.PopHidePopup();

            yield return new WaitForSeconds(2);

            PopupManager.Instance.PopHidePopup();
            
            yield return new WaitForSeconds(4);
            
            PopupManager.Instance.PopHidePopup();
            
            yield return new WaitForSeconds(2);
        }
    }
}