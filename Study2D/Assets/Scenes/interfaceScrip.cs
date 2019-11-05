using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class A : MonoBehaviour //추상 클래스 A선언
{
    abstract public void Abc(); //추상 함수 Abc선언
}

interface ITest //인터페이스 만듬, 인터페이스는 뼈대만 제공하기 때문에 다중 상속 가능, 또한 인터페이스끼리 상속도 가능
{
    void Bbc();
    //int a; //오류남
    //함수, 프로퍼티, 인덱서, 이벤트 등만 뼈대만(기능구현 X)구성한 상태로 선언 가능
    
    int minguProp { get; set; }
}
public class interfaceScrip : A,ITest //클래스는 두 개 상속받을 수 없지만, 인터페이스는 다중 상속 가능
{
    public override void Abc() //추상 함수이므로 재정의
    {
        print("abc");
    }

    public void Bbc() //뼈대밖에 없는 인터페이스이므로 재정의해줌
    {
        print("bbc");
    }

    public int minguProp //인터페이스에서 프로퍼티 사용
    { get; set; }
}
