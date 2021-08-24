using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//이거 두개 잇어야 함
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableAsset : MonoBehaviour
{
    public AssetReference PrefabReference;
    GameObject TempObj;

    public Image Image;
    AsyncOperationHandle Handle;


    private void Start()
    {
        //주소를 가지고 와 로드와 동시에 생성
        PrefabReference.InstantiateAsync(transform.position, Quaternion.identity).Completed +=
        (AsyncOperationHandle<GameObject> obj) =>
        {
            TempObj = obj.Result;
            Invoke("ReleaseDestroy", 3f); //3초 후 메모리 할당 해제
        };
        
    }
    //시간차 해제
    void ReleaseDestroy()
    {
        PrefabReference.ReleaseInstance(TempObj);
        //해제와 함꼐 오브젝트 삭제
    }


    public void ClickLoad()
    {
        //Sprite타입의 NyarukoSprite라는 어드레서블 에셋을 로드해 추가함
        Addressables.LoadAssetAsync<Sprite>("NyarukoSprite").Completed +=
           (AsyncOperationHandle<Sprite> Obj) =>
           {
               Handle = Obj;
               Image.sprite = Obj.Result;
           };
    }

    public void ClickUnLoad()
    {
        //Release로 제대로 해제해 주지 않으면 계속 남아있는다!
        Addressables.Release(Handle); //레퍼런스 카운트 내려감
        Image.sprite = null;
    }

    public void LoadByServer()
    {
        Addressables.InstantiateAsync("치카", GameObject.Find("Canvas").transform);
    }

}
