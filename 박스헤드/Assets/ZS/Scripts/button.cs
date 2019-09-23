using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class button : MonoBehaviour
{
    public Text enhancebtn;
    public int enhancecount = 10;
    private int a;
    private int currentweapon = 0;
    public Canvas canvas;
    private GoldManager gold;
    private Move1 player;
    private Level lv;
    private void Start()
    {
        gold = FindObjectOfType<GoldManager>();
        lv = FindObjectOfType<Level>();
        player = FindObjectOfType<Move1>();
    }

    private void Update()
    {
        enhancebtn.text = "무기 강화 - " + enhancecount + "G";
    }

    public void gatcha()
    {
        if (gold.gold >= 30)
        {
            gold.gold -= 30;
            while (true)
            {
                a = Random.Range(0, 4);
                if (currentweapon != a)
                {
                    currentweapon = a;
                    break;
                }
            }

            switch (a)
                {
                    case 0:
                        player.weaponselect(0);
                        break;
                    case 1:
                        player.weaponselect(1);
                        break;
                    case 2:
                        player.weaponselect(2);
                        break;
                    case 3:
                        player.weaponselect(3);
                        break;
                }
        }
    }
    public void heal()
    {
        if (gold.gold >= 10)
        {
            gold.gold -= 10;
            player.slider.value += 20;
            player.audiosource.PlayOneShot(player.healsound, 0.7f);
        }
    }

    public void enhance()
    {
        if (gold.gold >= enhancecount)
        {
            gold.gold -= enhancecount;
            int r = Random.Range(1, 22);
            switch (r)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    player.shotdelaycount++;
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    player.damagecount++;
                    break;
                case 21:
                    player.damagecount++; //나중에주석처리
                    player.specialcount++;
                    break;
            }

            enhancecount += 10;
        }
    }
    public void set()
    {
        Time.timeScale = 1;
        lv.isnext = true;
        canvas.gameObject.SetActive(false);
    }
}
