using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public struct Count //점수 관리용 구조체, 연쇄 수와 점수, 합계 점수가 들어가 있다.
    {
        public int ignite; //연쇄 수
        public int score; //점수
        public int total_score; //함계 점수
    };

    public Count last; //마지막(이번) 점수
    public Count best; //최고 점수

    public static int QUOTA_SCORE = 1000; //클리어에 필요한 점수
    public GUIStyle guistlye; //폰트 스타일

    private void Start()
    {
        last.ignite = 0;
        last.score = 0;
        last.total_score = 0;
        guistlye.fontSize = 16;
    }

    private void OnGUI()
    {
        int x = 20;
        int y = 50;
        GUI.color = Color.black;
        print_value(x + 20, y, "연쇄 수", last.ignite);
        y += 30;
        print_value(x + 20, y, "가산 점수", last.score);
        y += 30;
        print_value(x + 20, y, "합계 점수", last.total_score);
        y += 30;
    }

    public void print_value(int x, int y, string label, int value) //지정된 두 개의 데이터를 두 행으로 나누어 표시한다.
    {
        //label을 표시
        GUI.Label(new Rect(x,y,100,20),label, guistlye);
        y += 15;
        //다음 행에 value를 표시
        GUI.Label(new Rect(x+20,y,100,20),value.ToString(),guistlye);
        y += 15;
    }

    public void addIgniteCount(int count) //연쇄 정보를 가산한다.
    {
        last.ignite += count; //연쇄 후에 count를 합산
        update_score(); //점수 계산
    }

    public void clearIgniteCount() //연쇄 횟수를 리셋한다.
    {
        last.ignite = 0; //0으로 초기화
    }

    public void update_score() //더해야 할 점수를 게산한다.
    {
        last.score = last.ignite * 10; //점수 갱신
    }

    public void updateTotalscore() //합계 점수를 갱신한다.
    {
        last.total_score += last.score; //합계 점수 갱신
    }

    public bool isGameClear() //게임을 클리어했는지 판정한다.(SceneControl에서 사용)
    {
        bool is_clear = false;
        //현재 합계 점수가 클리어 기준보다 크면,
        if (last.total_score > QUOTA_SCORE)
            is_clear = true;

        return (is_clear);
    }
    
}
