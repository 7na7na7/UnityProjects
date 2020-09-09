using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void exit()
    {
        anim.Play("DefaultAnim");
    }

    public void open()
    {
        anim.Play("SettingPanelAnim");
    }

    public void portrait()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution=new Vector2(1080,1920);
    }

    public void landscape()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution=new Vector2(1920,1080);
    }
}
