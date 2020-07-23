using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
  public List<GameObject> boxNoteList=new List<GameObject>();

  [SerializeField] private Transform Center = null;
  [SerializeField] private RectTransform[] timingRect = null;
  private Vector2[] timingBoxs = null;

  private EffectManager theEffect;
  private ScoreManager theScore;
  private ComboManager theCombo;
  private void Start()
  {
    theCombo = FindObjectOfType<ComboManager>();
    theEffect = FindObjectOfType<EffectManager>();
    theScore = FindObjectOfType<ScoreManager>();
    timingBoxs=new Vector2[timingRect.Length];

    for (int i = 0; i < timingRect.Length; i++)
    {
      timingBoxs[i].Set(Center.localPosition.x-timingRect[i].rect.width/2,
        Center.localPosition.x+timingRect[i].rect.width/2);
    }
  }

  public void CheckTiming()
  {
    for (int i = 0; i < boxNoteList.Count; i++)
    {
      float t_notePosX = boxNoteList[i].transform.localPosition.x;

      for (int x = 0; x < timingBoxs.Length; x++)
      {
        if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
        {
          //노트 제거
          boxNoteList[i].GetComponent<Note>().HideNote();
          boxNoteList.Remove(boxNoteList[i]);
          //이펙트 연출
          if(x<timingBoxs.Length-1) 
            theEffect.NoteHitEffect();
          theEffect.JudgementEffect(x);
          //점수 증가
          theScore.IncreaseScore(x);
          return;
        }
      }
    } 
    theCombo.ResetCombo(); //초기화
    theEffect.JudgementEffect(4);
  }
}
