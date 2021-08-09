using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarController : MonoBehaviour
{
    public delegate void addStarGauge(int count);
    public static event addStarGauge AddStarGauge;

    public static StarController Instance;

    public GameObject prefabStar;
    public Transform floatTransform;

    // 좌표 변환용
    public Canvas uiCanvas;
    public Camera ingameCamera;

    public void Init(List<Node> nodes)
    {
        BuiltInUIController builtIn = gameObject.GetComponent<BuiltInUIController>();
        foreach (Node node in nodes)
        {

            GameObject _go = Instantiate(
                                prefabStar,
                                floatTransform
                             );

            // 위치 이동
            //_go.GetComponent<Transform>().SetParent(floatTransform);

            float offsetX = FieldDesignManager.fieldXOffset;
            float offsetY = FieldDesignManager.fieldYOffset;
            // 별 만들기
            // 좌표 변환
            Vector3 starCoords = new Vector3(node.x + offsetX, node.y + offsetY, 0);
            Vector3 convertedVector = WorldToCanvas(uiCanvas, starCoords, ingameCamera);
            //_go.GetComponent<StarController>().transform.position = convertedVector;
            RectTransform _goRectTransform = _go.GetComponent<RectTransform>();
            _goRectTransform.anchoredPosition = convertedVector;
            //_go.transform.position = convertedVector;
            // 생성 전 세팅
            //_go.GetComponent<LineRenderer>().SetPosition(0, new Vector3(node.x + offsetX, node.y + offsetY, 0));
            //_go.GetComponent<LineRenderer>().SetPosition(1, new Vector3(node.ParentNode.x + offsetX, node.ParentNode.y + offsetY, 0));

            // 별 날리기
            _goRectTransform.DOAnchorPos(new Vector3(91, 549), 1f).SetEase(Ease.InOutCirc).OnComplete(() =>
            {
                // 점수 올리기
                AddStarGauge(1);
                Destroy(_go);
            });

            //Destroy(_go, 10.0f); // todo: 이후에 삭제 이펙트 및 모션에 대해서 고민해야 함
        }

    }

    // 좌표 변환
    public Vector3 WorldToCanvas(Canvas _canvas, Vector3 _position, Camera _camera)
    {
        if (_camera == null) _camera = Camera.main;

        var viewport_position = _camera.WorldToViewportPoint(_position);
        var canvas_rect = _canvas.GetComponent<RectTransform>();

        float canvasPosX = (viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f);
        float canvasPosY = (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f);

        return new Vector3(canvasPosX, canvasPosY, 0);
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

}
