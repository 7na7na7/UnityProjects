using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoGo.Extension;

[RequireComponent(typeof(ScreenController))]
[RequireComponent(typeof(UICameraScript))]
[RequireComponent(typeof(PopupController))]
public class UICenter : MonoBehaviour
{
    private static UICenter _instance;
    public static UICenter Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject _gameobject = GameObject.FindGameObjectWithTag("UICenter");
                _instance = _gameobject.GetComponent<UICenter>();
                return _instance;
            }
            else return _instance;
        }
    }

    [HideInInspector]
    public PopupController popupController;
    [HideInInspector]
    public UICameraScript uiCameraScript;

    public ScreenController screenController;
    //public MessagingHelper messagingHelper;

    public SpriteRenderer fadeRenderer;


    //public PlayTimeHelper playTimeHelper;

    public enum MessageStyle
    {
        Default,
    }



    public enum CanvasType
    {
        BuiltIn = 0,
        Float = 1,
        Popup = 2,
        Cover = 3
    }


    private void Awake()
    {

        if (popupController == null) popupController = gameObject.GetComponent<PopupController>();
        if (uiCameraScript == null) uiCameraScript = gameObject.GetComponent<UICameraScript>();
    }

    public void ActionFadeScreen(bool _Show)
    {
        if (_Show)
        {
            StartCoroutine(RoutineFade(0, 0.85f, 0.3f));
        }
        else
        {
            StartCoroutine(RoutineFade(0.85f, 0.0f, 0.3f));
        }
    }
    public Color screenColor;
    IEnumerator RoutineFade(float _from, float _to, float _duration, float _dealy = 0.0f,
        EventListener.Call_boolean _result = null)
    {
        screenColor.a = _from;
        fadeRenderer.color = screenColor;


        float _elapsedTime = 0;

        if (_dealy > 0)
        {
            while (_elapsedTime < 1)
            {
                _elapsedTime += Time.deltaTime / _dealy;
                yield return null;
            }
        }

        _elapsedTime = 0;
        while (_elapsedTime < 1.0f)
        {
            _elapsedTime += Time.deltaTime / _duration;
            screenColor.a = Mathf.Lerp(_from, _to, _elapsedTime.Interpolation(SmoothType.EaseInWithCos));
            fadeRenderer.color = screenColor;
            yield return null;
        }
        _result?.Invoke(true);
    }


    public void ShowMessage(string _desc)
    {
        //string _message = StringUtill.TransLocal(_desc);
        string _message = _desc; // todo: 추후 번역 작업 필요함
        //messagingHelper.ShowAnimationText(_message, 0.3f);
    }

    public void ShowMessage(string _desc, EventListener.Call_boolean _listener)
    {
        //string _message = StringUtill.TransLocal(_desc);
        string _message = _desc; // todo: 추후 번역 작업 필요함
        //messagingHelper.ShowAnimationText(_message, 0.3f, MessageStyle.Default, _listener);
    }



    #region Test Editor Button

    public void EditorBTN_FadeIn()
    {
        StartCoroutine(RoutineFade(0.85f, 0.0f, 1.0f));
    }
    public void EditorBTN_FadeOut()
    {
        StartCoroutine(RoutineFade(0, 0.85f, 1.0f));
    }
    public void EditorBTN_ShowMessage()
    {
        ShowMessage("테스트 메시지 연출 입니다.");
    }

    public void EditorBTN_OpenPopup()
    {

    }
    #endregion
}
