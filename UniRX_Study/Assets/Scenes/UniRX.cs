using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx; //이거있어야 UniRx사용가능
using UniRx.Triggers; //이거있어야 Trigger사용가능
using UnityEngine.UI;

public class UniRX : MonoBehaviour
{
    void Start()
    {
        //SubjectSample();
        //ReactivePropSample();
        //ObservableSample();
        //CoroutineRxSample();
        UGUIRxSample();
        StopSubscribeSample();
        WhereSelectSample();
        SelectMany();
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
        rp.Subscribe(x => Debug.Log(x + "로 값이 변경되었어요!")).AddTo(gameObject);
        //이 게임오브젝트가 파괴되면 Dispose를 호출해 자동으로 구독 중단
        rp.Value = 10;
    }

    //Update대용으로 사용할 함수
    void ObservableSample()
    {
        //this.LateUpdateAsObservable
        //this.FixedUpdateAsObservable
        //this.OnTriggerEnter2DAsObservable
        //위처럼 Observable은 다양한 타이밍이 존재한다.

        //this.UpdateAsObservable().Subscribe(_ =>
        //Debug.Log("Update실행!")
        //);

        this.OnTriggerEnter2DAsObservable().Subscribe(col =>
        print("충돌한 오브젝트의 이름 : " + col.name)
        );
    }

    //코루틴이랑 같이쓰기
    void CoroutineRxSample()
    {
        //Coroutine 함수를 Observable 객체로 변환
        Observable.FromCoroutine<int>(observer => GameTimerCoroutine(observer, 10))
            .Subscribe(t => Debug.Log(t == 0 ? "끗!" : "카운트다운 " + t + "!"));
    }
    IEnumerator GameTimerCoroutine(System.IObserver<int> observer, int initialCount)
    {
        var count = initialCount; //100으로 카운트 초기화
        while (count > 0)
        {
            observer.OnNext(count--); //1초마다 1씩 감소시켜서 보냄
            yield return new WaitForSeconds(1);
        }
        observer.OnNext(0); //끗
    }


    //UGUI랑 같이쓰기
    void UGUIRxSample()
    {
        var button = GameObject.Find("버튼").GetComponent<Button>();
        button.OnClickAsObservable().Subscribe(x =>
        print("버튼이 눌렸어요!!!")); //Button의 OnClick에 해당

        //var slider = GetComponent<Slider>();
        //slider.OnValueChangedAsObservable().Subscribe(); //Slider의 OnValueChanged()에 해당
    }

    //구독 중단하는 
    void StopSubscribeSample()
    {
        var subject = new Subject<int>();

        var stream1 = subject.Subscribe(x => Debug.Log("stream 1 : " + x),
            () => Debug.Log("Stream1 Complete!")); //Oncompleted를 호출할 수 있다.

        var stream2 = subject.Subscribe(x => Debug.Log("stream 2 : " + x),
            () => Debug.Log("Stream2 Complete!"));
        subject.OnNext(1);
        //subject.OnCompleted(); //구독 완전 중단
        stream1.Dispose(); //스트림1만 중단, 2는 살아잇음  
        subject.OnNext(2);
    }

    //Where은 발동조건 정해주는거라보면 댐, Select는 값걸러내는거라보면댐
    void WhereSelectSample()
    {
        //클릭중이면 마우스좌표 표
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButton(0)) //Where로 마우스 클릭시만 걸러냄
            .Select(_ => Input.mousePosition) //Select로 기존 스트림에서 값 바꾸는 처리
            .Subscribe(_vector => print(_vector)); //Vector3형으로 출력

        //그 외 여러 오퍼레이터들 - https://skuld2000.tistory.com/48
    }

    void SelectMany()
    {
        var button = GameObject.Find("버튼2").GetComponent<Button>();
        button.OnPointerDownAsObservable() //마우스 눌렀을 때 실행
            .SelectMany(_ => button.UpdateAsObservable()) //이 순간 스트림을 매프레임 관찰 스트림으로 대체!
            .TakeUntil(button.OnPointerUpAsObservable()) //버튼에서 뗄때까지 계속관찰
            .Subscribe(_ =>
            {
                print("버튼 누르는 중...");
            });
    }
}
