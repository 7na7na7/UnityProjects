using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 자주사용되는 이미지, 팝업 프리팹 등을 Addressable 키 경로로부터 가져와 딕셔너리에 할당
        AddressableManager.Instance.CustomInit();

        // 오브젝트풀(게임플로팅 라벨,이펙트) 자원 할당
        // GamePoolManager.Instance.CustomInit();

        //사운드 소스 인덱스 접근 초기화
        GameAudioManager.Instance.CustomInit();

        //인트로 씬에서 다음 씬 (로그인 씬으로) 접근 진행
        GameSceneManager.Instance.CustomInit();

        // 게임에서 쓰이는 테이블 데이타 로드 후 캐시
        //GameDataManager.Instance.CustomInit();

        // 사용자 정보 가져오기
        //UserDataModel.Instance.Init();

        // 기타 게임 진행중의 데이타 정보 가공 관리
        //GameInfoManager.Instance.CustomInit();
    }
}
