using System.Collections;
using System.Collections.Generic;
using MP_Patterns;
using MPGameLib.Extensions;
using MPGameLib.Util;
using UnityEngine;
using UnityEngine.UI;

namespace MPGameLib.UI
{
    public class PopupManagerSettings
    {
        public bool isOnlyOnePopup;
        public float dimmingTime = 0.1f;
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

    public class PopupManager : Singleton<PopupManager>
    {
        private RectTransform[] _popupRoots;
        private PopupManagerSettings _settings;

        public Dictionary<string, PopupBase> PopupDic { get; private set; }
        public List<string> showingPopupList { get; private set; }
        public Queue<PopupActionData> waitingPopupQueue { get; private set; }

        // Action을 받으면 코루틴을 돌려 큐로 돌리기로!
        private Queue<PopupActionData> _popupActions;

        public void PreInit(RectTransform[] popupRoots, PopupManagerSettings settings)
        {
            _popupRoots = popupRoots;
            _settings = settings;

            PopupDic = new Dictionary<string, PopupBase>();
            showingPopupList = new List<string>();
            waitingPopupQueue = new Queue<PopupActionData>();
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
                    {
                        PopupDic.Add(child.name, child);

                        child.gameObject.SetActive(true);
                        child.PreInit(popupRoot, _settings.dimmingTime);
                    }
                }

                popupRoot.GetComponent<CanvasScaler>().matchWidthOrHeight = DeviceUtil.GetScaleMatch();
            }
        }

        public void AddWaitingPopupQueue(string waitPopupName, float delay = 0.0f)
        {
            waitingPopupQueue.Enqueue(new PopupActionData
                {actionType = PopupAction.Show, delay = delay, popupName = waitPopupName});
        }

        private void PopWaitingQueue()
        {
            if (waitingPopupQueue.Count == 0)
                return;

            var waitingPopupData = waitingPopupQueue.Dequeue();
            AddPopupAction(waitingPopupData.actionType, waitingPopupData.popupName, waitingPopupData.delay);
        }

        public void ShowPopup(string showingPopup, float delay = 0.0f)
        {
            AddPopupAction(PopupAction.Show, showingPopup, delay);
        }

        public void PopHidePopup(float delay = 0.0f)
        {
            AddPopupAction(PopupAction.PopHide, "", delay);
        }

        public void HidePopup(string hidingPopup, float delay = 0.0f)
        {
            AddPopupAction(PopupAction.Hide, hidingPopup, delay);
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
                    if (showingPopupList.Count == 0 && waitingPopupQueue.Count > 0)
                        PopWaitingQueue();

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

                        yield return ShowPopupRoutine(popupName);
                        showingPopupList.Add(popupName);
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
                        yield return HidePopupRoutine(lastPopupName);
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

            if (_settings.isOnlyOnePopup && showingPopupList.Count >= 1)
            {
                var hidePopupName = showingPopupList.GetLast();
                StartCoroutine(PopupDic[hidePopupName].HidePopup(true));
            }

            yield return curPopup.ShowPopup();

            while (curPopup.IsShown == false)
                yield return null;
        }

        private IEnumerator HidePopupRoutine(string hidingPopupName)
        {
            var hidingPopup = PopupDic[hidingPopupName];

            // TODO: Show Only one Popup 처리
            if (_settings.isOnlyOnePopup && showingPopupList.Count >= 1)
            {
                if (PopupDic[hidingPopupName].IsShown)
                {
                    var prevPopupName = showingPopupList.GetLast();
                    StartCoroutine(PopupDic[prevPopupName].ShowPopup(true));
                }
            }

            hidingPopup.HideWill();

            yield return hidingPopup.HidePopup();
        }
    }
}