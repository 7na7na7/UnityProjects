using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{

    private ScoreCounter score_counter = null;

    public enum STEP
    {
        NONE = -1, //상태 정보 없음
        PLAY = 0, //플레이 중
        CLEAR, //클리어
        NUM, //상태의 종류가 몇 개인지 나타냄(= 2)
    };

    public STEP step = STEP.NONE; //현재 상태
    public STEP next_step = STEP.NONE; //다음 상태
    public float step_timer = 0.0f; //경과 시간
    private float clear_time = 0.0f; //클리어 시간
    public GUIStyle guistyle; //폰트 스타일
    
    private BlockRoot block_root = null;
    void Start()
    {
        //BlockRoot스크립트를 가져온다.
        block_root = gameObject.GetComponent<BlockRoot>();
        //BlockRoot스크립트의 initialSetUp()을 호출한다.
        block_root.initialSetUp();
        
        //ScoreCounter 가져오기
        score_counter = gameObject.GetComponent<ScoreCounter>();
        next_step = STEP.PLAY; //다음 상태를 '플레이 중'으로
        guistyle.fontSize = 24; //폰트 크기를 24로
    }

    private void Update()
    {
        
    }
}
