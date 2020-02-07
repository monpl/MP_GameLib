using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MPGameLib.UI
{
    public enum PopupShowTransitionType
    {
        None,
        ScaleUp,
        Up,
        Down,
        Left,
        Right,
    }
    public enum PopupHideTransitionType
    {
        None,
        ScaleDown,
        Up,
        Down,
        Left,
        Right,
    }
    
    public class PopupBase : MonoBehaviour
    {
        [SerializeField] private PopupShowTransitionType popupShowType; 
        [SerializeField] private PopupHideTransitionType popupHideType;
        [SerializeField] private float showingTime = 0.3f;
        [SerializeField] private float hidingTime = 0.3f;
        [SerializeField] private bool dimmingHave = true;

        private CanvasRootObject _popupRoot;
        private Transform _popupTrs;
        private Vector2 _oriLocalPos;
        
        private Image _dimmingImg;

        private RectTransform _popupCanvasTrs;
        private Vector2 _screenSize;
        
        public bool IsShown { get; protected set; }

        public virtual void PreInit(RectTransform popupCanvasTrs)
        {
            if (PreInitChildPopup() == false)
                return;

            if (dimmingHave)
            {
                if (PreInitDimming() == false)
                    return;
            }

            _popupCanvasTrs = popupCanvasTrs;
            _popupRoot.SetActiveCanvasGroup(false);
            IsShown = false;
            
            Invoke(nameof(GetScreenSize), 0.05f);
        }

        private bool PreInitChildPopup()
        {
            var popup = transform.Find("Popup");
            
            if (popup == null)
            {
                Debug.LogError($"Popup is not exist! name: {name}");
                return false;
            }

            _popupRoot = popup.GetComponent<CanvasRootObject>();
            _oriLocalPos = popup.transform.localPosition;

            if (_popupRoot == null)
            {
                Debug.LogError($"Popup is not have CanvasRootObject.. name: {name}");
                return false;
            }

            _popupTrs = _popupRoot.transform;
            return true;
        }

        private bool PreInitDimming()
        {
            var dimming = transform.Find("Dimming");
            if (dimming == null)
            {
                Debug.LogError($"Dimming is not exist! name: {name}");
                return false;
            }

            // TODO: Replace to GetOrAddComponent<Image>();
            _dimmingImg = dimming.GetComponent<Image>();

            if (_dimmingImg == null)
            {
                Debug.LogError($"Dimming's Image is not exist! name: {name}");
                return false;
            }

            _dimmingImg.enabled = false;
            
            return true;
        }

        private void GetScreenSize()
        {
            _screenSize = _popupCanvasTrs.sizeDelta;
        }

        public virtual IEnumerator ShowPopup(bool isReOpen = false)
        {
            transform.SetAsLastSibling();
            _popupTrs.DOKill();
            _popupTrs.localPosition = _oriLocalPos;
            
            if (dimmingHave)
                _dimmingImg.enabled = true;

            switch (popupShowType)
            {
                case PopupShowTransitionType.ScaleUp:
                    _popupTrs.localScale = new Vector3(0.5f, 0.5f);
                    _popupRoot.SetActiveCanvasGroup(true);
                    yield return _popupTrs.DOScale(1f, showingTime).SetEase(Ease.OutBack).WaitForCompletion();
                    break;
                case PopupShowTransitionType.Up:
                    _popupTrs.localPosition = new Vector3(0, -_screenSize.y, 0);
                    _popupRoot.SetActiveCanvasGroup(true);
                    yield return _popupTrs.DOLocalMoveY(0f, showingTime).SetEase(Ease.Linear).WaitForCompletion();
                    break;
                case PopupShowTransitionType.Down:
                    _popupTrs.localPosition = new Vector3(0, _screenSize.y, 0);
                    _popupRoot.SetActiveCanvasGroup(true);
                    yield return _popupTrs.DOLocalMoveY(0f, showingTime).SetEase(Ease.Linear).WaitForCompletion();
                    break;
                case PopupShowTransitionType.Left:
                    _popupTrs.localPosition = new Vector3(-_screenSize.x, 0, 0);
                    _popupRoot.SetActiveCanvasGroup(true);
                    yield return _popupTrs.DOLocalMoveX(0f, showingTime).SetEase(Ease.Linear).WaitForCompletion();
                    break;
                case PopupShowTransitionType.Right:
                    _popupTrs.localPosition = new Vector3(_screenSize.x, 0, 0);
                    _popupRoot.SetActiveCanvasGroup(true);
                    yield return _popupTrs.DOLocalMoveX(0f, showingTime).SetEase(Ease.Linear).WaitForCompletion();
                    break;
                default:
                    _popupRoot.SetActiveCanvasGroup(true);
                    break;
            }
            if(isReOpen == false)
                ShowDone();
            else
            {
                SetGoodsArea();
                IsShown = true;
            }
        }

        public virtual void SetGoodsArea()
        {
            
        }
        
        public virtual void ShowWill()
        {
            IsShown = false;
        }

        protected virtual void ShowDone()
        {
            IsShown = true;
        }

        public virtual IEnumerator HidePopup(bool isTemporaryHide = false)
        {
            _popupTrs.DOKill();

            if (dimmingHave)
                _dimmingImg.enabled = false;

            IsShown = false;
            if (isTemporaryHide == false)
            {
                switch (popupHideType)
                {
                    case PopupHideTransitionType.ScaleDown:
                        yield return _popupTrs.DOScale(0, hidingTime).SetEase(Ease.Linear).WaitForCompletion();
                        break;
                    case PopupHideTransitionType.Up:
                        yield return _popupTrs.DOLocalMoveY(_screenSize.y, hidingTime).SetEase(Ease.Linear).WaitForCompletion();
                        break;
                    case PopupHideTransitionType.Down:
                        yield return _popupTrs.DOLocalMoveY(-_screenSize.y, hidingTime).SetEase(Ease.Linear).WaitForCompletion();
                        break;
                    case PopupHideTransitionType.Left:
                        yield return _popupTrs.DOLocalMoveX(-_screenSize.x, hidingTime).SetEase(Ease.Linear).WaitForCompletion();
                        break;
                    case PopupHideTransitionType.Right:
                        yield return _popupTrs.DOLocalMoveX(_screenSize.x, hidingTime).SetEase(Ease.Linear).WaitForCompletion();
                        break;
                    default:
                        break;
                }
            }

            HideDone();
        }
        
        public virtual void HideWill()
        {
            
        }

        private void HideDone()
        {
            IsShown = false;
        }
        
        public virtual void OnPressedBackKey()
        {
            PopupManager.Instance.PopHidePopup();
        }
    }
}