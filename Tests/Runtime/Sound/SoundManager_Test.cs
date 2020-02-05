using System.Collections;
using MPGameLib.Sound;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace MPGameLib.Tests
{
    public class SoundManager_Test
    {
        private string _testSfxName = "TestSFX";
        
        // A Test behaves as an ordinary method
        [Test, Order(0)]
        public void SoundManager_PreInitTest()
        {
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
            SoundManager.Instance.PreInit(true, true, "Sounds/SFX", "Sounds/BGM", "Main");
        }

        [UnityTest, Order(1)]
        public IEnumerator BGMTest()
        {
            Debug.Log("BGM Pause!!");
            SoundManager.Instance.BgmAction(BgmAction.Pause);
            yield return new WaitForSeconds(1f);
            
            Debug.Log("BGM Resume!!");
            SoundManager.Instance.BgmAction(BgmAction.Resume);
            yield return new WaitForSeconds(1f);
            
            Debug.Log("BGM Stop!!");
            SoundManager.Instance.BgmAction(BgmAction.Stop);
            yield return new WaitForSeconds(1f);
            
            Debug.Log("BGM Play!!");
            SoundManager.Instance.BgmAction(BgmAction.Play);
            yield return new WaitForSeconds(1f);
            
            Debug.Log("BGM Change!!");
            SoundManager.Instance.ChangeBgm("Main2");
            yield return new WaitForSeconds(1f);
            
            Debug.Log("BGM Pitch!!");
            SoundManager.Instance.SetBGMPitch(1.5f, 1f);
            yield return new WaitForSeconds(1f);
            
            Debug.Log("BGM CLEAR!!");
            SoundManager.Instance.BgmAction(BgmAction.Stop);
        }

        [UnityTest, Order(2)]
        public IEnumerator SFXTest()
        {
            Debug.Log("SFX PlayEffect!!");
            SoundManager.Instance.PlayEffect(_testSfxName);
            yield return new WaitForSeconds(1f);
            
            Debug.Log("SFX PlayEffect IN Loop!!"); 
            SoundManager.Instance.PlayEffectInLoop(_testSfxName);
            Debug.Log("SFX PlayEffect SET PITCH!"); 
            SoundManager.Instance.SetSFXPitch(2.5f, 2f);
            yield return new WaitForSeconds(2f);
            
            Debug.Log("SFX PlayEffect IN Loop!!"); 
            SoundManager.Instance.StopEffectInLoop();
            yield return new WaitForSeconds(1f);
            
            Debug.Log("SFX CLEAR!!");
        }
    }
}
