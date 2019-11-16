using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class dontdestroy : MonoBehaviour
{
    public static dontdestroy instance;
    
    
   public bool effect = true;
   
   public bool music = true;

   private void Start()
   {
       if (instance == null)
       {
           DontDestroyOnLoad(this.gameObject);
           instance = this;
       }
       else
           Destroy(this.gameObject);
   }
}
