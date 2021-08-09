using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BuiltInUIController : MonoBehaviour
{
    // top
    //// time
    //public TextMeshProUGUI leftTimeMin;
    //public TextMeshProUGUI leftTimeSec;
    // gauge
    public Image starBar;
    // star icon
    public GameObject star1, star2, star3;
    // star point
    public TextMeshProUGUI starPoints;
    // stage info
    public TextMeshProUGUI stageLevel;
    public TextMeshProUGUI stageName;


    // bottom
    // hint
    public TextMeshProUGUI hintCount;
    public TextMeshProUGUI shuffleCount;
    public TextMeshProUGUI crushBadTileCount;
    public TextMeshProUGUI changeTilesetCount;

    private string ingameTimerName = "ingameTimer";

    //UserModel userData;


    // Start is called before the first frame update
    private void Start()
    {
        //userData = new UserModel();

        //hintCount.text = userData.itemHintCount.ToString();
        //shuffleCount.text = userData.itemShuffleCount.ToString();
        //crushBadTileCount.text = userData.itemCrushBadTileCount.ToString();
        //changeTilesetCount.text = userData.itemChangeTileCount.ToString();
        //stageLevel.text = userData.stageLevel.ToString();
        //stageName.text = userData.currentStageName.ToString();
        //starPoints.text = userData.starPoints.ToString();

        starBar.fillAmount = 0.0f;
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);

        // 남은 시간은 레벨에서 가져온다.
        //TimerManager.Instance.InitTimer(ingameTimerName, LevelDesigner.thisLevel.limitTime);
        //TimerManager.Instance.StartTimer(ingameTimerName);
        //TimerManager.Instance.ReserveEndCallback(ingameTimerName, TimeOut);
        //TimerManager.Instance.ReserveLeftTimeCallback(ingameTimerName, LeftTime);

        UserIngameEvents.UpdateItemCount += UpdateItemText;
        StarController.AddStarGauge += AddStarGauge;
    }

    private void OnDestroy()
    {
        TimerManager.Instance.DestoryTimer(ingameTimerName);
    }

    // 타이머 다 되면 팝업 띄우기
    public void TimeOut()
    {
        PopupController.Instance.ShowPopupDefeat();
    }

    /// <summary>
    /// 분 초로 분할해서 업데이트 시켜줌
    /// </summary>
    /// <param name="leftTime"></param>
    public void LeftTime(int leftTime)
    {
        // 분, 초로 분할해서 업데이트 시켜주기
        int min = leftTime / 60;
        int sec = leftTime % 60;

        //leftTimeMin.text = min.ToString();
        //leftTimeSec.text = sec.ToString();
    }


    // event
    public void TimerToggle()
    {
        bool currentTimerRunningStatus = TimerManager.Instance.IsTimerRunning(ingameTimerName);
        if (currentTimerRunningStatus)
        {
            TimerManager.Instance.StopTimer(ingameTimerName);
        }
        else
        {
            TimerManager.Instance.ResumeTimer(ingameTimerName);
        }
    }

    public void UpdateItemText()
    {
        //hintCount.text = userData.itemHintCount.ToString();
        //shuffleCount.text = userData.itemShuffleCount.ToString();
        //crushBadTileCount.text = userData.itemCrushBadTileCount.ToString();
        //changeTilesetCount.text = userData.itemChangeTileCount.ToString();
    }

    /// <summary>
    /// star gauge를 올린다.
    /// </summary>
    /// <param name="count">올릴 값</param>
    public void AddStarGauge(int count)
    {
        for(int i = 0; i < count; i++)
        {
            starBar.fillAmount += 0.01f;
        }
        if (starBar.fillAmount >= 0.2f) star1.SetActive(true);
        if (starBar.fillAmount >= 0.6f) star2.SetActive(true);
        if (starBar.fillAmount >= 0.8f) star3.SetActive(true);
        //Debug.Log($"current star gauge: {starBar.fillAmount}");
    }
}
