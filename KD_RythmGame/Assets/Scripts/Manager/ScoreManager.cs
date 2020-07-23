using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text txtScore = null;
    public int increaseScore = 10;
    public float[] weight = null;
    private int currentScore = 0;
    public int comboBonusScore;
    private ComboManager theCombo;
    void Start()
    {
        theCombo = FindObjectOfType<ComboManager>();
        currentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        //콤보 증가
        theCombo.IncreaseCombo();

        int t_currentCombo = theCombo.GetCurrentCombo();
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;

        //가중치 계산
        int t_increaseScore = increaseScore+t_bonusComboScore;
        t_increaseScore = (int) (t_increaseScore * weight[p_JudgementState]);

        //점수 반영
        currentScore += t_increaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore);
    }
}
