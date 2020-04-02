using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public Text bonusText1,bonusText2;
    public float textDelay;
    public int headScore,maxComboScore;
    public Text head, killedOni, maxCombo, score;
    private int headv, killedOniv, scorev, maxcombov;
    private int headc = 0, killedOnic = 0, scorec=0,maxcomboc = 0;
    
    private string highScoreKey1 = "highScoreKey1";
    private string highScoreKey2 = "highScoreKey2";
    private int highScore1;
    private int highScore2;
    private string highComboKey1 = "highComboKey1";
    private string highComboKey2 = "highComboKey2";
    private int highCombo1;
    private int highCombo2;
    public IEnumerator bonus()
    {
        head.text = "급소 공격 횟수 : 0번";
        killedOni.text = "죽인 오니 : 0마리";
        maxCombo.text = "최대 콤보 횟수 : 0번";
        score.text = ScoreMgr.instance.score.ToString();
        scorec = ScoreMgr.instance.score;
        headv = ScoreMgr.instance.headshot;
        killedOniv=ScoreMgr.instance.killedOni;
        scorev = ScoreMgr.instance.score;
        maxcombov = ScoreMgr.instance.maxCombo;
        
        //점수 불러오기
        highScore1 = PlayerPrefs.GetInt(highScoreKey1, 0);
        highScore2 = PlayerPrefs.GetInt(highScoreKey2, 0);

       
        if (SceneManager.GetActiveScene().name == "Main") //스테이지 1
        {
            //점수
            if (ScoreMgr.instance.score + (headv * headScore) + (maxcombov * maxComboScore) > highScore1)
            {
                GooglePlayManager.instance.AddScore1(ScoreMgr.instance.score + (headv * headScore) + (maxcombov * maxComboScore)); //리더보드에 점수추가
                highScore1 = ScoreMgr.instance.score + (headv * headScore) + (maxcombov * maxComboScore);
                PlayerPrefs.SetInt(highScoreKey1,ScoreMgr.instance.score + (headv * headScore) + (maxcombov * maxComboScore));
            }
            //콤보
            highCombo1 = PlayerPrefs.GetInt(highComboKey1, 0);
            if (maxcombov > highCombo1)
            {
                GooglePlayManager.instance.AddCombo1(maxcombov);
                highCombo1 = maxcombov;
                PlayerPrefs.SetInt(highComboKey1,maxcombov);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Main2") //스테이지 2
        {
            //점수
            if (ScoreMgr.instance.score + (headv * headScore) + (maxcombov * maxComboScore) > highScore2)
            {
                GooglePlayManager.instance.AddScore2(ScoreMgr.instance.score + (headv * headScore) + (maxcombov * maxComboScore)); //리더보드에 점수추가
                highScore2 = ScoreMgr.instance.score + (headv * headScore) + (maxcombov * maxComboScore);
                PlayerPrefs.SetInt(highScoreKey2,ScoreMgr.instance.score + (headv * headScore) + (maxcombov * maxComboScore));
            }
            highCombo2 = PlayerPrefs.GetInt(highComboKey2, 0);
            //콤보
            if (maxcombov > highCombo2)
            {
                GooglePlayManager.instance.AddCombo2(maxcombov);
                highCombo2 = maxcombov;
                PlayerPrefs.SetInt(highComboKey2,maxcombov);
            }
        }
       

       

        if (killedOniv >= 50)
        {
            if(SceneManager.GetActiveScene().name=="Main") 
                GooglePlayManager.instance.Achievement4();
            else if(SceneManager.GetActiveScene().name=="Main2") 
                GooglePlayManager.instance.Achievement7();
        }

        if (killedOniv >= 100)
        {
            if(SceneManager.GetActiveScene().name=="Main") 
                GooglePlayManager.instance.Achievement5();
            else if(SceneManager.GetActiveScene().name=="Main2") 
                GooglePlayManager.instance.Achievement8();
        }

        if (killedOniv >= 300)
        {
            if(SceneManager.GetActiveScene().name=="Main") 
                GooglePlayManager.instance.Achievement6();
            else if(SceneManager.GetActiveScene().name=="Main2") 
                GooglePlayManager.instance.Achievement9();
        }

        yield return new WaitForSecondsRealtime(0.3f);
        while (killedOnic<killedOniv)
        {
            yield return new WaitForSecondsRealtime(textDelay);
            if (killedOnic + 1 == killedOniv)
                killedOnic++;
            else
                killedOnic+=2;
            
            if (killedOnic% 3 == 0)
                SoundManager.instance.scoreCount();
                killedOni.text = "죽인 오니 : " + killedOnic + "마리";
        }
        yield return new WaitForSecondsRealtime(0.3f);
        if (headv > 0)
        {
            while (headc < headv)
            {
                yield return new WaitForSecondsRealtime(textDelay);
                if (headc + 1 == headv)
                    headc++;
                else
                    headc+=2;
                if (headc% 3 == 0)
                    SoundManager.instance.scoreCount();
                head.text = "급소 공격 횟수 : " + headc + "번";
            }

            yield return new WaitForSecondsRealtime(0.3f);
        }
        if (maxcombov > 1)
        {
            while (maxcomboc < maxcombov)
            {
                yield return new WaitForSecondsRealtime(textDelay);
                maxcomboc++;
                if (maxcomboc % 3 == 0)
                    SoundManager.instance.scoreCount();
                maxCombo.text = "최대 콤보 횟수 : " + maxcomboc + "번";
            }

            yield return new WaitForSecondsRealtime(0.3f);
        }

        if (headv > 0)
        {
            bonusText1.text = "급소 보너스 + " + headv * headScore;
            while (scorec < ScoreMgr.instance.score + (headv * headScore))
            {
                yield return new WaitForSecondsRealtime(textDelay);
                scorec += 25;
                if (scorec % 75 == 0)
                    SoundManager.instance.scoreCount();
                score.text = scorec.ToString();
            }

            yield return new WaitForSecondsRealtime(0.2f);
        }

        if (maxcombov > 1)
        {
            bonusText2.text = "최대콤보 보너스 + " + maxcombov * maxComboScore;
            while (scorec < ScoreMgr.instance.score + (headv * headScore) + (maxcombov * maxComboScore))
            {
                yield return new WaitForSecondsRealtime(textDelay);
                scorec += 25;
                if (scorec % 75 == 0)
                    SoundManager.instance.scoreCount();
                score.text = scorec.ToString();
            }

            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}
