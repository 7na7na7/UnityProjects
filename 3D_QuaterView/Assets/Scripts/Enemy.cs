using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   public int maxHealth;
   public int curHealth;

   private Rigidbody rigid;
   private BoxCollider boxCollider;

   private Material mat;
   private void Awake()
   {
      rigid = GetComponent<Rigidbody>();
      boxCollider = GetComponent<BoxCollider>();
      mat = GetComponent<MeshRenderer>().material;
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
