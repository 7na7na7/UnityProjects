using System.Collections.Generic;
using Leguar.TotalJSON;
using UniRx;


/// <summary>
/// 사용자 정보를 저장하고 불러오는 곳
/// todo: 미완성이므로 추가 필요함
/// </summary>
public static class UserModel
{

    /// <summary>
    /// 사용자 이름
    /// </summary>
    public static string userName { get; set; }
    /// <summary>
    /// 회원 등급 점수: 높으면 난이도가 올라간다.
    /// </summary>
    public static int mmr { get; set; }
    /// <summary>
    /// 진행한 스테이지 단계
    /// </summary>
    public static ReactiveProperty<int> stageLevel { get; set; }
    /// <summary>
    /// 힌트 남은 사용량
    /// </summary>
    public static ReactiveProperty<int> itemHintCount { get; set; }
    /// <summary>
    /// 패 섞기 남은 사용량
    /// </summary>
    public static ReactiveProperty<int> itemShuffleCount { get; set; }
    /// <summary>
    /// 타일 교환권 남은 사용량
    /// </summary>
    public static ReactiveProperty<int> itemChangeTileCount { get; set; }
    /// <summary>
    /// 특수 타일(안 좋은 타일) 제거 사용량
    /// </summary>
    public static ReactiveProperty<int> itemCrushBadTileCount { get; set; }
    /// <summary>
    /// 스테이지 이름 리스트 todo: 재설계 필요함
    /// </summary>
    public static List<string> stageName { get; set; }
    /// <summary>
    /// 현재 스테이지 이름
    /// </summary>
    public static ReactiveProperty<string> currentStageName { get; set; }
    /// <summary>
    /// 보유한 스타 포인트
    /// </summary>
    //public int starPoints { get; set; }
    public static ReactiveProperty<int> starPoints { get; set; }

    public static void Init()
    {
        // todo: 외부에서 값을 읽어와야 한다.
        // 값이 없을 때 최초 초기화
        JSON userDataJO = new JSON();

        userDataJO.Add("text", "Hello World!");

        mmr = 100;
        itemHintCount = new ReactiveProperty<int>(994);
        itemShuffleCount = new ReactiveProperty<int>(993);
        itemCrushBadTileCount = new ReactiveProperty<int>(992);
        itemChangeTileCount = new ReactiveProperty<int>(991);
        starPoints = new ReactiveProperty<int>(1024);
        stageLevel = new ReactiveProperty<int>(1);
        currentStageName = new ReactiveProperty<string>("Osaka"); // todo: 스테이지 이름은 정리를 해야된다.
        userName = "Montora";
    }


    //public useHint
    
}
