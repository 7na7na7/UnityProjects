using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Minion : MonoBehaviour
{
    public int hp = 10;
    public Material mat;
    private Material originalMat;
    private SpriteRenderer spr;

    void OnMouseDown()
    {
        if (Player.instance.CanDash(this))
            spr.material = mat;
        Player.instance.Dash(transform.position,this);
    }

    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1, 1, 1), 0.25f).SetEase(Ease.OutBack);

        spr = GetComponent<SpriteRenderer>();
        originalMat = spr.material;
    }

    public bool Hit(int value)
    {
        hp -= value;
        if (hp <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        else
            return false;
    }
}
