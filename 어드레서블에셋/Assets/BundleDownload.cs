using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BundleDownload : MonoBehaviour
{
    [SerializeField] Text SizeText;

    [Space]
    [Header("다운로드를 원하는 번들 또는 번들들에 포함된 레이블중 아무거나 입력해주세요.")]
    [SerializeField] string LableForBundleDown = string.Empty;

    public void ClickBundleDown()
    {
        Addressables.DownloadDependenciesAsync(LableForBundleDown).Completed +=
            (AsyncOperationHandle Handle) =>
            {
                //DownloadPercent프로퍼티로 다운로드 정도를 확인할 수 있음.
                //ex) float DownloadPercent = Handle.PercentComplete;

                Debug.Log("다운로드 완료!");

                //다운로드가 끝나면 메모리 해제.
                Addressables.Release(Handle);

            };
    }

    public void ClickCheckTheDownloadFileSize()
    {
        //크기를 확인할 번들 또는 번들들에 포함된 레이블을 인자로 주면 됨.
        //long타입으로 반환되는게 특징임.
        Addressables.GetDownloadSizeAsync(LableForBundleDown).Completed +=
            (AsyncOperationHandle<long> SizeHandle) =>
            {
                string sizeText = string.Concat(SizeHandle.Result, " byte");

                SizeText.text = sizeText;

                //메모리 해제.
                Addressables.Release(SizeHandle);
            };
    }
}