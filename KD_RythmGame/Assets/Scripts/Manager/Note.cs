using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400;
    
    private UnityEngine.UI.Image noteImage;

    private void OnEnable()
    {
        if(noteImage==null) 
            noteImage = GetComponent<UnityEngine.UI.Image>();
        noteImage.enabled = true;
    }

    void Update()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        noteImage.enabled = false;
    }

    public bool GetNoteFlag()
    {
        return noteImage.enabled;
    }
}
