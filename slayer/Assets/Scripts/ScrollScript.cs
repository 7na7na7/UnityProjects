using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScrollScript : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
 public Scrollbar scrollbar;
 public GameObject[] t;
    private const int SIZE = 2;
    private float[] pos = new float[SIZE];
    private float distance, curPos, targetPos;
    private bool isDrag;
    private int targetIndex;
    public GameObject stage2panel;
    public Text stage2Text;
    void Start()
    {
        FadePanel.instance.UnFade();
        //거리에 따라 0~1인 pos 대입
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
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
        {
            StartCoroutine(delayChange()); 
        }
        else if(targetIndex==1)
        {
            StartCoroutine(delayChange2());
        }
    }

    IEnumerator delayChange()
    {
        SoundManager.instance.tsuzumi(1);
        FadePanel.instance.Fade();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
    }
    IEnumerator delayChange2()
    {
        if (GooglePlayManager.instance.canStage1 == 0)
        {
            SoundManager.instance.Locked();
        }
        else
        {
            SoundManager.instance.Grass();
            FadePanel.instance.Fade();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Main2");
        }
    }
}
