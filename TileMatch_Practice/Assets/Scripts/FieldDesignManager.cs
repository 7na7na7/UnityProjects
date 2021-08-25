//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FieldDesignManager : MonoBehaviour
//{
//    //public int col, row;
//    public int tileSetCount;
//    public GameObject prefabTile;
//    public Transform field;
//    public Transform worldBackground;

//    // 글로벌 타일맵 정보
//    public static Tile[,] tileMap;
//    public static Sprite[] useImageArray;
//    public static int fieldSizeX;
//    public static int fieldSizeY;


//    // 필드 좌표 이동에 따른 offset
//    public static float fieldXOffset;
//    public static float fieldYOffset;



//    // private
//    private Sprite[] tileImage;
//    private Sprite[] tileBG;

//    //private int totalTileCount;

//    // todo 마이그레이션 필요함
//    //UserModel userModel;


//    // Start is called before the first frame update
//    void Start()
//    {
//        // todo: 여기서 난이도, 필드 사이즈, 개인 설정 등 사전 정보를 가져와야 한다. // mmr을 주고 받으며 처음 게임 로딩시 자신의 mmr을 가져와야 한다.

//        //userModel = new UserModel();

//        //Level thisLevel = LevelDesigner.GetLevel(UserData.Instance.GetMMR());
//        //LevelDesigner.Init(userModel.mmr, userModel.stageLevel.Value); // 레벨 세팅

//        // 필드 사이즈 정해주기
//        fieldSizeX = LevelDesigner.thisLevel.col + 2;
//        fieldSizeY = LevelDesigner.thisLevel.row + 2;
//        // 사용할 리소스 가져오기
//        // 타일 필드의 수에 따라서 
//        // 타일 오브젝트를 가져와서 총 보여줄 개수만큼 무작위로 만든다.
//        tileImage = Resources.LoadAll<Sprite>("Tileset/TileImg/sample-tile");
//        tileBG = Resources.LoadAll<Sprite>("Tileset/TileBG/Tile-01");

//        // 사용할 이미지 셋 가져오기
//        useImageArray = GetImageSet(LevelDesigner.thisLevel.tileComplexSize, tileImage);

//        // 타일 데이터리스트 생성
//        //TileController.tileDataList = GetTileList(2, totalTileCount, useImageArray.Length, col, row);
//        // 타일맵 만들기
//        tileMap = GetTileMap(LevelDesigner.thisLevel.col, LevelDesigner.thisLevel.row, useImageArray.Length, 2, LevelDesigner.thisLevel.tileSize);

//        SetField(tileMap, useImageArray, field);
//        MoveFieldToCenter(field, LevelDesigner.thisLevel.col, LevelDesigner.thisLevel.row);
//        SetCameraDistance(LevelDesigner.thisLevel.col, LevelDesigner.thisLevel.row);

//        // 준비되면 셔플하기
//        UseItem.Instance.ItemShuffle(false);
//    }

//    /// <summary>
//    /// 필드에 타일 배치
//    /// </summary>
//    /// <param name="tileMap">필드에 타일을 배치한다.</param>
//    public void SetField(Tile[,] tileMap, Sprite[] useImageArray, Transform field)
//    {
//        for (int x = 0; x < tileMap.GetLength(0); x++)
//        {
//            for (int y = 0; y < tileMap.GetLength(1); y++)
//            {
//                // data가 null 이면 처리하지 않음
//                if (tileMap[x, y] == null) continue;

//                // 프리팹으로 타일 만들어놓기
//                GameObject _go = Instantiate(prefabTile, Vector3.zero, Quaternion.identity);
//                //_go.GetComponent<TilePresenter>().Init(tileBG[0], useImageArray[tileMap[x, y].ImageID], tileMap[x, y].TileID, tileMap[x, y].ImageID, x, y);

//                // 타일을 필드에 붙이기
//                _go.transform.SetParent(field);

//                // 타일 순서 잡아주기
//                _go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = x * y;
//                _go.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = x * y + 1;

//                // 위치 배치
//                _go.transform.position = new Vector2(x, y);
//            }
//        }
//    }

