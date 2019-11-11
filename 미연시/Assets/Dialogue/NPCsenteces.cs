using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class dialogue
{
    public int choicevalue;
    public bool[] canchoice;
    public string[] choicesen;
    public string[] sentences;
    public Sprite[] spr;
    public AudioClip[] sound;
}

[System.Serializable]
public class choicedialogue
{
    public dialogue[] choiceD;
}
public class NPCsenteces : MonoBehaviour
{
    public bool isevent = false; //이벤트가 발생했는가? 발생했다면 이벤트실행, 아니면 그냥문장실행
    public bool isend = false; //끝났는가? 끝날때까지 이벤트발생값을 올리지 않음

    public GameObject parent; //fade의 부모를 canvas로 해주기 위해서
    public GameObject fade; //페이드를 위한 게임오브젝트
    public int[] minimumvalue;
    private int minimum = 0, maximum = 0; //최소 문장, 최대 문장
    public int eventvalue = 0; //이벤트발생변수
    public int eventcurrency = 100; //이벤트발생빈도
    public Text heart; //호감도
    public Sprite idlesprite; //기본 스프라이트
    
    
    public dialogue[] sen;
    public dialogue[] eventsen;
    public choicedialogue[] choiceDial;

    public bool choiceing = false;
    
    public int i=0;
    
    private int heartvalue = 0;
    private int[] previousvalue= new int[10];
    private int previousnum = 0;
    IEnumerator Start()
    {
        previousvalue[0] = 1000;
        previousvalue[1] = 1000;
        previousvalue[2] = 1000;
        previousvalue[3] = 1000;
        previousvalue[4] = 1000;
        previousvalue[5] = 1000;
        previousvalue[6] = 1000;
        previousvalue[7] = 1000;
        previousvalue[8] = 1000;
        previousvalue[9] = 1000;
        isend = false;
        isevent = true;
        Instantiate(fade, parent.transform);
        DialogueManager.instance.Ondialogue(eventsen[eventvalue].sentences);
        yield return new WaitUntil(()=>isend==true);
        minimum += minimumvalue[eventvalue];
        maximum += minimumvalue[eventvalue + 1];
        eventvalue++;
        isend = false;
        isevent = false;
    }

    private IEnumerator OnMouseDown()
    {
        if (DialogueManager.instance.dialoguegroup.alpha == 0)
        {
            if (heartvalue / eventcurrency >= eventvalue)
            {
                isend = false;
                isevent = true;
                    Instantiate(fade, parent.transform);
                    DialogueManager.instance.Ondialogue(eventsen[eventvalue].sentences);
                    yield return new WaitUntil(()=>isend==true);
                    minimum += minimumvalue[eventvalue];
                    maximum += minimumvalue[eventvalue + 1];
                    eventvalue++;
                    isend = false;
                    isevent = false;
            }
                else
                {
                    while (true) //앞의 10문장과 일치하는 문장을 말하지 않도록
                    {
                        i = Random.Range(minimum, maximum);
                        if (i != previousvalue[0] && i != previousvalue[1] && i != previousvalue[2] && i != previousvalue[3] && i != previousvalue[4] 
                            && i != previousvalue[5] && i != previousvalue[6]&& i != previousvalue[7]&& i != previousvalue[8]&& i != previousvalue[9])
                            break;
                    }

                    previousvalue[previousnum] = i;
                    
                    if (previousnum != 9)
                        previousnum++;
                    else
                        previousnum = 0;
                    
                    DialogueManager.instance.Ondialogue(sen[i].sentences);
                    yield return new WaitUntil(()=>isend==true);
                    heartvalue += sen[i].sentences.Length*2; //호감도증가
                }
                heart.text = heartvalue.ToString();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            {
                OnMouseDown();
            }
        }
    }
}
