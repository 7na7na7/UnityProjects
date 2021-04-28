using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   public enum Type { A, B, C };

   public GameObject bullet;
   public Type enemyType;
   public int maxHealth;
   public int curHealth;
   public BoxCollider meleeArea;
   public bool isAttack;

   public Transform target;
   private Rigidbody rigid;
   private BoxCollider boxCollider;
   private NavMeshAgent nav;

   private Material mat;
   private Animator anim;
   public bool isChase;
   private void Awake()
   {
      rigid = GetComponent<Rigidbody>();
      boxCollider = GetComponent<BoxCollider>();
      mat = GetComponentInChildren<MeshRenderer>().material;
      nav = GetComponent<NavMeshAgent>();
      anim = GetComponentInChildren<Animator>();
      
      Invoke("ChaseStart",2f);
   }

   void ChaseStart()
   {
      isChase = true;
      anim.SetBool("isWalk",true);
   }
   
   private void Update()
   {
      if (nav.enabled)
      {
         nav.isStopped = !isChase;
         nav.SetDestination(target.position);
      }
   }

   private void FixedUpdate()
   {
      FreezeVelocity();
      Targeting();
   }

   void Targeting()
   {
      float targetRadius = 0;
      float targetRange = 0;
      
      switch (enemyType)
      {
         case Type.A :
            targetRadius = 1.5f;
            targetRange = 3f;
            break;
         case Type.B:
            targetRadius = 1f;
            targetRange = 12f;
            break;
         case Type.C:
            targetRadius = .5f;
            targetRange = 25f;
            break;
      }
      
      RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));
      if (rayHits.Length > 0 && !isAttack)
      {
         StartCoroutine(Attack());
      }
   }

   IEnumerator Attack()
   {
      isChase=false;
      isAttack = true;
      anim.SetBool("isAttack",true);

      switch (enemyType)
      {
         case Type.A:
            yield return new WaitForSeconds(.3f);
            meleeArea.enabled = true;
            yield return new WaitForSeconds(.9f);
            meleeArea.enabled = false;
            yield return new WaitForSeconds(.7f);
            break;
         case Type.B:
            yield return new WaitForSeconds(.1f);
            rigid.AddForce(transform.forward*35,ForceMode.Impulse);
            meleeArea.enabled = true;
            yield return new WaitForSeconds(.5f);
            rigid.velocity=Vector3.zero;
            meleeArea.enabled = false;
            yield return new WaitForSeconds(1.5f);
            break;
         case Type.C:
            yield return new WaitForSeconds(.5f);
            GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
            Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
            rigidBullet.velocity = transform.forward * 20;
            yield return new WaitForSeconds(2.25f);
            break;
      }
    
      isChase = true;
      isAttack = false;
      anim.SetBool("isAttack",false);
   }
      void FreezeVelocity()
   {
      if (isChase)
      {
         rigid.velocity=Vector3.zero;
         rigid.angularVelocity=Vector3.zero;   
      }
   }
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.tag == "Melee")
      {
         Weapon weapon = other.GetComponent<Weapon>();
         curHealth -= weapon.damage;
         Vector3 reactVec = transform.position - other.transform.position;
         StartCoroutine(OnDamage(reactVec));
      }
      else if (other.tag == "Bullet")
      {
         Bullet bullet = other.GetComponent<Bullet>();
         curHealth -= bullet.damage;
         Vector3 reactVec = transform.position - other.transform.position;
        Destroy(other.gameObject);
         StartCoroutine(OnDamage(reactVec));
      }
   }

   IEnumerator OnDamage(Vector3 reactVec, bool isGeenade = false)
   {
      mat.color=Color.red;
      yield return new WaitForSeconds(0.1f);
      if (curHealth > 0)
      {
         mat.color=Color.white;
      }
      else
      {
         mat.color=Color.gray;
         gameObject.layer = 12;
         nav.enabled = false;
         isChase = false;
         anim.SetTrigger("doDie");
         if (isGeenade)
         {
            reactVec.Normalize();
            reactVec+=Vector3.up*3;
            rigid.freezeRotation = false;
            rigid.AddForce(reactVec*5,ForceMode.Impulse);
            rigid.AddTorque(reactVec*15,ForceMode.Impulse); 
         }
         else
         {
            reactVec.Normalize();
            reactVec+=Vector3.up;
            rigid.AddForce(reactVec*5,ForceMode.Impulse);
         }
         Destroy(gameObject,4);  
      }
   }

   public void HitByGrenade(Vector3 explosionPos)
   {
      curHealth -= 100;
      Vector3 reactVec = transform.position - explosionPos;
      StartCoroutine(OnDamage(reactVec,true));
   }
}
