using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class girl : MonoBehaviour
{
    public string hitText = "아얏!";
    public static girl instance;
    //public float invisibleTime;
    public Slider hp;
    private Animator anim;
    void Start()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }
    
    public IEnumerator hitted(int damage)
    {
        anim.Play("girlhit");
        SoundManager.instance.girl();
        hp.value -= damage;
        if (hp.value <= 0)
        {
            Player.instance.isGameOver = true;
            FindObjectOfType<GameManager>().isGameOver = true;
            FindObjectOfType<GameOverManager>().GameoverFunc(gameObject);
            StopAllCoroutines();
            yield break;
        }
        else
        {
            if (FindObjectOfType<girlText>() != null)
                FindObjectOfType<girlText>().text.text = hitText;
            yield return new WaitForSeconds(1);
            anim.Play("girlAnim");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("damage"))
            StartCoroutine(hitted(10));
    }
    /*
    public IEnumerator invisibleCor()
    {
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
    }
    */
    
}
