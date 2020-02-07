using System.Collections;
using System.Collections.Generic;
using MP_Patterns;
using MPGameLib.Extensions;
using UnityEngine;
using UnityEngine.UI;
using YCLib.Util;

namespace MPGameLib.UI
{
    public class PopupManagerSettings
    {
        public bool isOnlyOnePopup;
    }
    
    public class PopupActionData
    {
        public PopupAction actionType;
        public string popupName;
        public float delay;
    }
    
    public enum PopupAction
    {
        Show,
        Hide,
        PopHide,
    }
    // 들어가야할 기능
    // 1. 웨이팅 팝업 큐
    //     - 팝업이 전부 사라지고 난 이후 나오는 대기중인 팝업
    // 2. 
    public class PopupManager : Singleton<PopupManager>
    {
        private RectTransform[] _popupRoots;
        private PopupManagerSettings _settings;

        public Dictionary<string, PopupBase> PopupDic { get; private set; }
        public List<string> showingPopupList { get; private set; }
        public Queue<string> waitingPopupQueue { get; private set; }

        // Action을 받으면 코루틴을 돌려 큐로 돌리기로!
        private Queue<PopupActionData> _popupActions;

        public void PreInit(RectTransform[] popupRoots, PopupManagerSettings settings)
        {
            _popupRoots = popupRoots;
            _settings = settings;
            
            PopupDic = new Dictionary<string, PopupBase>();
            showingPopupList = new List<string>();
            waitingPopupQueue = new Queue<string>();
            _popupActions = new Queue<PopupActionData>();
            
            FindChildPopups();
            
            StopAllCoroutines();
            StartCoroutine(PopupRoutine());
        }

        private void FindChildPopups()
        {
            foreach (var popupRoot in _popupRoots)
            {
                for (var i = 0; i < popupRoot.childCount; ++i)
                {
                    var child = popupRoot.GetChild(i).GetComponent<PopupBase>();
                    if (child != null)
                        PopupDic.Add(child.name, child);
                }

                popupRoot.GetComponent<CanvasScaler>().matchWidthOrHeight = DeviceUtil.GetScaleMatch();
            }
        }

        public void ShowPopup(string showingPopup, float delay = 0.0f)
        {
            AddPopupAction(PopupAction.Show, showingPopup, delay);
        }

        public void PopHidePopup(float delay =  0.0f)
        {
            AddPopupAction(PopupAction.PopHide, "", delay);
        }

        private void AddPopupAction(PopupAction actionType, string popupName, float delay = 0.0f)
        {
            _popupActions.Enqueue(new PopupActionData
            {
                actionType = actionType,
                popupName = popupName,
                delay = delay
            });
        }

        private IEnumerator PopupRoutine()
        {
            while (true)
            {
                if (_popupActions.Count == 0)
                {
                    yield return null;
                    continue;
                }

                var action = _popupActions.Dequeue();
                var popupName = action.popupName;
                
                yield return new WaitForSeconds(action.delay);
                
                switch (action.actionType)
                {
                    case PopupAction.Show:
                        // TODO: 대기 팝업인 경우엔 중복검사xx
                        if (showingPopupList.Contains(popupName))
                        {
                            Debug.Log($"Popup is overlap.. popup: {popupName}");
                            break;
                        }

                        showingPopupList.Add(popupName);
                        yield return ShowPopupRoutine(popupName);
                        break;
                    case PopupAction.Hide:
                        if (showingPopupList.Contains(popupName) == false)
                        {
                            Debug.Log($"It Is not have in ShowingPopupList / name: {action.actionType}");
                            break;
                        }

                        showingPopupList.Remove(popupName);
                        yield return HidePopupRoutine(popupName);
                        break;
                    case PopupAction.PopHide:
                        if (showingPopupList.Count == 0)
                            break;

                        var lastPopupName = showingPopupList.GetLastAndRemove();
                        showingPopupList.RemoveAt(showingPopupList.Count - 1);
                        yield return HidePopupRoutine(popupName);
                        break;
                    default:
                        break;
                }
                
                yield return null;
            }
        }

        private IEnumerator ShowPopupRoutine(string showPopupName)
        {
            var curPopup = PopupDic[showPopupName];
            
            curPopup.ShowWill();
            
            // TODO: SHOW only one popup 처리
            
            yield return curPopup.ShowPopup();
        }

        private IEnumerator HidePopupRoutine(string hidingPopupName)
        {
            var hidingPopup = PopupDic[hidingPopupName];
            
            // TODO: Show Only one Popup 처리

            hidingPopup.HideWill();

            yield return hidingPopup.HidePopup();
        }
    }
}