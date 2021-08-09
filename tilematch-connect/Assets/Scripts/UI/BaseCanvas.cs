using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class BaseCanvas : MonoBehaviour
{
    public ScreenController.ResolutionType resolutionType;
    public UICenter.CanvasType canvasType;
    [HideInInspector]
    public Canvas canvas;
    private CanvasScaler _canvasScaler;

    private void Awake()
    {
        _canvasScaler = gameObject.GetComponent<CanvasScaler>();
        canvas = gameObject.GetComponent<Canvas>();
    }
    /// <summary>
    /// 스크린 사이즈 비율에 맞는 캔버스 스케일러의 가로 세로 매칭
    /// </summary>
    /// <param name="_value"></param>
    public void SetCanvasScale(float _value)
    {
        if (this.gameObject.activeInHierarchy)
            _canvasScaler.matchWidthOrHeight = _value;
    }
}
