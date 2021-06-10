using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
public class AsyncTest : MonoBehaviour
{
    private async void Start()
    {
        print("현재 쓰레드 : "+Thread.CurrentThread.ManagedThreadId);
        //현재 쓰레드 Id 표시, 1이라면 메인 쓰레드
        
        print("현재 프레임 : "+Time.frameCount);
        await TestAsync();
        print("현재 프레임 : "+Time.frameCount);
    }

    //void로 해도 댄다. 그럼 async안붙여도 된다!
    async Task TestAsync()
    {
        //await써야 기다려진다!
        await Task.Run(() =>
        {
            print("작업 했음");
            print("현재 쓰레드 : "+Thread.CurrentThread.ManagedThreadId);
            //다른 쓰레드에서 작업함
        });
    }
}
