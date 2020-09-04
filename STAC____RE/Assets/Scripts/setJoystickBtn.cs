using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setJoystickBtn : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("joystickscale", 0.5f);
    }

    public void set()
    {
        JoyStickScale.instance.SetSize(slider.value);
    }

    public void Bug()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSenwRA0F47aGmmgLo2RoCx3MPG_Ldj2iIBVFB6iHVe-pHQKrg/viewform?vc=0&c=0&w=1&flr=0");
    }
}
