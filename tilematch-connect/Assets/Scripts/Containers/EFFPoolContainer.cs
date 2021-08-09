using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoGo.Extension;
using System.Linq;

public class EFFPoolcontainer : BasePoolContainer<EFFType>
{
    [SerializeField]
    private string PoolLabel = "EFFPool";
    [SerializeField]
    private List<EFFTypeObject> _poolList = null;


    public override void InitPoolContainer() { base.InitPoolContainer(); }
    public async void InitPoolTypeObject(){

        var result = await AddressableManager.Instance.LoadAssetsByLabel<EFFTypeObject>(PoolLabel);
        var orderedList = from n in result orderby n.initialValue.EFFTypeToIDX () select n;
        foreach (EFFTypeObject val in orderedList) _poolList.Add(val);

        foreach (EFFTypeObject _objectPool in _poolList)
        {
            if (_objectPool != null)
            {
                EFFTypeObject poolInfo = Instantiate(_objectPool) as EFFTypeObject;
                SetObjectPool(
                          poolInfo.PoolList,
                          poolInfo.poolSize,
                          poolInfo.itemPrefab,
                          GetContainerTransform(poolInfo.initialValue)
                          );
            }
        }
    }
    [SerializeField] GameObject _ref;
    public GameObject GetPoolObject(EFFType _type)
    {
        if (_poolList[_type.EFFTypeToIDX()] == null)
        {
            Debug.LogError("PoolList에 이펙트 프리팹 정보 없음!");
            return null;
        }

        for (int i = 0; i < _poolList[_type.EFFTypeToIDX()].PoolList.Count; i++)
        {
            if (_poolList[_type.EFFTypeToIDX()].PoolList[i] != null){
                _ref = _poolList[_type.EFFTypeToIDX()].PoolList[i];

                if (!_ref.activeSelf){
                    _ref.SetActive(true);
                    return _ref;
                }
            }
        }
        GameObject newObject = Instantiate(_poolList[_type.EFFTypeToIDX()].itemPrefab, Vector3.zero, Quaternion.identity);
        _poolList[_type.EFFTypeToIDX()].PoolList.Add(newObject);
        newObject.transform.SetParent(GetContainerTransform(_type));
        return newObject;
    }
    public void ReturnPool(EFFType _type, Transform _transform)
    {
        _transform.SetParent(GetContainerTransform(_type));
    }
}
