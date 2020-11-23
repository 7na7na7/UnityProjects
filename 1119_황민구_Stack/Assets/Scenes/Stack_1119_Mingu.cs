using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stack_1119_Mingu : MonoBehaviour
{
    public Text stacktext; //스택을 표시할 텍스트 선언
    public Text errortext; //에러 표시할 텍스트 선언
    public Text outputtext; //출력값 표시할 텍스트 선언
    public Text inputtext; //입력값 표시할 텍스트 선언
    public InputField input; //값을 받을 입력창
    private Stack<int> stack = new Stack<int>(); //int형 스택 선언

    
    void Start()
    {
        Screen.SetResolution(1920,1080,true);
        //처음에 입력창으로 포인터 가져다대도록
        input.Select(); 
        input.ActivateInputField();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) //입력했을 때
        {
            if (int.Parse(input.text) != 9999)//만약 9999을 입력했을 때
            {
                if (int.Parse(input.text) == 8888) //만약 8888을 입력했을 때
                {
                    Application.Quit(); //프로그램 종료
                }
                else
                {
                    if (stack.Count >= 10)//스택이 꽉 찬 상태라면
                    {
                        errortext.text = "스택 오버플로우!!!!\n으아아아악!!!";
                        StartCoroutine(overflow()); //오버플로우 실행
                    }
                    else
                    {
                        stack.Push(int.Parse(input.text)); //푸시!
                        foreach (int i in stack) //스택 요소값 표시
                        {
                            stacktext.text =  stacktext.text +" "+i; 
                            break;
                        }
                    }
                }
               
            }
            else
            {
                if (stack.Count <= 0) //스택이 비었다면
                {
                    errortext.text = "스택 언더플로우!!!!\n으아아아악!!!";
                    StartCoroutine(underflow()); //언더플로우 실행
                }
                else
                {
                    outputtext.text = outputtext.text+" "+stack.Peek(); //하나 꺼내서 값 출력
                    stack.Pop(); //팝!
                    foreach (int i in stack) //스택 요소값 표시
                    {
                        stacktext.text =  stacktext.text +" "+i; 
                        break;
                    }
                }
            }
            //입력한 후 포인터 다시 입력창으로
            input.Select();
            input.ActivateInputField();
            
            inputtext.text = "input : "+input.text; //입력한 값 표시
            
            input.text = ""; //입력할 창 공백으로 초기화
        }
    }

    IEnumerator overflow() //오버플로우
    {
        yield return new WaitForSeconds(1f); //대기시간
        errortext.text = "스택이 터졌으므로\n프로그램이 3초 후 폭발합니다.";
        yield return new WaitForSeconds(1f);
        errortext.text = "스택이 터졌으므로\n프로그램이 2초 후 폭발합니다.";
        yield return new WaitForSeconds(1f);
        errortext.text = "스택이 터졌으므로\n프로그램이 1초 후 폭발합니다.";
        yield return new WaitForSeconds(1f);
        errortext.text = "퍼ㅓㅓㅓㅓㅓㅓㅓㅓㅓㅇ!";
        Application.Quit(); //종료
    }
    IEnumerator underflow() //언더플로우 코루틴
    {
        yield return new WaitForSeconds(1f); //대기시간
        errortext.text = "스택 언더플로우로\n프로그램이 3초 후 폭발합니다.";
        yield return new WaitForSeconds(1f);
        errortext.text = "스택 언더플로우로\n프로그램이 2초 후 폭발합니다.";
        yield return new WaitForSeconds(1f);
        errortext.text = "스택 언더플로우로\n프로그램이 1초 후 폭발합니다.";
        yield return new WaitForSeconds(1f);
        errortext.text = "퍼ㅓㅓㅓㅓㅓㅓㅓㅓㅓㅇ!";
        Application.Quit(); //종료
    }
}
