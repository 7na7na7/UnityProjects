using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
   private PlayerScript player;
   public GameObject Player;
   public float jumpforce;

   private void Awake()
   {
      player = FindObjectOfType<PlayerScript>();
   }

   public void Jump()
   {
      if (player.canjump)
      {
         Player.GetComponent<Rigidbody>().AddForce(0, jumpforce, 0);
         player.canjump = false;
      }
   }
}
