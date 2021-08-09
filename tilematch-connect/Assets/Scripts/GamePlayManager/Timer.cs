using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타이머 매니저에서 사용하는 타이머 오브젝트
/// </summary>
public class Timer : MonoBehaviour
{
    private int totalLeftTime;
    private bool isRunTimer;

    // 호출할 콜백
    public delegate void timerEndCallBack();
    public event timerEndCallBack TimerEndCallBack;

    public delegate void timerLeftTimeCallBack(int leftTime);
    public event timerLeftTimeCallBack TimerLeftTimeCallBack;

    /// <summary>
    /// 최초 타이머 init
    /// </summary>
    /// <param name="leftTime">남은 시간</param>
    public void Init(int leftTime)
    {
        totalLeftTime = leftTime;
        isRunTimer = true;
        //Debug.Log($"Timer Init: {totalLeftTime}");
    }

    /// <summary>
    /// 타이머 진행 상태 가져오기
    /// </summary>
    /// <returns>true, false</returns>
    public bool TimerStatus()
    {
        return isRunTimer;
    }

    /// <summary>
    /// 타이머 시작, 남은 시간이 감소한다.
    /// </summary>
    public void StartTimer()
    {
        isRunTimer = true;
        StartCoroutine("Run");
    }

    /// <summary>
    /// 타이머 일시 중지
    /// </summary>
    public void StopTimer()
    {
        isRunTimer = false;
    }

    /// <summary>
    /// 타이머 재가동, 일시 중지 이후부터 진행
    /// </summary>
    public void ResumeTimer()
    {
        isRunTimer = true;
        StartCoroutine("Run");
    }

    /// <summary>
    /// 오브젝트 삭제
    /// </summary>
    public void DesoryTimer()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 남은 시간 돌려줌
    /// </summary>
    /// <returns>초 단위</returns>
    public int GetLeftTime()
    {
        return totalLeftTime;
    }

    /// <summary>
    /// 남은 시간을 재조정
    /// </summary>
    /// <param name="seconds"></param>
    public void SetLeftTime(int seconds)
    {
        totalLeftTime = seconds;
    }

    /// <summary>
    /// 타이머 동작 및 콜백 처리
    /// </summary>
    /// <returns></returns>
    private IEnumerator Run()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        if (totalLeftTime > 0 && isRunTimer == true)
        {
            totalLeftTime--;
            StartCoroutine("Run");
        }
        else if(totalLeftTime <= 0)
        {
            TimerEndCallBack?.Invoke();
        }
        TimerLeftTimeCallBack?.Invoke(totalLeftTime);
    }
}
