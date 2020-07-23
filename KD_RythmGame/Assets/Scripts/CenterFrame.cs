using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
   private AudioSource myAydio;
   private bool musicStart = false;
   private void Start()
   {
      myAydio = GetComponent<AudioSource>();
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (!musicStart)
      {
         if (other.CompareTag("Note"))
         {
            myAydio.Play();
            musicStart = true;
         }
      }
   }
}
