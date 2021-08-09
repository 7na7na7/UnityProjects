using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using DG.Tweening;
using System;

public class LobbyView : MonoBehaviour
{
    LobbyPresenter presenter;

    // text
    public TextMeshProUGUI starPoints;
    public TextMeshProUGUI userName;

    // button
    public Button popupSetting;
    public Button popupPersonalInfo;
    public Button popupRank;
    public Button popupStore;
    public Button popupStoreFromTop;
    public Button popupWorldMap;
    public Button buttonGameStart;

    // icon
    public Button roulette;
    public Button noAds;

    // for animation
    public RectTransform buttonRectTransformStart;
    public RectTransform rouletteBackground;
    public RectTransform stageInfoAnchor;

    private void Start()
    {
        presenter = new LobbyPresenter(this);

        // 버튼 이벤트 등록
        popupSetting.onClick.AsObservable().Subscribe(_ => OpenPopupSetting());
        popupPersonalInfo.onClick.AsObservable().Subscribe(_ => OpenPopupPersonalInfo());
        popupRank.onClick.AsObservable().Subscribe(_ => OpenPopupRank());
        popupStore.onClick.AsObservable().Subscribe(_ => OpenPopupStore());
        popupStoreFromTop.onClick.AsObservable().Subscribe(_ => OpenPopupStore());
        popupWorldMap.onClick.AsObservable().Subscribe(_ => OpenWorldMap());
        noAds.onClick.AsObservable().Subscribe(_ => OpenPopupNoAds());

        // 사이드 아이콘
        roulette.onClick.AsObservable().Subscribe(_ => OpenRoulette());

        // play
        buttonGameStart.onClick.AsObservable().Subscribe(_ =>
        {
            buttonGameStart.interactable = false;
            GameStart();
        });

        // 버튼 idle 애니메이션 등록
        Observable.Interval(TimeSpan.FromSeconds(4))
            .Subscribe(_ => {
                RectButtonIdle(buttonRectTransformStart);
            })
            .AddTo(this);

        // 룰렛 돌리기
        //MainThreadDispatcher.StartUpdateMicroCoroutine(SpinRoulette(rouletteBackground));
        rouletteBackground.DORotate(new Vector3(0, 0, 360), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

        // 액자 흔들리기
        Observable.Interval(TimeSpan.FromSeconds(3))
            .Subscribe(_ =>
            {
                ShakeFrame(stageInfoAnchor);
            })
            .AddTo(this);
    }

    public void ShakeFrame(RectTransform _frame)
    {
        float z = 4.0f;
        _frame.DORotate(new Vector3(0, 0, z), 0.2f).SetEase(Ease.Flash).OnComplete(() =>
        {
            _frame.DORotate(new Vector3(0, 0, -z), 0.4f).SetEase(Ease.Flash).OnComplete(() =>
            {
                _frame.DORotate(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.Flash);
            });
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

    public void SetStarPoints(int points)
    {
        starPoints.text = points.ToString();
    }

    public void SetUserName(string name)
    {
        userName.text = name;
    }


    #region IEnumerator
    //IEnumerator SpinRoulette(RectTransform rouletteBackground)
    //{
    //    while (true)
    //    {
    //        rouletteBackground.DORotateQuaternion(Quaternion.Euler(0, 0, 10), 1.0f);
    //        yield return null;
    //    }
    //}
    #endregion


    #region 사용자 이벤트
    // todo: 여기서 팝업을 바로 띄우는것 검토 필요함, 아마 present로 보내야 할거 같음
    private void OpenPopupSetting()
    {
        gameObject.GetComponent<PopupController>().ShowPopupSetting();
    }

    private void OpenPopupPersonalInfo()
    {
        gameObject.GetComponent<PopupController>().ShowPopupPersonalInfo();
    }

    private void OpenPopupRank()
    {
        gameObject.GetComponent<PopupController>().ShowPopupRank();
    }

    private void OpenPopupStore()
    {
        gameObject.GetComponent<PopupController>().ShowPopupStore();
    }

    private void OpenWorldMap()
    {
        gameObject.GetComponent<PopupController>().ShowPopupWorldMap();
    }

    private void OpenPopupNoAds()
    {
        gameObject.GetComponent<PopupController>().ShowPopupNoAds();
    }

    // game start
    private void GameStart()
    {
        GameSceneManager.Instance.LoadScene(GameSceneManager.SceneType.Play);
    }

    // roulette
    private void OpenRoulette()
    {
        gameObject.GetComponent<PopupController>().ShowRoulette();
    }
    #endregion
}
