using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BasePoolContainer<T> : MonoBehaviour
{
    
    protected void SetObjectPool(List<GameObject> objectList, int size, GameObject prefab, Transform transform)
    {
        objectList.Clear();
        for (int i = 0; i < size; i++)
        {
            GameObject poolObject = Instantiate(prefab) as GameObject;
            poolObject.transform.SetParent(transform);
            objectList.Add(poolObject);
            poolObject.SetActive(false);
        }
                
    }

    protected Dictionary<T, Transform> mPoolListTransform = new Dictionary<T, Transform> { };
    public virtual void InitPoolContainer()
    {
        foreach (T _type in Enum.GetValues(typeof(T))) mPoolListTransform[_type] = null;
        List<T> _typeList = mPoolListTransform.Keys.ToList();
        for (int i = 0; i < _typeList.Count; i++)
        {
            Transform _container = new GameObject().transform;
            mPoolListTransform[_typeList[i]] = _container;
            AddContainer(_container, Vector3.zero, "Container_" + _typeList[i].ToString());
        }
    }
    protected void AddContainer(Transform _tr, Vector3 _pos, string _name)
    {
        _tr.transform.SetParent(this.transform, true);
        _tr.transform.position = _pos;
        _tr.name = _name;
    }
    public Transform GetContainerTransform(T _poolType)
    {
        return mPoolListTransform[_poolType];
    }
}
