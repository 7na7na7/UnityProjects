using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    private double currentTime = 0d;

    [SerializeField] private Transform tfNoteAppear=null;
    [SerializeField] private GameObject goNote = null;
    private TimingManager theTimingManager;
    private EffectManager theEffectManager;

    private void Start()
    {
        theTimingManager = GetComponent<TimingManager>();
        theEffectManager = FindObjectOfType<EffectManager>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm)
        {
            //GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
            t_note.transform.SetParent(this.transform);
            theTimingManager.boxNoteList.Add(t_note);
            currentTime -= 60d / bpm;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            if(other.GetComponent<Note>().GetNoteFlag())
                theEffectManager.JudgementEffect(3);
            theTimingManager.boxNoteList.Remove(other.gameObject);
            Destroy(other.gameObject);   
        }
    }
}
