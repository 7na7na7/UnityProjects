using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using DG.Tweening;
using System;
using UniRx.Toolkit;

public class IngameUIPresenter : MonoBehaviour
{
    // ui text
    // top
    public TextMeshProUGUI leftTimeMin;
    public TextMeshProUGUI leftTimeSec;
    public TextMeshProUGUI starPoints;
    public TextMeshProUGUI stageName;
    public TextMeshProUGUI currentLevel;
    // star
    public Image starGauge;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    // bottom
    public TextMeshProUGUI itemChangeTile;
    public TextMeshProUGUI itemCrushBadTile;
    public TextMeshProUGUI itemShuffle;
    public TextMeshProUGUI itemHint;

    // button
    public Button btnChangeTile;
    public Button btnCrushBadTile;
    public Button btnShuffle;
    public Button btnHint;
    public Button btnSettings;

    // image
    public RectTransform imgHintIcon;

    // star
    public StarShooter prefabStar;
    public RectTransform canvasFloat;
    public RectTransform targetStarPos;
    ObjectPooling<StarShooter> starObjPool;
    // 좌표 변환용
    public Canvas uiCanvas;
    public Camera ingameCamera;

    // model
    private LevelModel levelModel = new LevelModel(100);

    // popupController
    public GameObject popupController;

    private void Start()
    {
        UserModel.Init();

        #region timer
        TimerModel.InitTimer(TimerModel.TimerName.inGame, levelModel.limitTime.Value);
        TimerModel.StartTimer(TimerModel.TimerName.inGame);

        // 타이머 변경 구독
        TimerModel.leftTimeArray[(int)TimerModel.TimerName.inGame].Subscribe(leftTime =>
        {
            var (min, sec) = TimerModel.SplitMinSec(leftTime);
            leftTimeMin.text = min;
            leftTimeSec.text = sec;
        });
        #endregion

        #region settings menu
        btnSettings.onClick.AsObservable().Subscribe(_ =>
        {
            // 시간 정지
            TimerModel.StopTimer(TimerModel.TimerName.inGame);
            popupController.GetComponent<PopupController>().ShowPopupSetting();
        });

        #endregion

        #region stage info
        UserModel.starPoints.Subscribe(x => starPoints.text = x.ToString());
        UserModel.stageLevel.Subscribe(x => currentLevel.text = x.ToString());
        UserModel.currentStageName.Subscribe(x => stageName.text = x);
        #endregion

        #region game item
        // text
        UserModel.itemChangeTileCount.Subscribe(x => itemChangeTile.text = x.ToString());
        UserModel.itemCrushBadTileCount.Subscribe(x => itemCrushBadTile.text = x.ToString());
        UserModel.itemHintCount.Subscribe(x => itemHint.text = x.ToString());
        UserModel.itemShuffleCount.Subscribe(x => itemShuffle.text = x.ToString());

        // button
        // 타일 변경
        btnChangeTile.onClick.AsObservable().Subscribe(_ =>
        {

        })
        .AddTo(this);
        // 셔플
        btnShuffle.onClick.AsObservable().Subscribe(_ =>
        {
            FieldModel.ShuffleTile();
            UserModel.itemShuffleCount.Value--;
        })
        .AddTo(this);
        // 힌트
        btnHint.onClick.AsObservable().Subscribe(_ =>
        {
            //FieldModel.HintTile(true);
            //UserModel.itemHintCount.Value--;
        })
        .AddTo(this);
        #endregion

        // 힌트 움직이게 하기
        Observable.Interval(TimeSpan.FromSeconds(4))
        .Subscribe(_ =>
        {
            imgHintIcon.DOScale(1.3f, 0.3f).SetEase(Ease.Flash).SetLoops(4, LoopType.Yoyo);
        })
        .AddTo(this);

        // 별 그리기 구독
        MatchModel.drawNode.Subscribe(x =>
        {
            DrawStar(MatchModel.FinalNodeList,x.lineIsMatched);
            //Debug.Log($"draw star");
        })
        .AddTo(this);

        // 별 게이지
        MatchModel.nodeStarAmount.Subscribe(_ =>
        {
            starGauge.fillAmount += 0.043f;
            if (starGauge.fillAmount >= 0.2f) star1.SetActive(true);
            if (starGauge.fillAmount >= 0.6f) star2.SetActive(true);
            if (starGauge.fillAmount >= 0.8f) star3.SetActive(true);
        })
        .AddTo(this);

        #region victory and fail
        // 남은 게임 타일이 없을 때 승리 표시
        FieldModel.leftTileCount.Where(x => x <= 0)
        .Subscribe(x =>
        {
            Observable.Timer(TimeSpan.FromSeconds(1.5f))
            .Subscribe(_ =>
            {
                popupController.GetComponent<PopupController>().ShowPopupVictory();
            })
            .AddTo(this);
        });
        #endregion

        # region object pool
        starObjPool = new ObjectPooling<StarShooter>(canvasFloat, prefabStar);
        #endregion

    }

    /// <summary>
    /// 별 그리기
    /// </summary>
    /// <param name="nodes"></param>
    public void DrawStar(List<Node> nodes,bool isMatched)
    {
        if (nodes == null) return;

        BuiltInUIController builtIn = gameObject.GetComponent<BuiltInUIController>();
        foreach (Node node in nodes)
        {
            float offsetX = FieldModel.fieldXOffset;
            float offsetY = FieldModel.fieldYOffset;
            Vector3 starCoords = new Vector3(node.x + offsetX, node.y + offsetY, 0);
            Vector3 convertedVector = WorldToCanvas(uiCanvas, starCoords, ingameCamera); // init pos

            // 오브젝트풀 사용
            StarShooter starShooter = starObjPool.Rent();

            starShooter.ShootStar(targetStarPos, convertedVector,isMatched).Subscribe(_ =>
            {
                starObjPool.Return(starShooter);
            });

            //Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ =>
            //{
            //    starObjPool.Return(starPresenter);
            //});

        }
    }

    // 좌표 변환
    public Vector3 WorldToCanvas(Canvas _canvas, Vector3 _position, Camera _camera)
    {
        if (_camera == null) _camera = Camera.main;

        var viewport_position = _camera.WorldToViewportPoint(_position);
        var canvas_rect = _canvas.GetComponent<RectTransform>();

        float canvasPosX = (viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f);
        float canvasPosY = (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f);

        return new Vector3(canvasPosX, canvasPosY, 0);
    }

}
