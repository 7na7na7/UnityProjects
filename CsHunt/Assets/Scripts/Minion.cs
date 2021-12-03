using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class Minion : MonoBehaviour
{
    public int hp = 10;
    public Material mat;
    private Material originalMat;
    private SpriteRenderer spr;

    void OnMouseDown()
    {
        Player.instance.Dash(transform.position,this);
        spr.material = mat;
    }

    void Start()
    {
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
