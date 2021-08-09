using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 글로벌하게 타이머를 관리하는 매니저
/// </summary>
public class TimerManager : MonoBehaviour
{
    // 싱글톤
    protected TimerManager() { }
    private static TimerManager _instance;
    public static TimerManager Instance
    { get { return _instance; } }
    private void MakeSingleton()
    {
        if (_instance != null) DestroyImmediate(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleton();
    }

    // 설계
    public GameObject timer;
    //private List<GameObject> timerList = new List<GameObject>();
    private Dictionary<string, GameObject> timerDict = new Dictionary<string, GameObject>();

    /// <summary>
    /// 타이머 초기화 요청
    /// </summary>
    /// <param name="timerName">만들 타이머 이름</param>
    /// <param name="second">남은 시간 설정</param>
    public void InitTimer(string timerName, int second)
    {
        if (HasTimer(timerName))
        {
            SetTimer(timerName, second);
            return;
        }

        GameObject _go = Instantiate(timer);
        timerDict.Add(timerName, _go);

        // 자기자신 밑에 놓기
        timerDict[timerName].GetComponent<Transform>().SetParent(gameObject.transform);

        // 타이머 Init
        timerDict[timerName].GetComponent<Timer>().Init(second);
    }

    /// <summary>
    /// 타이머 진행 상태 확인
    /// </summary>
    /// <param name="timerName"></param>
    /// <returns>true, false</returns>
    public bool IsTimerRunning(string timerName)
    {
        return timerDict[timerName].GetComponent<Timer>().TimerStatus();
    }

    /// <summary>
    /// 타이머 실행
    /// </summary>
    /// <param name="timerName">타이머 이름</param>
    public void StartTimer(string timerName)
    {
        timerDict[timerName].GetComponent<Timer>().StartTimer();
    }

    /// <summary>
    /// 타이머 중지
    /// </summary>
    /// <param name="timerName">타이머 이름</param>
    public void StopTimer(string timerName)
    {
        timerDict[timerName].GetComponent<Timer>().StopTimer();
    }

    /// <summary>
    /// 타이머 재시작
    /// </summary>
    /// <param name="timerName">타이머 이름</param>
    public void ResumeTimer(string timerName)
    {
        timerDict[timerName].GetComponent<Timer>().ResumeTimer();
    }

    public bool HasTimer(string timerName)
    {
        return timerDict.ContainsKey(timerName);
    }

    /// <summary>
    /// 타이머 파괴
    /// </summary>
    /// <param name="timerName">타이머 이름</param>
    public void DestoryTimer(string timerName)
    {
        timerDict[timerName].GetComponent<Timer>().DesoryTimer();
        timerDict.Remove(timerName);
    }

    /// <summary>
    /// 타이머의 남은 시간을 변경
    /// </summary>
    /// <param name="timerName">타이머 이름</param>
    /// <param name="seconds">초 설정</param>
    public void SetTimer(string timerName, int seconds)
    {
        timerDict[timerName].GetComponent<Timer>().SetLeftTime(seconds);
    }

    /// <summary>
    /// 타이머가 끝났을때 받을 콜백 예약
    /// </summary>
    /// <param name="timerName">타이머 이름</param>
    /// <param name="callbackMethod">콜백 메소드</param>
    public void ReserveEndCallback(string timerName, Timer.timerEndCallBack callbackMethod)
    {
        timerDict[timerName].GetComponent<Timer>().TimerEndCallBack += callbackMethod;
    }

    /// <summary>
    /// 1초 단위로 타이머 남은 시간을 받을 콜백 요청
    /// </summary>
    /// <param name="timerName">타이머 이름</param>
    /// <param name="callbackMethod">콜백 메소드</param>
    public void ReserveLeftTimeCallback(string timerName, Timer.timerLeftTimeCallBack callbackMethod)
    {
        timerDict[timerName].GetComponent<Timer>().TimerLeftTimeCallBack += callbackMethod;
    }

}
