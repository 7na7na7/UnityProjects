using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class PopupNoAds : BasePopup
{
    public Button closeButton;
    public Button buyButton;


    public override void OpenPopup(PopupController _controller, EventListener.Call_string _listener = null, EventListener.Call_boolean _openCheck = null)
    {
        base.OpenPopup(_controller, _listener);

        // 버튼 idle 애니메이션 등록
        Observable.Interval(TimeSpan.FromSeconds(3))
            .Subscribe(_ => {
                RectButtonIdle(buyButton.GetComponent<RectTransform>());
            })
            .AddTo(this);


        closeButton.onClick.AsObservable().Subscribe(_ => OnClick_Close());
    }

    bool _isClicked;

    public void OnClick_Close()
    {
        if (_isClicked) return;
        _isClicked = true;

        //GameAudioManager.Instance.PlaySound(GameAudioManager.SoundTag.UI_BTN_Common);
        InsertCloseEvent("Close");
        ClosePopup();
    }

}
