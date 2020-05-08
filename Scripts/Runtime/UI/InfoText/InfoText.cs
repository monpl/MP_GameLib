using System.Collections;
using MP_Patterns;
using MPGameLib.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace MPGameLib.UI
{
    /// <summary>
    /// 인포 텍스트 보여줄때 데이터
    /// </summary>
    public class InfoTextShowData
    {
        public Vector2 spawnPos;
        public float delay;
    }

    /// <summary>
    /// 정보를 띄워주는 이미지 텍스트 베이스
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class InfoText : MonoBehaviour, IManagedObject
    {
        public int textCount;
        [Header("Showing이 끝나고 지속되는 시간")]
        public float duration;
        
        private Coroutine _showRoutine;
        
        public virtual void PreInit()
        {
            
        }

        public virtual void Release()
        {
            
        }

        public void ShowInfoText(InfoTextShowData data, string infoTextType, InfoTextPool pool)
        {
            PrepareShow(data);

            _showRoutine = this.RestartCoroutine(ShowInfoTextRoutine(data, infoTextType, pool), _showRoutine);
        }

        protected virtual void PrepareShow(InfoTextShowData data)
        {
            transform.position = data.spawnPos;
        }

        protected virtual IEnumerator ShowInfoTextRoutine(InfoTextShowData data, string infoTextType, InfoTextPool pool)
        {
            yield return new WaitForSeconds(data.delay);
            yield return ShowingTextInfo();
            yield return HidingTextInfo();
            pool.PushText(this, infoTextType);
        }

        protected virtual IEnumerator ShowingTextInfo()
        {
            yield return new WaitForSeconds(duration);
        }

        protected virtual IEnumerator HidingTextInfo()
        {
            yield break;
        }
    }
}