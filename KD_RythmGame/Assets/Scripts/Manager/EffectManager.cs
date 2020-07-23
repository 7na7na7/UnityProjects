using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private Animator noteHitAnimator = null;
    private string hit = "hit";
    
    [SerializeField] private Animator judgementAnimator = null;
    [SerializeField] private UnityEngine.UI.Image judgementImage = null;
    [SerializeField] private Sprite[] judgementSprites = null;

    public void JudgementEffect(int p_n)
    {
        judgementImage.sprite = judgementSprites[p_n];
        judgementAnimator.SetTrigger(hit);
    }
    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }
}
