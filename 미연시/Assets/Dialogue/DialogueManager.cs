using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DialogueManager : MonoBehaviour, IPointerDownHandler
{
    public SpriteRenderer BG;
    public Sprite living_room;
    public Sprite amusement_park;
    
    public GameObject choicepanel; //선택지 창
    public Text choice1;
    public Text choice2;
    public Text choice3;
    
    public bool ischoice = false; //선택지가 활성화된 상태인지
    
    public AudioSource audio;
    private NPCsenteces cat;
    public int sentencevalue = 0;
    
    public Text dialogueText;
    public GameObject nextText;
    public Queue<string> sentences;
    private string currentSentence;
    public float typeSpeed = 0.1f;
    private bool istyping = false;
    public CanvasGroup dialoguegroup;
    public static DialogueManager instance;
    
    void Start()
    {
        BG.sprite = living_room;
        cat = FindObjectOfType<NPCsenteces>();
     sentences=new Queue<string>();
    }

    private void Awake()
    {
        instance = this;
    }

    public void Ondialogue(string[] lines)
    {
        sentences.Clear();
        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        dialoguegroup.alpha = 1; //다이얼로그 창이 보이도록 설정
        dialoguegroup.blocksRaycasts = true; //blockRaycasts가 true일 때만 마우스이벤트 감지가능
        NextSentence();
    }

    public void NextSentence()
    {
        if (!ischoice)
        {
            if (sentences.Count != 0)
            {
                currentSentence = sentences.Dequeue(); //문장에서 디큐를 반환함
                //코루틴
                istyping = true;
                nextText.SetActive(false);
                if (cat.choiceing)
                {
                    cat.GetComponent<SpriteRenderer>().sprite = cat.choiceDial[Scripts.instance.choicevalue_2]
                            .choiceD[Scripts.instance.choicevalue].spr[sentencevalue];
                        audio.PlayOneShot(cat.choiceDial[Scripts.instance.choicevalue_2]
                            .choiceD[Scripts.instance.choicevalue].sound[sentencevalue]); //효과음

                }
                else
                {
                    if (cat.isevent)
                    {
                        if (cat.eventsen[cat.eventvalue].canchoice[sentencevalue]) //canchoice가 true가면
                        {
                            Scripts.instance.rect();
                            choicepanel.SetActive(true);
                            choice1.text = cat.eventsen[cat.eventvalue].choicesen[0];
                            choice2.text = cat.eventsen[cat.eventvalue].choicesen[1];
                            choice3.text = cat.eventsen[cat.eventvalue].choicesen[2];
                            ischoice = true;
                        }
                        cat.GetComponent<SpriteRenderer>().sprite = cat.eventsen[cat.eventvalue].spr[sentencevalue]; //스프라이트 교체
                        audio.PlayOneShot(cat.eventsen[cat.eventvalue].sound[sentencevalue]); //효과음
                    }
                    else
                    {
                        if (cat.sen[cat.i].canchoice[sentencevalue]) //canchoice가 true가면
                        {
                            Scripts.instance.rect();
                            choicepanel.SetActive(true);
                            choice1.text = cat.sen[cat.i].choicesen[0];
                            choice2.text = cat.sen[cat.i].choicesen[1];
                            choice3.text = cat.sen[cat.i].choicesen[2];
                            ischoice = true;
                        }

                        cat.GetComponent<SpriteRenderer>().sprite = cat.sen[cat.i].spr[sentencevalue]; //스프라이트 교체
                        audio.PlayOneShot(cat.sen[cat.i].sound[sentencevalue]); //효과음
                    }
                }
                StartCoroutine(Typing(currentSentence));
                sentencevalue++;   
            }
            else //큐의 카운트가 0이 되면
            {
                dialoguegroup.alpha = 0; //다이얼로그 창이 안 보이도록 설정
                dialoguegroup.blocksRaycasts = false; //마우스이벤트 감지불가
                sentencevalue = 0;
                cat.isend = true;
                cat.GetComponent<SpriteRenderer>().sprite = cat.idlesprite;
                cat.choiceing = false;
            }
        }
    }

   IEnumerator Typing(string line) //타이핑하는 듯한 효과
    {
        if(line=="여기가 놀이동산이구냥!") //놀이공원으로 전환
        {
            Instantiate(cat.fade, cat.parent.transform);
            BG.sprite = amusement_park;
        }
        dialogueText.text = "";
            foreach (char letter in line.ToCharArray()) //line의 글자 하나하나를 letter에 넣어주면서 반복문
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typeSpeed);
            }
    }

    private void Update()
    {
        //dialogueText == currentSentence이면 대사 한줄 끝
        if (dialogueText.text.Equals(currentSentence))
        {
            istyping = false;
            nextText.SetActive(true);
        }
        print(cat.isevent);
    }

    //OnPointerDown은 해당 오브젝트에 클릭, 터치가 있을 때 호출딤
    public void OnPointerDown(PointerEventData eventData) 
    {
        if (!istyping)
        {
            NextSentence();
        }
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(!istyping) 
                NextSentence();
        }
            
    }
}
