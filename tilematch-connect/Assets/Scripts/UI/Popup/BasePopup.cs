using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoGo.Extension;
using DG.Tweening;

public class BasePopup : MonoBehaviour
{
    protected PopupController mPopupController = null;

    public EventListener.Call_string mEventListener = null;
    public Dictionary<int, string> mEventContainer = null;

    public RectTransform bodyRect;
    public CanvasGroup bodyGroup;

    public PopupTweenProfile tweenProfile;

    public Image overlay;

    public Color overlayOn;
    public Color overlayOff;

    public readonly int openHash = Animator.StringToHash("Open");
    public readonly int closeHash = Animator.StringToHash("Close");
    string _closeEventCheck = "";
    public void InsertCloseEvent(string _event = "")
    {
        if (_event == "") _closeEventCheck = PopupController.mCloseEvent;
        else _closeEventCheck = _event;
    }

    public Color overlayColor;


    #region 팝업 오픈 제어

    public virtual void OpenPopup(PopupController _controller, EventListener.Call_string _listener = null, EventListener.Call_boolean _openCheck = null)
    {
        mPopupController = _controller;
        mPopupController.OnEffect = true;
        mEventListener = _listener;
        //StartCoroutine(RoutineFade(0.0f, 1.0f, 0.3f));
        ActionOpenTween(_openCheck);
    }
    public virtual void OpenPopup(PopupController _controller, Dictionary<int, string> _eventContainer, EventListener.Call_string _listener)
    {
        mPopupController = _controller;
        mPopupController.OnEffect = true;
        mEventContainer = _eventContainer;
        mEventListener = _listener;
        //StartCoroutine(RoutineFade(0.0f, 1.0f, 0.3f));
        ActionOpenTween(null);
    }
    public virtual void OnClickButton(int _index)
    {
        if (mEventContainer != null) InsertCloseEvent(mEventContainer[_index]);
        ClosePopup();
    }

    #endregion

    #region 팝업 클로즈 제어

    public virtual void ClosePopup()
    {
        // 인게임 타이머가 존재하면 재개 todo: 나중에 쳬계를 잡아야 된다.(임시처리)
        //string ingameTimerName = "ingameTimer";
        //if (TimerManager.Instance.HasTimer(ingameTimerName))
        //{
        //    TimerManager.Instance.ResumeTimer(ingameTimerName);
        //}
        
        //StartCoroutine(RoutineFade(1.0f, 0.0f, 0.3f));
        //StartCoroutine(RoutineClose());
        ActionCloseTween();
        //float _curr = overlayColor.a;
        //StartCoroutine(RoutineColorChange(_curr, 0.0f, 0.2f));
    }
    protected bool isClosing;
    /// <summary>
    /// insert event ClosePopup
    /// </summary>
    /// <param name="_prev"></param>
    /// <param name="_next"></param>
    public virtual void ClosePopup(EventListener.CallBack _prev, EventListener.CallBack _next = null)
    {
        _prev?.Invoke();
        ActionCloseTween();
        _next?.Invoke();
    }
    /// <summary>
    /// Insert Default Close Delegate Action
    /// </summary>
    public virtual void ActionPrevClose()
    {
        if (isClosing) return; isClosing = true;
        //GameAudioManager.Instance.PlaySound(GameAudioManager.SoundTag.UI_BTN_Common);
        InsertCloseEvent();
    }

    #endregion

    public void ActionInDelayTime(EventListener.CallBack _action, float _delay)
    {
        StartCoroutine(RoutineDelayAction(_action, _delay));
    }
    IEnumerator RoutineDelayAction(EventListener.CallBack _action, float _delayTime)
    {
        float _elapsedTime = 0.0f;
        while (_elapsedTime < 1.0f)
        {
            _elapsedTime += Time.deltaTime / _delayTime;
            yield return null;
        }
        _action?.Invoke();
    }

    public void ActionOpenTween(EventListener.Call_boolean _openCheck = null)
    {
        //GameAudioManager.Instance.PlaySound(GameAudioManager.SoundTag.UI_Pop_Open);
        bodyRect.anchoredPosition = new Vector2(0.0f, -tweenProfile.startPosY);
        ActionInDelayTime(() => { bodyRect.DOPunchScale(tweenProfile.startPunchScale, tweenProfile.punchTime); },
            tweenProfile.startTime - tweenProfile.punchJust);

        overlay.DOColor(overlayOn, tweenProfile.punchTime).SetEase(tweenProfile.startEase);
        bodyRect.DOAnchorPosY(0.0f, tweenProfile.punchTime).SetEase(tweenProfile.startEase).OnComplete(() => {
            _openCheck?.Invoke(true);
        });
    }
    public void ActionCloseTween(EventListener.Call_boolean _closeCheck = null)
    {
        overlay.DOColor(overlayOff, tweenProfile.closeTime).SetEase(tweenProfile.closeEase);
        bodyGroup.DOFade(0, tweenProfile.closeTime).SetEase(tweenProfile.closeEase);
        bodyRect.DOAnchorPosY(-tweenProfile.closePosY, tweenProfile.closeTime).SetEase(tweenProfile.closeEase).OnComplete(() => {
            if (_closeEventCheck != "")
            {
                mEventListener?.Invoke(_closeEventCheck);
                mPopupController.OnEffect = false;
                mPopupController.CloseObject(gameObject);
            }
            else
            {
                mPopupController.OnEffect = false;
                mPopupController.CloseObject(gameObject);
            }
        });
    }

    // todo: 버튼 바운스 효과, 구조적 정리가 필요함
    public void RectButtonIdle(RectTransform _button)
    {
        float shrinkSize = 0.7f;
        float shrinkSpeed = 0.3f;
        float returnSpeed = 0.2f;
        // 1 회 반복
        _button.DOScale(shrinkSize, shrinkSpeed).OnComplete(() =>
        {
            _button.DOScale(1, returnSpeed).OnComplete(() =>
            {
                // 2 회 반복
                _button.DOScale(shrinkSize, shrinkSpeed).OnComplete(() =>
                {
                    _button.DOScale(1, returnSpeed);
                });
            });
        });
    }
}
