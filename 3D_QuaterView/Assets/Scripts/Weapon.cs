using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };

    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;
    
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing());   
        }
        else if (type == Type.Range && curAmmo>0)
        {
            curAmmo--; 
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.1f);
        trailEffect.enabled = false;
    }
    
    IEnumerator Shot()
    {
        //총알 발사
        GameObject bulletObj = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = bulletObj.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;
        //탄피 배출
        GameObject caseObj = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = caseObj.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3f, -2f) + Vector3.up * Random.Range(2f, -2f);
        caseRigid.AddForce(caseVec,ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up*10,ForceMode.Impulse);
        yield return null;
    }
}
