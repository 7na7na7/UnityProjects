using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public float scrollSpeed;
    private bool isFirst = false;
    
    public Scrollbar scrollbar;
    public Transform contentTr;

    private const int SIZE =3;
    private float[] pos = new float[SIZE];
    private float distance, curPos, targetPos;
    private bool isDrag;
    public int targetIndex;

    public Slider tabSlider;
    public RectTransform[] BtnRect, BtnImgRect;
    public RectTransform[] BtnRect2;

    private float value = 0.5f;
    public float SelectedBtnSize, NormalBtnSize;
    void Start()
    {
        //거리에 따라 0~1인 pos 대입
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
        
        value=1f/(SIZE-1);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    { 
        curPos = SetPos();
        value = scrollbar.value;
    } 
    
    public void OnDrag(PointerEventData eventData)=> isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    { 
        isDrag = false;
        targetPos = SetPos();

        //절반거리를 넘지 않아도 마우스를 빠르게 이동하려면
            if (curPos == targetPos) //처음드래그시작한 위치와 드래그 끝난 위치가 같다면
            {
                if (Mathf.Abs(eventData.delta.x) > 8)
                {
                    //스크롤이 왼쪽으로 빠르게 이동시 목표가 하나 감소
                    if (scrollbar.value < value) //왼쪽이면
                    {
                        if (targetIndex == 0)
                        {
//                            targetIndex = SIZE-1;
//                            targetPos = 1;
                        }
                        else
                        {
                            --targetIndex;
                            targetPos = curPos - distance;
                        }
                    }
                    else
                    {
                        if (targetIndex == SIZE-1)
                        {
//                            targetIndex = 0;
//                            targetPos = 0;
                        }
                        else
                        {
                            ++targetIndex;
                            targetPos = curPos + distance;
                        }
                    }
                }
            }

    }

    void Update()
    {
        tabSlider.value = scrollbar.value;
        
        if (!isFirst)
        {
            isFirst = true;
            targetIndex = 1;
            scrollbar.value = 1f / (SIZE-1);
            targetPos = 1f / (SIZE-1);
        }
        else
        {
            if (!isDrag)
            {
                scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, scrollSpeed);
                //목표 버튼은 크기가 커짐
                for(int i=0;i<SIZE;i++) BtnRect[i].sizeDelta=new Vector2(i==targetIndex ? SelectedBtnSize:NormalBtnSize,BtnRect[i].sizeDelta.y);
            }

            for (int i = 0; i < SIZE; i++)
            {
                Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
                Vector3 BtnTargetScale=Vector3.one;
                if (i == targetIndex)
                {
                    //BtnTargetPos.y = -23f;
                    BtnTargetScale=new Vector3(1.3f,1.3f,1);
                }
                BtnImgRect[i].anchoredPosition3D = Vector3.Lerp(BtnImgRect[i].anchoredPosition3D, BtnTargetPos, 0.25f);
                BtnImgRect[i].localScale = Vector3.Lerp(BtnImgRect[i].localScale, BtnTargetScale, 0.25f);
            }
        }

        for (int i = 0; i < BtnRect.Length; i++)
        {
            BtnRect2[i].anchoredPosition3D =new Vector3(BtnRect[i].anchoredPosition3D.x,BtnRect2[i].anchoredPosition3D.y,BtnRect2[i].anchoredPosition3D.z); 
            BtnRect2[i].sizeDelta=new Vector2( BtnRect[i].sizeDelta.x, BtnRect2[i].sizeDelta.y);
        }
    }

    public void Right()
    {
        targetPos = 1;
    }

    public void Left()
    {
        targetPos = 0;
    }

    public void Middle()
    {
        targetPos = 0.5f;
    }

    public float SetPos()
    {
        //절반거리를 기준으로 가까운 위치를 반환
            for (int i = 0; i < SIZE; i++)
            {
                if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
                {
                    targetIndex = i;
                    return pos[i];
                }
            }
//            if (scrollbar.value > 0.3f)
//            {
//                targetIndex = 0;
//                return 0;
//            }
//            else
//            {
//                targetIndex = SIZE - 1;
//                return 1;
//            }
        return 0;
    }

    public void TabClick(int n)
    {
        targetIndex = n;
        targetPos = pos[n];
    }
}
