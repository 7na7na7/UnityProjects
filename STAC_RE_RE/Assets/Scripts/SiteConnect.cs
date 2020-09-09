using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteConnect : MonoBehaviour
{
   public void OpenSite(string url)
   {
      Application.OpenURL(url);
   }
}