//    /// <summary>
//    /// 필드를 중앙에 맞춘다.
//    /// todo: offset을 리턴해줘야 할 필요가 있음, 가로 세로 크기를 넘겨받아야 함
//    /// </summary>
//    /// <param name="field">대상 필드</param>
//    private void MoveFieldToCenter(Transform field, int col, int row)
//    {
//        // 필드 좌표 이동 (크기 기반)
//        fieldXOffset = -col / 2.0f - 0.5f;
//        fieldYOffset = -row / 2.0f - 0.5f;
//        field.transform.position = new Vector2(fieldXOffset, fieldYOffset);
//    }


//    /// <summary>
//    /// 카메라 거리를 조정한다.
//    /// todo: 함수형으로 변경할 필요가 있음
//    /// </summary>
//    private void SetCameraDistance(int col, int row)
//    {
//        // 카메라 거리 이동
//        // 카메라 크기 계산
//        float cameraDistance;
//        if (col > row * 1.6)
//        {
//            cameraDistance = col * 1.6f;
//        }
//        else
//        {
//            cameraDistance = row;
//        }
//        Camera.main.orthographicSize = cameraDistance;

//        // 배경 크기 변경
//        float scaleRatio = cameraDistance / 5.0f;
//        worldBackground.localScale = new Vector3(scaleRatio, scaleRatio, 1);
//    }


//    /// <summary>
//    /// 사용할 이미지 선정
//    /// 주어진 이미지 셋에서 사용할 수 있는 세트를 무작위로 꺼내온다.
//    /// </summary>
//    /// <param name="requestImageCount">꺼내올 세트 개수</param>
//    /// <param name="imageSet">꺼내올 대상 세트</param>
//    /// <returns>요청한 이미지 개수만큼 무작위로 리턴</returns>
//    private Sprite[] GetImageSet(int requestImageCount, Sprite[] imageSet)
//    {
//        int imageSetCount, startRandomIndex;
//        Sprite[] returnImageSet;

//        imageSetCount = imageSet.Length;
//        if (imageSetCount < requestImageCount)
//        {
//            throw new System.Exception("이미지 세트 개수 부족");
//        }
//        if (requestImageCount <= 0)
//        {
//            throw new System.Exception("최소 1개 이상의 이미지 세트 필요함");
//        }

//        // 스프라이트 선정
//        startRandomIndex = Random.Range(0, imageSetCount - requestImageCount);
//        // 담기
//        returnImageSet = new Sprite[requestImageCount];
//        for (int i = 0; i < requestImageCount; i++)
//        {
//            returnImageSet[i] = imageSet[i + startRandomIndex];
//        }

//        return returnImageSet;
//    }

//    /// <summary>
//    /// 타일맵 생성기
//    /// </summary>
//    /// <returns>2차원 배열의 타일 데이터(타일에 대한 값이 수치로 들어있음)</returns>
//    private Tile[,] GetTileMap(int col, int row, int useImageArrayCount, int pairCount, int maxTileCount)
//    {
//        // 타일 데이터 초기화
//        Tile[,] returnTileData = new Tile[col + 2, row + 2]; // 가장자리를 비워줘야 하기 때문에 여백을 두고 만든다.

//        // 반환 개수
//        //int maxTileCount = (col * row) - (totalTileCount % pairCount);

//        // 페어 카운트를 체크해서 절반만 만든 후 복제하고 아이디 붙이기
//        // 이미지 아이디는 쌍으로 완성해야 한다. 하단에서 만들어 놓은 것 사용한다.
//        List<int> imageIdList = new List<int>();

//        for (int j = 0; j < maxTileCount / pairCount; j++)
//        {
//            for (int i = 0; i < pairCount; i++)
//            {
//                imageIdList.Add(j % useImageArrayCount);
//            }
//        }

//        int tileId = 0;
//        for (int x = 1; x < col + 1; x++)
//        {
//            for (int y = 1; y < row + 1; y++)
//            {
//                if (tileId >= maxTileCount) break; // 짝수가 안맞을수 있기 때문에 브레이크 해야 한다.
//                // 데이터 생성
//                int thisImageId = imageIdList[tileId % imageIdList.Count];
//                Tile.TileCoords coords = new Tile.TileCoords(x, y);
//                returnTileData[x, y] = new Tile(tileId, thisImageId, coords);
//                tileId++;
//            }
//        }

//        return returnTileData;
//    }


//}
