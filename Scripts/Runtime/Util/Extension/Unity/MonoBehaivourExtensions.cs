using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MPGameLib.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static void StopAndNullCoroutine(this MonoBehaviour mono, Coroutine co)
        {
            if(co != null)
                mono.StopCoroutine(co);
            
            co = null;
        }

        public static Coroutine RestartCoroutine(this MonoBehaviour mono, IEnumerator routine, Coroutine co)
        {
            mono.StopAndNullCoroutine(co);
            co = mono.StartCoroutine(routine);

            return co;
        }

        /// <summary>
        /// 자신의 child 찾는 함수, 만약 자식의 자식인 경우, (button/circle/bg) 이런식으로 계층을 적어준다. 
        /// </summary>
        public static T GetChildOrNull<T>(this MonoBehaviour mono, string childName) where T : Component
        {
            var child = mono.transform.Find(childName);
            return child == null ? null : child.GetComponent<T>();
        }
    }
}