using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasePoolGeneric<T> : ScriptablePoolTypeGeneric<T>
{
    private List<GameObject> poolList = new List<GameObject>();
    public List<GameObject> PoolList
    {
        get { return poolList; }
    }
    [Header("PoolObject Prefab")]
    [Space]
    public GameObject itemPrefab;
    public int poolSize;

    private int poolIndex;
    public int PoolIndex
    {
        get { return poolIndex; }
        set { poolIndex = value; }
    }
}
