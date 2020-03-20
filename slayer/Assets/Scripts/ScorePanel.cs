using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public Text bonusText1,bonusText2;
    public float textDelay;
    public int headScore,maxComboScore;
    public Text head, killedOni, maxCombo, score;
    private int headv, killedOniv, scorev, maxcombov;
    private int headc = 0, killedOnic = 0, scorec=0,maxcomboc = 0;
    
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
        yield return new WaitForSecondsRealtime(0.3f);
        while (killedOnic<killedOniv)
        {
            yield return new WaitForSecondsRealtime(textDelay);
            if (killedOnic + 1 != killedOniv)
                killedOnic += 2;
            else
                killedOnic++;
            
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
                headc++;
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

            yield return new WaitForSecondsRealtime(0.3f);
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

            yield return new WaitForSecondsRealtime(0.3f);
        }
    }
}
