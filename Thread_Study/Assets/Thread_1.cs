using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Thread_1 : MonoBehaviour
{
    private void Start()
    {
        init();
    }

    void init()
    {
        // 파라미터 없는 ThreadStart 사용
        Thread t1 = new Thread(new ThreadStart(Run));
        t1.Start();

        // ParameterizedThreadStart 파라미터 전달
        // Start()의 파라미터로 radius 전달
        Thread t2 = new Thread(new ParameterizedThreadStart(Calc));
        t2.Start(10.00);

        // ThreadStart에서 파라미터 전달
        Thread t3 = new Thread(() => Sum(10, 20, 30));
        t3.Start();
    }

    void Run()
    {
        print("Run");
    }

    // radius라는 파라미터를 object 타입으로 받아들임
    void Calc(object radius)
    {
        double r = (double)radius;
        double area = r * r * 3.14;
        print(string.Format("r={0},area={1}", r, area));
    }

    void Sum(int d1, int d2, int d3)
    {
        int sum = d1 + d2 + d3;
        print(sum);
    }
}
