using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoGo.Extension;
using System.Linq;

public class OBJPoolContainer : BasePoolContainer<OBJType>
{
    [SerializeField]
    private string PoolLabel = "OBJPool";
    [SerializeField]
    private List<OBJTypeObject> _poolList = null;

    public async void InitPoolTypeObject()
    {
        var result = await AddressableManager.Instance.LoadAssetsByLabel<OBJTypeObject>(PoolLabel);
        var orderedList = from n in result orderby n.initialValue.OBJTypeToIDX() select n;
        foreach (OBJTypeObject val in orderedList) _poolList.Add(val);

        foreach (OBJTypeObject _objectPool in _poolList)
        {
            if (_objectPool != null)
            {
                OBJTypeObject poolInfo = Instantiate(_objectPool) as OBJTypeObject;
                SetObjectPool(
                          poolInfo.PoolList,
                          poolInfo.poolSize,
                          poolInfo.itemPrefab,
                          GetContainerTransform(poolInfo.initialValue)
                          );
            }
        }
    }
    public override void InitPoolContainer()
    {
        base.InitPoolContainer();
    }
    [SerializeField]
    GameObject _ref;
    public GameObject GetPoolObject(OBJType _type)
    {
        if (_poolList[_type.OBJTypeToIDX()] == null){
            Debug.LogError("PoolList에 오브젝트 프리팹 정보 없음!");
            return null;
        }

        for (int i = 0; i < _poolList[_type.OBJTypeToIDX()].PoolList.Count; i++)//저장소 탐색
        {
            if (_poolList[_type.OBJTypeToIDX()].PoolList[i] != null){
                _ref = _poolList[_type.OBJTypeToIDX()].PoolList[i];
                if (!_ref.activeSelf){
                    _ref.SetActive(true);
                    return _ref;
                }
            }
        }
        GameObject newObject = Instantiate(_poolList[_type.OBJTypeToIDX()].itemPrefab, Vector3.zero, Quaternion.identity);
        _poolList[_type.OBJTypeToIDX()].PoolList.Add(newObject);
        newObject.transform.SetParent(GetContainerTransform(_type));
        return newObject;
    }
    public void ReturnPool(OBJType _type, Transform _transform)
    {
        _transform.SetParent(GetContainerTransform(_type));
    }
}