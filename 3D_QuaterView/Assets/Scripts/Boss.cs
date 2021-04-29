using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;

    private Vector3 lookVec;
    private Vector3 tauntVec;
    private bool isLook = true;

    private void Start()
    {
        StartCoroutine(Think());
        boxCollider.enabled = false;
    }

    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
        
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec=new Vector3(h,0,v)*5f;
            transform.LookAt(target.position+lookVec);
        }
        else
        {
            nav.SetDestination(tauntVec);
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(.1f);

        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3:
                StartCoroutine(RockShot());
                break;
            case 4:
                StartCoroutine(Taunt());
                break;
        }
    }
    
    IEnumerator MissileShot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(.2f);
        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target = target;
        yield return new WaitForSeconds(.3f);
        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileB.target = target;
        yield return new WaitUntil(()=>(anim.GetCurrentAnimatorStateInfo(0).IsName("Shot") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)==true);
        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        isLook = false;
        anim.SetTrigger("doBigShot");
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitUntil(()=>(anim.GetCurrentAnimatorStateInfo(0).IsName("BigShot") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)==true);
        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        anim.SetTrigger("doTaunt");
        tauntVec = target.position + lookVec;
        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        yield return new WaitUntil(()=>(anim.GetCurrentAnimatorStateInfo(0).IsName("Taunt") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)==true);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(.5f);
        meleeArea.enabled = false;
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }
}
