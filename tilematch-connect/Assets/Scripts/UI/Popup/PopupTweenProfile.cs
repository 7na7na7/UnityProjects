using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[CreateAssetMenu(fileName = "PopupTween", menuName = "TweenProfile/Popup", order = 0)]
public class PopupTweenProfile : ScriptableObject
{
    public float startTime = 0.3f;
    public float closeTime = 0.15f;
    public float punchTime = 0.2f;
    public float punchJust = 0.09f;


    public Vector3 startPunchScale = new Vector3(-0.12f, 0.18f, 0.0f);

    public float startPosY = 800f;
    public float closePosY = 500f;
    public Ease startEase = Ease.InOutSine;
    public Ease closeEase = Ease.OutSine;
}
