using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Rotate : MonoBehaviour
{
    //public RectTransform rotate;
    public int StartRot = 30;
    public bool isTriangle = true;
    public float delay;
    private float bpm;
    public int value;
    public ParticleSystem[] rotateParticles;

    private float bpmValue;

    private bool canRot = true;
    private void Start()
    {
        Player.instance.transform.eulerAngles=new Vector3(0,0,StartRot);
       bpm = BulletData.instance.BPMs[BulletData.instance.currentColorIndex];
       bpmValue = 60f / bpm;
    }

    private void Update()
    {
        if (BGM.instance.source.time % bpmValue<=0.05f)
        {
            if (Player.instance != null)
            {
                if(canRot)
                    StartCoroutine(RotateCor());
            }
        }
    }
    public void RotateSound()
    {
        SoundMgr.instance.Play(2,4,1);
    }
    IEnumerator RotateCor()
    {
        StartCoroutine(delayCor());
        RotateSound();
        //Player.instance.isSuper = true;
            for(int i=0;i<120/value;i++)
            {
                if(Player.instance.transform.eulerAngles.z+value>=361)
                    Player.instance.transform.eulerAngles = new Vector3(Player.instance.transform.eulerAngles.x, Player.instance.transform.eulerAngles.y, Player.instance.transform.eulerAngles.z + value);
                else
                    Player.instance.transform.eulerAngles = new Vector3(Player.instance.transform.eulerAngles.x, Player.instance.transform.eulerAngles.y, Player.instance.transform.eulerAngles.z - value);
                yield return new WaitForSeconds(delay);
            }
            //Player.instance.isSuper = false;
            ScoreMgr.instance.scoreUp(0,GameManager.instance.liveScoreUpValue,false,false);
            Emission();
    }

    IEnumerator delayCor()
    {
        canRot = false;
        yield return new WaitForSeconds(0.2f);
        canRot = true;
    }
//    IEnumerator RotateCor2(bool isR)
//    {
//        if (isR)
//        {
//            for(int i=0;i<90/value;i++)
//            {
//                if(transform.eulerAngles.z+value>=410)
//                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + value);
//                else
//                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - value);
//                yield return new WaitForSeconds(delay);
//            }
//        }
//        else
//        {
//            for(int i=0;i<90/value;i++)
//            {
//                if(transform.eulerAngles.z+value>=410)
//                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - value);
//                else
//                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + value);
//                yield return new WaitForSeconds(delay);
//            }
//            transform.eulerAngles=new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,Mathf.CeilToInt(transform.eulerAngles.z*10)/10);
//        }
//        RotateSound();
//        Emission();
//    }
//    public void ChangeRotate()
//    {
//        rotate.localScale=new Vector3(rotate.localScale.x*-1,rotate.localScale.y,rotate.localScale.z);
//        isRight = !isRight;
//        //이미지 좌우반전시키기
//    }

    public void Emission()
    {
        foreach (ParticleSystem p in rotateParticles)
        {
            p.Play();
        }
    }
}
