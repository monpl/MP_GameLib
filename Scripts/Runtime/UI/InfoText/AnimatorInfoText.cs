using System.Collections;
using System.Linq;
using UnityEngine;

namespace MPGameLib.UI
{ 

    [RequireComponent(typeof(Animator))]
    public class AnimatorInfoText : InfoText
    {
        private Animator _animator;

        public override void PreInit()
        {
            base.PreInit();
            _animator = GetComponent<Animator>();
        }

        protected override void PrepareShow(InfoTextShowData data)
        {
            base.PrepareShow(data);

            _animator.enabled = true;
            Clear();
        }

        public override float GetTotalTime()
        {
            return base.GetTotalTime()
                   + _animator.runtimeAnimatorController.animationClips
                       .First(obj => obj.name == "Show").length;
        }

        protected override IEnumerator ShowingTextInfo()
        {
            yield return null;
            yield return PlayAniAndWait("Show");
            yield return base.ShowingTextInfo();
        }
        
        protected override IEnumerator HidingTextInfo()
        {
            yield return PlayAniAndWait("Hide");
        }
        
        private IEnumerator PlayAniAndWait(string animName)
        {
            _animator.PlayInFixedTime(animName, -1, 0.0f);
            
            yield return null; 
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        }

        public void Clear()
        {
            _animator.Play("Reset", -1);
        }
    }
}