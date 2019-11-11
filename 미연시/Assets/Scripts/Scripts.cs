using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Scripts : MonoBehaviour
{
    public int choicevalue;
    public int choicevalue_2;
    
    public static Scripts instance;
    public GameObject[] choicewindow;
    public int like, hate;
    private NPCsenteces cat;
    private DialogueManager dialogue;
    private int i,j;
    private Vector2 min, max;
    private void Awake()
    {
        like = 0;
        hate = 0;
        instance = this;
        cat = FindObjectOfType<NPCsenteces>();
        dialogue = FindObjectOfType<DialogueManager>();
        
        //처음값 저장
        min=choicewindow[0].GetComponent<RectTransform>().offsetMin;
        max = choicewindow[0].GetComponent<RectTransform>().offsetMax;

        rect();
    }

    public void exit()
    {
        Application.Quit();
    }
    
    public void choiceFunc(int _choicevalue)
    {
        choicevalue = _choicevalue;
        choice();
        if (cat.isevent) //이벤트 중에서
        {
            choicevalue_2 = cat.eventsen[cat.eventvalue].choicevalue - 1;
            if(cat.eventsen[cat.eventvalue].choicevalue - 1>=0) 
                dialogue.Ondialogue(cat.choiceDial[cat.eventsen[cat.eventvalue].choicevalue - 1].choiceD[choicevalue]
                .sentences);
        }
        else
        {
            choicevalue_2 = cat.sen[cat.i].choicevalue - 1;
            if(cat.sen[cat.i].choicevalue - 1>=0) 
                dialogue.Ondialogue(cat.choiceDial[cat.sen[cat.i].choicevalue - 1].choiceD[choicevalue].sentences);
        }

        cat.choiceing = true;
    }

    void choice()
    {
        dialogue.choicepanel.SetActive(false);
        dialogue.ischoice = false;
        dialogue.NextSentence();
    }

    public void rect()
    {
        //처음값으로 조정
        choicewindow[0].GetComponent<RectTransform>().offsetMin = min;
        choicewindow[0].GetComponent<RectTransform>().offsetMax = max;
        choicewindow[1].GetComponent<RectTransform>().offsetMin = min;
        choicewindow[1].GetComponent<RectTransform>().offsetMax = max;
        choicewindow[2].GetComponent<RectTransform>().offsetMin = min;
        choicewindow[2].GetComponent<RectTransform>().offsetMax = max;
        
        //i,j중복 안되게 하기
        i = UnityEngine.Random.Range(0, 3);
        if (i == 0)
            j = UnityEngine.Random.Range(1, 3);
        else if (i == 1)
        {
            j = UnityEngine.Random.Range(0, 1);
            if (j == 0)
            {
            }
            else
                j = 2;
        }
        else
            j = UnityEngine.Random.Range(0, 2);

        choicewindow[i].GetComponent<RectTransform>().offsetMin = new Vector2(choicewindow[i].GetComponent<RectTransform>().offsetMin.x, choicewindow[i].GetComponent<RectTransform>().offsetMax.y-250); //Bottom을 250줄임
        choicewindow[i].GetComponent<RectTransform>().offsetMax = new Vector2(choicewindow[i].GetComponent<RectTransform>().offsetMax.x, choicewindow[i].GetComponent<RectTransform>().offsetMax.y-250); //Top을 250늘림
        choicewindow[j].GetComponent<RectTransform>().offsetMin = new Vector2(choicewindow[j].GetComponent<RectTransform>().offsetMin.x, choicewindow[j].GetComponent<RectTransform>().offsetMax.y+250); //Bottom을 250줄임
        choicewindow[j].GetComponent<RectTransform>().offsetMax = new Vector2(choicewindow[j].GetComponent<RectTransform>().offsetMax.x, choicewindow[j].GetComponent<RectTransform>().offsetMax.y+250); //Top을 250늘림
    }
}
