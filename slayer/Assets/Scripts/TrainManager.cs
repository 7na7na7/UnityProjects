using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    public Transform parent;
    public mon[] mons;
    private List<int> list = new List<int>(); //int형 리스트 선언
    private int n = 0;
    [System.Serializable]  
    public class mon
    {
        public GameObject monster;
        public int count;
        public Vector3[] poses;
    }
    
    void Start()
    {
        for (int i = 0; i < 4; i++) //i<mons.Length
        {
            list.Clear();
            if (i == 0 || i == 1 || i == 2 || i == 3) //
            {
                for (int j = 0; j < mons[i].count;j++)
                {
                    GameObject mon = Instantiate(mons[i].monster, parent);
                    while (true)
                    {
                        n = Random.Range(0, mons[i].poses.Length);
                        bool cango = false;
                        foreach (int a in list)
                        {
                            if (a == n)
                            {
                                cango = true;
                            }
                        }
                        if (!cango) //중복이 하나도 없으면
                        {
                            list.Add(n);
                            break;
                        }
                    }
                    mon.transform.position = mons[i].poses[n];
                }
            }
            else
            {
                for (int j = 0; j < mons[i].count;j++)
                {
                    GameObject mon = Instantiate(mons[i].monster);
                    while (true)
                    {
                        n = Random.Range(0, mons[i].poses.Length);
                        bool cango = false;
                        foreach (int a in list)
                        {
                            if (a == n)
                            {
                                cango = true;
                            }
                        }
                        if (!cango) //중복이 하나도 없으면
                        {
                            list.Add(n);
                            break;
                        }
                    }
                    mon.transform.position = mons[i].poses[n];
                }
            }
        }
    }
}
