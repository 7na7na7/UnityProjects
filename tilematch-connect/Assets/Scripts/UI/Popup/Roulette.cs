using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using DG.Tweening;

public class Roulette : BasePopup
{
    public Button closeButton;
    public Button startRoullete;

    public Transform lightSource;
    public Transform rouletteBoard;

    public GameObject itemLight;
    public GameObject itemDark;

    private void Start()
    {
        // 룰렛 아이템 초기화 // dark부터 시작해서 6개, light 6개
        for (int i = 1; i <= 6; i++)
        {
            int rotateDegree = - (i * 60); // 60도씩 증가
            GameObject _goDark = Instantiate(itemDark, rouletteBoard);
            GameObject _goLight = Instantiate(itemLight, rouletteBoard);

            // 필요한 컨텐츠(아이템)은 여기에 추가한다.
            // 회전
            _goDark.transform.rotation *= Quaternion.Euler(0, 0, rotateDegree);
            _goLight.transform.rotation *= Quaternion.Euler(0, 0, rotateDegree);
        }


        // 룰렛 돌리기 버튼 구독
        startRoullete.onClick.AsObservable().Subscribe(_ => {
            //RotateRoulette();
            startRoullete.interactable = false;
            // 룰렛 완료 감지
            Observable.FromCoroutine(RotateRoulette).Subscribe( //Subscribe( OnNext, OnCompleted ) 오버로드 함수 사용
            _ => {
                Debug.Log($"roulette end: {rouletteBoard.localEulerAngles.z}");
                // 룰렛 각도인 15도 모듈러로 얼만큼 되돌릴지 계산한다.
                float returnDegree = rouletteBoard.localEulerAngles.z % 30.0f;
                float targetDegree = rouletteBoard.localEulerAngles.z - returnDegree + 15.0f; // 15는 처음 보정 각도이다.
                Debug.Log($"degree: {returnDegree}");
                rouletteBoard.DORotateQuaternion(Quaternion.Euler(0, 0, targetDegree), 1.0f).OnComplete(() =>
                {
                    // 각도에 따라서 보상 팝업을 띄운다.
                    Debug.Log($"현재 각도 {rouletteBoard.localEulerAngles.z}");
                    int totalDegree = (int)rouletteBoard.localEulerAngles.z - 15; // 각도 보정
                    int matchedIndex = totalDegree / 30;
                    Debug.Log($"당첨된 인덱스:{matchedIndex}");

                    PopupController.Instance.ShowPopupReceive();
                });
            })
            .AddTo(this);
        });


        // 돌아가면서 전구 반짝이기
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => {
                LightSpinner();
            })
            .AddTo(this);
    }

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

        //GameAudioManager.Instance.PlaySound(GameAudioManager.SoundTag.UI_BTN_Common);
        InsertCloseEvent("Close");
        ClosePopup();
    }

    

    // 전구 돌리기
    private void LightSpinner()
    {
        lightSource.rotation *= Quaternion.Euler(0, 0, 30);
    }

    // 룰렛 완료시 현재 걸린 아이템 가져오기

    // coroutine
    // 룰렛 돌리기
    IEnumerator RotateRoulette()
    {
        float _elapsedTime = 0.0f;
        float _delayTime = 10.0f;

        float roatateSpeed = UnityEngine.Random.Range(5.0f, 15.0f);
        while (_elapsedTime < 1.0f)
        {
            _elapsedTime += Time.deltaTime / _delayTime;
            rouletteBoard.Rotate(0, 0, -roatateSpeed);
            roatateSpeed *= 0.99f;
            yield return null;
        }
    }
}
