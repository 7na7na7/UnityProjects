using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public int damage;
    public Minion currentMinion;
    public Minion nextMinion;
    public static Player instance;

    public bool isDeskTop;
    public float speed;
    public Ease easeType;
    private SpriteRenderer spr;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Init();
    }
    void Init()
    {
        spr = GetComponent<SpriteRenderer>();
    }
    /*void Update()
    {
        if (isDeskTop) //PC
        {
            if (Input.GetMouseButtonDown(0))
            {
                Dash(Input.mousePosition);
            }
        }
        else //모바일
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Dash(touch.position);
                }
            }
        }
    }
    */
    public bool CanDash(Minion minion)
    {
        if (currentMinion == null)
        {
            return true;
        }
        else
        {
            if (nextMinion == null)
            {
                if (currentMinion != minion)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public void Dash(Vector2 pos, Minion minion = null)
    {
        //Vector2 pos = Camera.main.ScreenToWorldPoint(position);

        if (currentMinion == null)
        {
            GameManager.instance.TouchEffect(pos);
            currentMinion = minion;
        }
        else
        {
            if (nextMinion == null)
            {
                if (currentMinion != minion)
                {
                    GameManager.instance.TouchEffect(pos);
                    nextMinion = minion;
                }
            }
            return;
        }

        rotate(pos);
        transform.DOMove(pos, Vector2.Distance(transform.position, pos) / speed).SetEase(easeType).OnComplete(() =>
        {
            if (nextMinion != null)
            {
                if (currentMinion.Hit(damage))
                {
                    currentMinion = null;
                    Dash(nextMinion.transform.position, nextMinion);
                    nextMinion = null;
                }
                else
                {
                    currentMinion.Hit(damage);
                    currentMinion = null;
                }
            }
            else
            {
                currentMinion.Hit(damage);
                currentMinion = null;
            }
        });
    }
    void rotate(Vector2 to)
    {
        transform.eulerAngles = new Vector3(0, 0, GetAngle(transform.position, to) - 90);
        if (transform.position.x < to.x) //오른쪽돌진
            spr.flipX = false;
        else
            spr.flipX = true;
    }
    float GetAngle(Vector2 start, Vector2 end) //탄젠트 공식을 사용해 벡터에서 회전값으로 변환
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

}
