using UnityEngine;
using UniRx.Toolkit;

public class ObjectPooling<T> : ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Transform _parentTransform;
    private readonly RectTransform _parentRectTransform;

    /// <summary>생성자</summary>
    /// <param name="parent">오브젝트들의 부모</param>
    /// <param name="prefab">풀링할 오브젝트 프리팹</param>
    public ObjectPooling(Transform parent, T prefab)
    {
        _parentTransform = parent;
        _prefab = prefab;
    }

    public ObjectPooling(RectTransform parent, T prefab)
    {
        _parentTransform = parent;
        _prefab = prefab;
    }

    /// <summary>오브젝트 Rent시 없을 때 생성하는 함수</summary>
    /// <returns>생성한 오브젝트 반환</returns>
    protected override T CreateInstance()
    {
        var obj = GameObject.Instantiate(_prefab, _parentTransform);
        //obj.transform.SetParent(_parentTransform);
        return obj;
    }
}