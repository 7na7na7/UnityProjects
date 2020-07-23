using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    private double currentTime = 0d;

    [SerializeField] private Transform tfNoteAppear=null;
    private TimingManager theTimingManager;
    private EffectManager theEffectManager;
    private ComboManager theCombo;
    private void Start()
    {
        theCombo = FindObjectOfType<ComboManager>();
        theTimingManager = GetComponent<TimingManager>();
        theEffectManager = FindObjectOfType<EffectManager>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm)
        {
            GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
            t_note.transform.position = tfNoteAppear.position;
            t_note.SetActive(true);
            theTimingManager.boxNoteList.Add(t_note);
            currentTime -= 60d / bpm;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            if (other.GetComponent<Note>().GetNoteFlag())
            {
                theEffectManager.JudgementEffect(4);
                theCombo.ResetCombo();
            }
            theTimingManager.boxNoteList.Remove(other.gameObject);
            ObjectPool.instance.noteQueue.Enqueue(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}
