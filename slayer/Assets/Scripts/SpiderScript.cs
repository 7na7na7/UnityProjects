using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpiderScript : MonoBehaviour
{
    public float startDown = 4.6f;
    public float startDownTime = 1f;
    public float minTIme, maxTime;
    public float maxDown;
    public Ease ease;
    private Vector3 StartPos;
    private Vector3 EndPos;
    public float[] randomMovingTr;
    public GameObject spiderAttack;
    public float attackDelay;
    void Start()
    {
        EndPos=new Vector3(transform.position.x,transform.position.y-maxDown,0);
        StartCoroutine(tween());
    }

    IEnumerator tween()
    {
        Vector3 targetPos=Vector3.zero;
        transform.DOMove(new Vector3(transform.position.x, transform.position.y - startDown, 0), startDownTime);
        yield return new WaitForSeconds(startDownTime);
        StartPos = transform.position;
        StartCoroutine(attack());
        while (true)
        {
            float time = Random.Range(minTIme, maxTime);
            if (Random.Range(0, 3) == 0)
            {
                
            }
            else
            {
                targetPos=new Vector3(transform.position.x,transform.position.y+randomMovingTr[Random.Range(0,randomMovingTr.Length)],0);
                if (targetPos.y > StartPos.y)
                    targetPos = StartPos;
                if (targetPos.y < EndPos.y)
                    targetPos = EndPos;
                if(transform.position!=targetPos) 
                    transform.DOMove(targetPos,time).SetEase(ease);   
            }
            if(transform.position!=targetPos) 
                yield return new WaitForSeconds(time);
            else
                yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);
            GetComponent<Animator>().Play("oni4_Attack");
            Instantiate(spiderAttack,transform.position,Quaternion.identity);
            SoundManager.instance.SpiderAttack();
            yield return new WaitForSeconds(0.5f);
            GetComponent<Animator>().Play("oni4_Idle");
        }
    }
}
