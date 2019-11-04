using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSet : MonoBehaviour
{
    public void one()
    {
        Screen.SetResolution(1920,1080,false);
    }
    public void two()
    {
        Screen.SetResolution(1540,810,false);
    }
    public void three()
    {
        Screen.SetResolution(960,540,false);
    }
    public void fullscreen()
    {
        Screen.SetResolution(1920,1080,true);
    }
}
