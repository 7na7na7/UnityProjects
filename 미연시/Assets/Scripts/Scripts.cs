using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts : MonoBehaviour
{
    public int para1=0, para2=0, para3=0;
    private NPCsenteces cat;
    private DialogueManager dialogue;

    private void Awake()
    {
        cat = FindObjectOfType<NPCsenteces>();
        dialogue = FindObjectOfType<DialogueManager>();
    }

    public void exit()
    {
        Application.Quit();
    }

    public void choice1()
    {
        para1++;
        choice();
        dialogue.Ondialogue(cat.choicesen[0].sentences);
    }
    public void choice2()
    {
        para2++;
        choice();
        dialogue.Ondialogue(cat.choicesen[1].sentences);
    }
    public void choice3()
    {
        para3++;
        choice();
        dialogue.Ondialogue(cat.choicesen[2].sentences);
    }
    void choice()
    {
        dialogue.choicepanel.SetActive(false);
        dialogue.ischoice = false;
        dialogue.NextSentence();
    }
}
