using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WebCol : MonoBehaviour
{
    public bool isPattern2 = false;
    private Color color;
    private SpriteRenderer spr;
    private string savedTag;

    private void Start()
    {
        if (isPattern2)
        {
            savedTag = gameObject.tag;
            gameObject.tag = "Untagged";
            transform.position = new Vector3(
                Random.Range(GameObject.Find("Min").transform.position.x + 5, GameObject.Find("Max").transform.position.x - 5), 
                Random.Range(GameObject.Find("Min").transform.position.y+3, GameObject.Find("Max").transform.position.y - 3), 0);
            transform.eulerAngles=new Vector3(0,0,Random.Range(0,360));
        }
        spr = GetComponent<SpriteRenderer>();
        color.r = spr.color.r;
        color.g = spr.color.g;
        color.b = spr.color.b;
        color.a = 0;
    }

    private void Update()
    {
        if (isPattern2)
        {
            if (color.a < 1f)
            {

                color.a += 0.015f;
                spr.color = color;
            }
            else
            {
                isPattern2 = false;
                gameObject.tag = savedTag;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isPattern2)
            {
                if (gameObject.tag != "damage")
                {
                    if (Player.instance.isattack)
                    {
                        SoundManager.instance.body();
                        Player.instance.oniBody(gameObject);
                        Player.instance.AttackCor();
                        Destroy(gameObject);
                    }
                    else
                    {
                        Player.instance.die();
                    }
                }
            }
        }
    }
}
