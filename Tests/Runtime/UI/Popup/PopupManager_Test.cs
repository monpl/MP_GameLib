using System.Collections;
using MPGameLib.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MPGameLib.Tests
{
    public class PopupManager_Test
    {
        [UnityTest, Order(1)]
        public IEnumerator PopupTest()
        {
            PopupManager.Instance.PreInit(
                new[] {GameObject.Find("PopupCanvas").GetComponent<RectTransform>()},
                new PopupManagerSettings {isOnlyOnePopup = false});

            yield return new WaitForSeconds(0.5f);
            
            PopupManager.Instance.ShowPopup("TestPopup");
        }
    }
}