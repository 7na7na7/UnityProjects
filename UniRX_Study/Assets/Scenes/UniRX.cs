using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UniRX : MonoBehaviour
{
    void Start()
    {
        //SubjectSample();
        ReactivePropSample();
    }


    //Subject 사용법
    void SubjectSample()
    {
        //Subject<T> 형태로 원하는 자료형을 Subject 타입으로 생성하면 스트림을 사용할 수 있다!
        //Subject 타입의 string 객체 생성
        Subject<string> subject = new Subject<string>();

        //메세지 발생 시 3회 출력한다.
        subject.Subscribe(msg => Debug.Log("Subscribe 첫번째 : " + msg));
        subject.Subscribe(msg => Debug.Log("Subscribe 두번째 : " + msg));
        subject.Subscribe(msg => Debug.Log("Subscribe ㅍ번째 : " + msg));

        //이벤트 메시지 전달
        subject.OnNext("Hello");
        subject.OnNext("Hi");
        //결과 : Hello 3번출력후 Hi 3번출
    }

    //ReactiveProperty 사용법
    void ReactivePropSample()
    {
        var rp = new ReactiveProperty<int>(10); //초기 값 지정 가능

        //.Value를 사용하면 일반 변수처럼 대입 또는 값 읽기가 가능해진다.
        rp.Value = 5; //값 수정 시 즉시 OnNext가 발생하며 Subscribe로 통보
        var currentValue = rp.Value; //5

        //Subscribe 사용가능!
        rp.Subscribe(x => Debug.Log(x + "로 값이 변경되었어요!")); //구독 설정
        rp.Value = 10;
    }
}
