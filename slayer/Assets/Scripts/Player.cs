using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int comboCount = 0;
    public bool canCombo = false;
    public int force;
    public Transform min, max;
    private Rigidbody2D rigid;
    public GameObject particle;
    public TrailRenderer trail;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(transform.position, pos) > 1)
                {
                    if (pos.x > min.position.x && pos.y > min.position.y && pos.x < max.position.x &&
                        pos.y < max.position.y)
                    {
                        StopAllCoroutines();
                        StartCoroutine(go(pos));
                    }
                    else
                    {
                        if (pos.y < min.position.y)
                        {
                            pos.y = min.position.y;
                            StopAllCoroutines();
                            StartCoroutine(go(pos));
                        }
                    }
                }
            }
        }


        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if (Vector2.Distance(transform.position, pos) > 1)
            {
                if (pos.x > min.position.x && pos.y > min.position.y && pos.x < max.position.x &&
                    pos.y < max.position.y)
                {
                    StopAllCoroutines();
                    StartCoroutine(go(pos));
                }
                else
                {
                    if (pos.y < min.position.y)
                    {
                        pos.y = min.position.y;
                        StopAllCoroutines();
                        StartCoroutine(go(pos));
                    }
                }
            }
        }
    }

    IEnumerator go(Vector2 to)
    {
        if (canCombo) //콤보중
        {
            trail.startColor=Color.yellow;
            particle.SetActive(true);
            comboCount++;
            //int currentcount = comboCount;
            rigid.bodyType = RigidbodyType2D.Kinematic;
            if(Time.timeScale==1) 
                Time.timeScale = 0.5f;
            while (Vector2.Distance(transform.position, to) >= 1f) 
            {
                Vector2 dir = to - (Vector2) transform.position;
                dir.Normalize();
                if (Time.timeScale == 0.5f)
                { 
                    transform.Translate(dir * force*0.01f);
                        yield return new WaitForSecondsRealtime(0.005f);
                }
                else
                {
                    transform.Translate(dir * force * Time.deltaTime);
                    yield return new WaitForSeconds(0.005f);
                }
            }
            rigid.velocity = Vector2.zero;
            yield return new WaitForSecondsRealtime(0.2f);
            rigid.bodyType = RigidbodyType2D.Dynamic;
            canCombo = false;
            comboCount = 0;
            Time.timeScale = 1f;
            particle.SetActive(false);
            trail.startColor=Color.white;
        }
        else //첫번째
        {
            canCombo = true; ////////////////////////
            rigid.bodyType = RigidbodyType2D.Kinematic;
            while (Vector2.Distance(transform.position, to) >= 1f) 
            {
                Vector2 dir = to - (Vector2) transform.position;
                dir.Normalize();
                transform.Translate(dir * force*0.5f * Time.deltaTime);
                yield return new WaitForSeconds(0.005f);
            }
            rigid.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            rigid.bodyType = RigidbodyType2D.Dynamic;
            canCombo = false;
            comboCount = 0;   
            particle.SetActive(false);
            trail.startColor=Color.white;
        }
    }
}
