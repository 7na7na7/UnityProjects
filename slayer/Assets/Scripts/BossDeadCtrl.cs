using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadCtrl : MonoBehaviour
{
    public static BossDeadCtrl instance;

    public bool isDakiFirstDead = false; //다키가 처음 죽었는가?
    public bool isDakiDead = false; //다키가 죽어 있는가?
    public bool isGyutaroDead = false;
    
    void Start()
    {
        instance = this;
    }
    
}
