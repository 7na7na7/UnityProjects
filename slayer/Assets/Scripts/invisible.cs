using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisible : MonoBehaviour
{
    public float invisibleTime; //뎀지입었을때 깜박거리는주기

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("oni"))
        {
            if (GetComponent<Player>().isattack)
            {
            }
            else
            {
                if (!GetComponent<Player>().isSuper)
                {
                    StopAllCoroutines();
                    StartCoroutine(invisibleCor());
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D hit)
    {
        if (hit.CompareTag("oni"))
        {
            if (GetComponent<Player>().isattack)
            {
            }
            else
            {
                if (!GetComponent<Player>().isSuper)
                {
                    StopAllCoroutines();
                    StartCoroutine(invisibleCor());
                }
            }
        }
    }
    public IEnumerator invisibleCor()
    {
        GetComponent<Player>().isSuper = true;
        Color color;
        SpriteRenderer sprite= GetComponent<SpriteRenderer>();//스프라이트로 함
        color.r = 255;
        color.g = 255;
        color.b = 255;
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        GetComponent<Player>().isSuper = false;
    }
}
