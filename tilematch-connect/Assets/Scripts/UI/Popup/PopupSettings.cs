using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PopupSettings : BasePopup
{
    public Button closeButton;


    public override void OpenPopup(PopupController _controller, EventListener.Call_string _listener = null, EventListener.Call_boolean _openCheck = null)
    {
        base.OpenPopup(_controller, _listener);


        closeButton.onClick.AsObservable().Subscribe(_ => OnClick_Close());
    }

    bool _isClicked;

    public void OnClick_Close()
    {
        if (_isClicked) return;
        _isClicked = true;

        // 인게임 타이머 재개하기
        TimerModel.StartTimer(TimerModel.TimerName.inGame);

        //GameAudioManager.Instance.PlaySound(GameAudioManager.SoundTag.UI_BTN_Common);
        InsertCloseEvent("Close");
        ClosePopup();
    }


}
