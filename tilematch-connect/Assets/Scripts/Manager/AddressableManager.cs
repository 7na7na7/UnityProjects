using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


using System;
using System.Threading.Tasks;

using Cysharp.Threading.Tasks;

using Object = UnityEngine.Object;

public class AddressableManager : MonoBehaviour
{
    protected AddressableManager() { }
    private static AddressableManager _instance;
    public static AddressableManager Instance
    { get { return _instance; } }
    private void MakeSingleton()
    {
        if (_instance != null) DestroyImmediate(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Awake()
    {

        MakeSingleton();

    }


    [System.Serializable]
    public class HashContainer : SerializableDictionary<string, int> { }
    [SerializeField]
    private HashContainer hashContainer = new HashContainer();


    [System.Serializable]
    public class AssetContainer : SerializableDictionary<int, Object> { }
    [SerializeField]
    private AssetContainer assetContainer = new AssetContainer();



    /// <summary>
    /// 어드레서블 리소스 키로 접근 초기화
    /// </summary>
    public void CustomInit()
    {
        Addressables.InitializeAsync().Completed += (handle) =>
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed InitializeAsync(). Status = " + handle.Status);
                return;
            }

            InitResourceAsset();
            //LoadResourceAsync<Sprite>("Sprite");
            LoadResourceAsync<GameObject>("Popup");
            //LoadResourceAsync<AudioSource>("Sound");
            //LoadResourceAsync<GameObject>("Tile");
        };

    }

    public ResourceController resourceController;

    /// <summary>
    /// 사용함수 : 타입에 맞는 에셋을 반환함
    /// </summary>
    /// <typeparam name="Type">리소스 타입</typeparam>
    /// <param name="_addressableKey">어드레서블 키</param>
    /// <returns></returns>
    public Type GetAsset<Type>(string _addressableKey) where Type : Object
    {
        int _hashKey = GetAssetHashKey(_addressableKey);
        if (!assetContainer.TryGetValue(_hashKey, out Object asset))
        {
            Debug.LogWarning("에셋 찾지 못함");
            return null;
        }
        return asset as Type;
    }

    /// <summary>
    /// 리소스의 해시키를 지정함 (AddressableKey -> HashKey)
    /// </summary>
    private void InitResourceAsset()
    {
        foreach (IResourceLocator loc in Addressables.ResourceLocators)
        {
            foreach (object objKey in loc.Keys)
            {
                string key = objKey as string;
                //Debug.Log("loc.Keys : " + key);
                if (key == null) continue;
                if (!System.Guid.TryParse(key, out System.Guid guid)) continue;

                IList<IResourceLocation> locationsFromKey;
                if (!loc.Locate(key, typeof(System.Object), out locationsFromKey)) continue;
                IResourceLocation location = locationsFromKey[0];
                hashContainer.Add(location.PrimaryKey, location.GetHashCode());
            }
        }
    }

    /// <summary>
    /// 어드레서블 리소스를 로드하여 에셋 컨테이너에 [해시키] = [리소스] 캐시함
    /// </summary>
    /// <typeparam name="Type">어드레서블 리소스 타입</typeparam>
    /// <param name="_label">어드레서블 리소스 라벨</param>
    private void LoadResourceAsync<Type>(string _label) where Type : Object
    {
        Addressables.LoadResourceLocationsAsync(_label).Completed += OnAssetLoaded;
    }
    /// <summary>
    /// 에셋 로드 핸들러
    /// </summary>
    /// <param name="handle"></param>
    private void OnAssetLoaded(AsyncOperationHandle<IList<IResourceLocation>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = handle.Result;
            foreach (IResourceLocation loc in locations)
            {
                AsyncOperationHandle<Object> assetHandle = Addressables.LoadAssetAsync<Object>(loc);

                /// 로딩바 추가

                assetHandle.Completed += (_assetHandle) =>
                {
                    if (_assetHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        assetContainer[GetAssetHashKey(loc.PrimaryKey)] = _assetHandle.Result;
                    }
                };
            }
        }
    }

    /// <summary>
    /// addressibleKey == IResourceLocation loc.PrimaryKey 를 통해 초기화 때 지정한 에셋의 해시키를 반환함
    /// </summary>
    /// <param name="_key">addressibleKey</param>
    /// <returns></returns>
    private int GetAssetHashKey(string _key)
    {
        if (!hashContainer.TryGetValue(_key, out int value))
        {
            Debug.LogError("PrimaryKey not found. key = " + _key);
        }
        return value;
    }




    #region Task 사용 비동기

    /// <summary>
    /// Loads an object of type T into memory.
    /// </summary>
    /// <typeparam name="T">Type of object to load.</typeparam>
    /// <param name="path">Path of the object to load in the Addressables system.</param>
    /// <returns>Returns an object of type T.</returns>
    public async Task<T> Load<T>(string path) where T : class
    {

        return await Addressables.LoadAssetAsync<T>(path);
    }

    /// <summary>
    /// Preloads all dependencies of an object, given its path.
    /// </summary>
    /// <param name="path">Path of the object to load dependencies from.</param>
    public async Task DownloadDependencies(string path)
    {
        await Addressables.DownloadDependenciesAsync(path);
    }

    public async Task<List<T>> LoadAssetsByLabel<T>(string label, Action<T> callback = null) where T : class
    {
        return await Addressables.LoadAssetsAsync<T>(label, callback) as List<T>;
    }
    public async Task<GameObject> InstantiateGameObject(string path)
    {
        return await Addressables.InstantiateAsync(path, Vector3.zero, Quaternion.identity) as GameObject;
    }


    /// <summary>
    /// Releases prefab from memory and destroys its instance in the currently active scene.
    /// </summary>
    /// <param name="obj">Gameobject reference of the prefab to release.</param>
    public void ReleaseInstance(ref GameObject obj)
    {
        if (obj != null)
            Addressables.ReleaseInstance(obj);
    }
    /// <summary>
    /// Releases a given object from memory.
    /// </summary>
    /// <typeparam name="T">Type of the object to release.</typeparam>
    /// <param name="obj">Object reference to release.</param>
    public void ReleaseAsset<T>(ref T obj) where T : class
    {
        if (obj != null)
            Addressables.Release(obj);
    }

    #endregion



}
