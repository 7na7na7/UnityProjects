using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;



public static class TimerModel
{
    public enum TimerName
    {
        inGame,
        global
    }

    public static IntReactiveProperty[] leftTimeArray = new IntReactiveProperty[Enum.GetValues(typeof(TimerName)).Length];
    private static IDisposable[] subscribe = new IDisposable[Enum.GetValues(typeof(TimerName)).Length];

    public static void InitTimer(TimerName name, int leftTime)
    {
        //if (name == null || leftTime == null) return;

        leftTimeArray[(int)name] = new IntReactiveProperty(leftTime);
    }

    public static void StartTimer(TimerName name)
    {
        if (leftTimeArray[(int)name] == null || leftTimeArray[(int)name].Value <= 0) return;

        subscribe[(int)name] = Observable.Interval(TimeSpan.FromSeconds(1))
        .Subscribe(_ =>
        {
            if (leftTimeArray[(int)name].Value > 0)
            {
                leftTimeArray[(int)name].Value--;
            }
        });
    }

    public static void StopTimer(TimerName name)
    {
        subscribe[(int)name].Dispose();
    }

    /// <summary>
    /// 시간을 분/초로 분리해서 리턴
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static (string, string) SplitMinSec(int time)
    {
        int min = time / 60;
        int sec = time % 60;
        string secString = sec.ToString();
        string secZeroFill;
        if (sec < 10)
        {
            secZeroFill = "0" + secString;
        }
        else
        {
            secZeroFill = secString;
        }

        return (min.ToString(), secZeroFill);
    }
}
