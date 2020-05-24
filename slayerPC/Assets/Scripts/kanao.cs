using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kanao : MonoBehaviour
{
    public static kanao instance;
    public bool isRage = false;
    public AnimatorOverrideController rage, blind;
    public Image img;
    public float speed;
    private bool isGo = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Rage()
    {
        if (!isGo)
        {
            isGo = true;
            StartCoroutine(rageCor());
        }
    }

    IEnumerator rageCor()
    {
        Player.instance.GetComponent<Animator>().runtimeAnimatorController = rage;
        int save1 = Player.instance.nuckbackforce;
        Player.instance.nuckbackforce = (int)(Player.instance.nuckbackforce * 0.75f);
        isRage = true;
        float save = Player.instance.force;
        Player.instance.force *= 1.5f;
        mpSlider.instance.isMana = false;
        while (img.fillAmount>0)
        {
            img.fillAmount -= Time.deltaTime * speed;   
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Player.instance.GetComponent<Animator>().runtimeAnimatorController = blind;
        mpSlider.instance.isMana = true;
        Player.instance.force = save;
        Player.instance.nuckbackforce = save1;
        isRage = false;
    }
}
