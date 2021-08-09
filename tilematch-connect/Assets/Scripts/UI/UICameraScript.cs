using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraScript : MonoBehaviour
{
    [SerializeField] Transform canvasParent;

    //public TranslucentImageSource translucentImage;

    Dictionary<UICenter.CanvasType, Canvas> canvasContainer = new Dictionary<UICenter.CanvasType, Canvas>();


    private void Start()
    {
        for (int i = 0; i < canvasParent.childCount; i++)
        {
            BaseCanvas _baseCanvas = canvasParent.GetChild(i).GetComponent<BaseCanvas>();
            var _canvas = _baseCanvas.GetComponent<Canvas>();

            canvasContainer[_baseCanvas.canvasType] = _canvas;

            UICenter.Instance.screenController.SetCanvasResolution(_baseCanvas);

        }
    }


    public Canvas GetCanvas(UICenter.CanvasType _type)
    {
        return canvasContainer[_type];
    }
}
