using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PopupVictory : BasePopup
{
    public Button home;
    public Button next;

    public override void OpenPopup(PopupController _controller, EventListener.Call_string _listener = null, EventListener.Call_boolean _openCheck = null)
    {
        base.OpenPopup(_controller, _listener);

        // 버튼 idle 애니메이션 등록
        Observable.Interval(TimeSpan.FromSeconds(3))
            .Subscribe(_ => {
                RectButtonIdle(next.GetComponent<RectTransform>());
            })
            .AddTo(this);

        //closeButton.onClick.AsObservable().Subscribe(_ => OnClick_Close());
    }

    private void Start()
    {
        home.onClick.AsObservable().Subscribe(_ =>
        {
            home.interactable = false;
            GameSceneManager.Instance.LoadScene(GameSceneManager.SceneType.Lobby);
        })
        .AddTo(this);

        next.onClick.AsObservable().Subscribe(_ =>
        {
            next.interactable = false;
            // 타이머 없애기
            TimerModel.StopTimer(TimerModel.TimerName.inGame);
            GameSceneManager.Instance.LoadScene(GameSceneManager.SceneType.Play);
        })
        .AddTo(this);
    }

    
}
