using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Image = UnityEngine.Experimental.UIElements.Image;

public class ScrollScript : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Text bestScoreText;
    public GameObject context;
    private int difficulty = 1;
    public Button normal, hard, EZ;
    public Scrollbar scrollbar;
 public GameObject[] t;
 private Color Alpha125;
 private Color Alpha255;
    private const int SIZE = 3;
    private float[] pos = new float[SIZE];
    private float distance, curPos, targetPos;
    private bool isDrag;
    private int targetIndex;
    public GameObject stage2panel;
    public Text stage2Text;
    public GameObject stage3panel;
    public Text stage3Text;
    void Start()
    {
        Alpha125.r = 255;
        Alpha125.g = 255;
        Alpha125.b = 255;
        Alpha125.a = 0.5f;
        Alpha255.r = 255;
        Alpha255.g = 255;
        Alpha255.b = 255;
        Alpha255.a = 1f;
        FadePanel.instance.UnFade();
        //거리에 따라 0~1인 pos 대입
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
        normal.GetComponent<UnityEngine.UI.Image>().color = Alpha255;
        hard.GetComponent<UnityEngine.UI.Image>().color = Alpha125;
        EZ.GetComponent<UnityEngine.UI.Image>().color = Alpha125;
        context.SetActive(false);
        difficulty = 1;
    }

    public void normalChange()
    {
        context.SetActive(false);
        SoundManager.instance.select();
        normal.GetComponent<UnityEngine.UI.Image>().color = Alpha255;
        hard.GetComponent<UnityEngine.UI.Image>().color = Alpha125;
        EZ.GetComponent<UnityEngine.UI.Image>().color = Alpha125;
        difficulty = 1;
    }
    public void hardChange()
    {
        context.SetActive(true);
        SoundManager.instance.select();
        normal.GetComponent<UnityEngine.UI.Image>().color = Alpha125;
        hard.GetComponent<UnityEngine.UI.Image>().color = Alpha255;
        EZ.GetComponent<UnityEngine.UI.Image>().color = Alpha125;
        difficulty = 2;
    }
    public void EZChange()
    {
        context.SetActive(true);
        SoundManager.instance.select();
        normal.GetComponent<UnityEngine.UI.Image>().color = Alpha125;
        EZ.GetComponent<UnityEngine.UI.Image>().color = Alpha255;
        hard.GetComponent<UnityEngine.UI.Image>().color = Alpha125;
        difficulty = 0;
    }
    void Update()
    {
        if (GooglePlayManager.instance.canStage1 == 0)
        {
            stage2panel.SetActive(true);
            stage2Text.text = "[쿄우가이 처치 시 잠금 해제]";
        }
        else
        {
            stage2panel.SetActive(false);
            stage2Text.text = "나타구모 산";
        }
        if (GooglePlayManager.instance.canStage2 == 0)
        {
            stage3panel.SetActive(true);
            stage3Text.text = "[루이 처치 시 잠금 해제]";
        }
        else
        {
            stage3panel.SetActive(false);
            stage3Text.text = "무한열차";
        }
        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
            for (int i = 0; i < SIZE; i++)
            {
                if(i==targetIndex)
                    t[i].SetActive(true);
                else
                    t[i].SetActive(false);
            }
        }

        if (targetIndex == 0)
        {
            if (difficulty==1)
            {
                bestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("highScoreKey1", 0);
            }
            else if(difficulty==2)
            {
                bestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("highScoreKey1_H", 0);
            }
            else
            {
                bestScoreText.text = "E-Z";
            }
        }
        else if (targetIndex == 1)
        {
            if (difficulty==1)
            {
                bestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("highScoreKey2", 0);
            }
            else if(difficulty==2)
            {
                bestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("highScoreKey2_H", 0);
            }
            else
            {
                bestScoreText.text = "E-Z";
            }
        }
        else if (targetIndex == 2)//해야됨
        {
            if (difficulty==1)
            {
                float a = PlayerPrefs.GetFloat("fastTimeKeyN", 0);
                if (a == 0)
                    bestScoreText.text = "최단시간 : 미기록";
                else
                    bestScoreText.text = "최단시간 : " + a;
            }
            else if(difficulty==2)
            {
                float a = PlayerPrefs.GetFloat("fastTimeKeyH", 0);
                if (a == 0)
                    bestScoreText.text = "최단시간 : 미기록";
                else
                    bestScoreText.text = "최단시간 : " + a;
            }
            else
            {
                bestScoreText.text = "E-Z";
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData) => curPos = SetPos();
    
    public void OnDrag(PointerEventData eventData)=> isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    { 
        isDrag = false;

        targetPos = SetPos();
        
        //절반거리를 넘지 않아도 마우스를 빠르게 이동하려면
        if (curPos == targetPos) //처음드래그시작한 위치와 드래그 끝난 위치가 같다면
        {
            //스크롤이 왼쪽으로 빠르게 이동시 목표가 하나 감소
            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }
            //스크롤이 오른쪽으로 빠르게 이동시 목표가 하나 증가
            else if (eventData.delta.x < -18 && curPos - distance <=1f)
            {
                if (targetIndex + 1 < SIZE)
                {
                    ++targetIndex;
                    targetPos = curPos + distance;
                }
            }
        }
    }

    public float SetPos()
    {
        //절반거리를 기준으로 가까운 위치를 반환
        for(int i=0;i<SIZE;i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        return 0;
    }
    public void SceneChange()
    {
        if (targetIndex == 0)
            StartCoroutine(delayChange());
        else if(targetIndex==1)
            StartCoroutine(delayChange2());
        else if(targetIndex==2)
            StartCoroutine(delayChange3());
            
    }

    IEnumerator delayChange()
    {
        if (difficulty==1)
        {
            SoundManager.instance.tsuzumi(1);
            FadePanel.instance.Fade();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Main");
        }
        else if(difficulty==2)
        {
            SoundManager.instance.tsuzumi(1);
            FadePanel.instance.Fade();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Main_H");
        }
        else
        {
            SoundManager.instance.tsuzumi(1);
            FadePanel.instance.Fade();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Main_EZ");
        }
    }
    IEnumerator delayChange2()
    {
        if (GooglePlayManager.instance.canStage1 == 0)
        {
            SoundManager.instance.Locked();
        }
        else
        {
            if (difficulty==1)
            {
                SoundManager.instance.Grass();
                FadePanel.instance.Fade();
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Main2");
            }
            else if(difficulty==2)
            {
                SoundManager.instance.Grass();
                FadePanel.instance.Fade();
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Main2_H");
            }
            else
            {
                SoundManager.instance.Grass();
                FadePanel.instance.Fade();
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Main2_EZ");
            }
        }
    }
    IEnumerator delayChange3()
    {
        if (GooglePlayManager.instance.canStage2 == 0)
        {
            SoundManager.instance.Locked();
        }
        else
        {
            if (difficulty==1)
            {
                SoundManager.instance.steam();
                FadePanel.instance.Fade();
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Main3");
            }
            else if(difficulty==2)
            {
                SoundManager.instance.steam();
                FadePanel.instance.Fade();
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Main3_H");
            }
            else
            {
                SoundManager.instance.steam();
                FadePanel.instance.Fade();
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("Main3_EZ");
            }
        }
    }
}
