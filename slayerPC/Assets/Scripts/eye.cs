using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class eye : MonoBehaviour
{
    public GameObject eyeObj;
    private bool isAttack = false;
    public GameObject attack;
    void Start()
    {
        StartCoroutine(moveEye());
    }

    IEnumerator moveEye()
    {
        while (true)
        {
            eyeObj.transform.DOMove(new Vector2(eyeObj.transform.position.x,eyeObj.transform.position.y+1f), 2).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);
            eyeObj.transform.DOMove(new Vector2(eyeObj.transform.position.x,eyeObj.transform.position.y-1f), 2).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);   
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isAttack)
            {
                isAttack = true;
                StartCoroutine(attackCor());
            }
        }
    }

    IEnumerator attackCor()
    {
        attack.SetActive(true);
            attack.transform.eulerAngles = new Vector3(0, 0, -getAngle(attack.transform.position.x, attack.transform.position.y, Player.instance.transform.position.x, Player.instance.transform.position.y)+180);
            yield return new WaitForSeconds(3);
            isAttack = false;
        
    }
   
        
    private float getAngle(float x1, float y1, float x2, float y2) //Vector값을 넘겨받고 회전값을 넘겨줌
    {
        float dx = x2 - x1;
        float dy = y2 - y1;

        float rad = Mathf.Atan2(dx, dy);
        float degree = rad * Mathf.Rad2Deg;
        
        return degree;
    }
}
