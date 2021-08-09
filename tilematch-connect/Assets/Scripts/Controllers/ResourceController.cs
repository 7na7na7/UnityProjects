using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoGo.Extension;

public class ResourceController : MonoBehaviour
{
    private static ResourceController _instance;
    public static ResourceController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = AddressableManager.Instance.resourceController;
                return _instance;
            }
            else return _instance;
        }
    }

    #region 메모리 로드 후에 어드레서블 키 값으로 에셋 가져오기
    public GameObject GetPopupPrefab(string _key)
    {
        return AddressableManager.Instance.GetAsset<GameObject>(_key);
    }
    public GameObject GetPrefabGameObject(string _key)
    {
        GameObject _prefab = AddressableManager.Instance.GetAsset<GameObject>(_key);


        GameObject _gameGameObject = Instantiate(_prefab, Vector3.zero, Quaternion.identity);

        return _gameGameObject;
    }

    public Sprite GetSpriteImage(string _key)
    {
        return AddressableManager.Instance.GetAsset<Sprite>(_key);
    }



    public AudioSource GetAudioSource(string _key)
    {
        return AddressableManager.Instance.GetAsset<AudioSource>(_key);
    }
    #endregion


    public async void InstantiateObject(Transform _parent, Vector3 _localPos, string _name)
    {
        GameObject _instantiated = await AddressableManager.Instance.InstantiateGameObject(_name);
        _instantiated.transform.SetParent(_parent);
        _instantiated.transform.localPosition = _localPos;

    }

    #region 테스트 단일 Task ... 
    [Header("-Test Field-")]
    public string testPrefab = "testPrefab";
    public Transform testParent;
    async void TestInstantiateObject(Vector3 _pos)
    {
        GameObject _instantiated = await AddressableManager.Instance.InstantiateGameObject(testPrefab);
        if (testParent == null) testParent = this.transform;
        _instantiated.transform.SetParent(testParent);
        _instantiated.transform.localPosition = Vector3.zero;

    }



    public string testLabel = "testLabel";
    public List<Sprite> testList = new List<Sprite>();

    async void TestLoadSpriteByLabel()
    {
        testList = await AddressableManager.Instance.LoadAssetsByLabel<Sprite>(testLabel);
    }



    public AudioSource source;
    public string testSource = "testSource";
    async void TestLoadAudioSource(string path)
    {
        source = await AddressableManager.Instance.Load<AudioSource>(testSource);
    }
    #endregion

}
