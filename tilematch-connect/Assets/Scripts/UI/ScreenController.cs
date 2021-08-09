using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;

public class ScreenController : MonoBehaviour
{
    /// <summary>
    /// 스크린 사이즈 종횡비
    /// </summary>
    Func<Vector2, float> GetAspectRatio = screen => screen.x / screen.y; // 1.77 (1920x1080 가로모드 종횡비) --> 가로가 더 길 때  커짐

    [SerializeField]
    Vector2 _screenRatio = new Vector2(720f, 1280f); // 참조 종회비

    //  new Vector2(1080f, 1920f); // 0.56 (세로모드 종횡비)  --> 세로가 더 길 때  작아짐 (가로가 더 길 때 커짐) 


    public enum ResolutionType
    {
        Matched,
        HalfFix,
    }

    /// <summary>
    /// 유아이 캔버스의 스크린 종횡비 세팅
    /// </summary>
    /// <param name="_canvas"></param>
    public void SetCanvasResolution(BaseCanvas _baseCanvas)
    {
        switch (_baseCanvas.resolutionType)
        {
            case ResolutionType.Matched:

                // 즉 기계 스크린 종횡비의 값이 참조한 스크린 종횡비보다 크다면 (모바일 화면이 길쭉하다면)
                bool isLong = GetAspectRatio(new Vector2(Screen.width, Screen.height)) > GetAspectRatio(_screenRatio);
                _baseCanvas.SetCanvasScale(isLong ? 1.0f : 0.0f);  //Canvas Match 값 1 에 맞추고(즉 변하지 않는 height에 맞춘다라고 생각하자 )



                //   세로모드 -> 기준보다 더 세로로 길어지면 -->  결국 위 계산이랑 똑같음 필요없음
                //   isLong = GetAspectRatio(new Vector2(Screen.width, Screen.height)) < GetAspectRatio(_screenRatio);
                //  _baseCanvas.SetCanvasScale(isLong ? 0.0f : 1.0f);
                break;


            case ResolutionType.HalfFix:
                _baseCanvas.SetCanvasScale(0.5f);
                break;
        }
    }

    /// <summary>
    /// 설정된 기본 디스플레이 사이즈를 참조하여 기기별 해상도 대응을 위한 "메인" 카메라 프로젝션 사이즈 조정값 반환 
    /// </summary>
    /// <param name="_value">설정된 카메라 프로젝션 사이즈</param>
    /// <returns></returns>
    public float GetCameraOffset(float _value)
    {
        float defaultValue = GetAspectRatio(_screenRatio); //참조 종횡비
        float currentValue = GetAspectRatio(new Vector2(Screen.width, Screen.height)); // 디스플레이 종횡비
        float resultOffset = (defaultValue / currentValue) * _value;
        return resultOffset > _value ? resultOffset : _value;
    }

    /// <summary>
    /// 월드 포지션에서 캔버스 좌표로 위치 이동한 vector 값 반환 (월드 좌표 포지션에 근거한 유아이 데미지 라벨을 캔버스에 나타내기 위해 사용)
    /// </summary>
    /// <param name="_canvas">표시할 캔버스</param>
    /// <param name="_position">월드 좌표</param>
    /// <param name="_camera">메인 카메라</param>
    /// <returns></returns>
    public Vector2 WorldToCanvas(Canvas _canvas, Vector3 _position, Camera _camera)
    {
        if (_camera == null) _camera = Camera.main;

        var viewport_position = _camera.WorldToViewportPoint(_position);
        var canvas_rect = _canvas.GetComponent<RectTransform>();

        float canvasPosX = (viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f);
        float canvasPosY = (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f);

        return new Vector2(canvasPosX, canvasPosY);
    }



}
