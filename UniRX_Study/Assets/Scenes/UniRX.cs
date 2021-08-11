using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UniRX : MonoBehaviour
{
    void Start()
    {
        SubjectSample();
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
        subject.Subscribe(msg => Debug.Log("Subscribe 번째 : " + msg));

        //이벤트 메시지 전달
        subject.OnNext("Hello");
        subject.OnNext("Hi");
        //결과 : Hello 3번출력후 Hi 3번출
    }
}
