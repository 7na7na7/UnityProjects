using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 아이템 사용 담당
/// </summary>
public class UseItem 
{
    static UseItem _instance = null;
    static readonly object _padlock = new object();

    // 셔플 이벤트 등록
    public delegate void tileShuffle(bool Animation);
    public static event tileShuffle runShuffleTile;

    // todo: 여기 고쳐야 된다
    //UserModel userData = new UserModel();

    public UseItem()
    {
    }

    public static UseItem Instance
    {
        get
        {
            lock (_padlock)
            {
                if (_instance == null)
                {
                    _instance = new UseItem();
                }

                return _instance;
            }
        }
    }

    public static UseItem GetInstance()
    {
        if (_instance == null)
        {
            _instance = new UseItem();
        }
        return _instance;
    }

    /// <summary>
    /// 힌트 사용 가능 여부 반환하고 true인 경우 차감한다.
    /// </summary>
    /// <returns></returns>
    public bool UseHintItem()
    {
        //if (userData.itemHintCount > 0)
        //{
        //    userData.itemHintCount--;
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return false;
    }
    
    public bool UseShuffleItem()
    {
        //if (userData.itemShuffleCount > 0)
        //{
        //    userData.itemShuffleCount--;
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return false;
    }

    public bool UseCrushBadTileItem()
    {
        //if (userData.itemCrushBadTileCount > 0)
        //{
        //    userData.itemCrushBadTileCount--;
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return false;
    }

    public bool UseTileChangeItem()
    {
        //if (userData.itemChangeTileCount > 0)
        //{
        //    userData.itemChangeTileCount--;
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return false;
    }


    /// <summary>
    /// 타일에 관련된 행동 후 셔플이 필요한 경우 자동으로 타일 셔플을 진행한다.
    /// 만약 클리어 할 타일이 없으면 클리어로 간주한다.
    /// </summary>
    public static void AfterTileAction(Tile[,] tileMap)
    {
        // 필드에 남은 타일 수 체크
        int leftTiles = 0;
        foreach (Tile tile in tileMap)
        {
            if (tile != null)
                leftTiles++;
        }
        Debug.Log($"남은 타일: {leftTiles}");
        if (leftTiles <= 0)
        {
            // 미션 클리어
            PopupController.Instance.ShowPopupVictory();
            //UICenter.Instance.popupController.ShowPopupVictory();
        }

        //List<Tile> clearbleList = TileController.ClearableTiles(tileMap);
        //Debug.Log($"깰 수 있는 타일: {clearbleList.Count}");
        //if (clearbleList.Count <= 0)
        //{
        //    tileMap = ShuffleTile(tileMap); // 글로벌 타일 정보 업데이트하기 
        //    runShuffleTile(true);
        //}
    }

    /// <summary>
    /// 타일맵을 받아 위치 정보를 변경하고 반환한다.
    /// </summary>
    /// <param name="tileMap"></param>
    /// <returns></returns>
    public static Tile[,] ShuffleTile(Tile[,] tileMap)
    {
        // 타일맵의 기존 정보를 리스트로 저장
        List<Tile> tileList = new List<Tile>();

        for (int x = 0; x < tileMap.GetLength(0); x++)
        {
            for (int y = 0; y < tileMap.GetLength(1); y++)
            {
                // data가 null 이면 처리하지 않음
                if (tileMap[x, y] == null) continue;
                tileList.Add(tileMap[x, y]);
            }
        }

        // 리스트 섞기.
        System.Random range = new System.Random();
        int n = tileList.Count;
        while (n > 1)
        {
            n--;
            int k = range.Next(n + 1);
            Tile value = tileList[k];
            tileList[k] = tileList[n];
            tileList[n] = value;
        }

        // 돌려줄 타일맵 복제
        Tile[,] returnTileMap = (Tile[,])tileMap.Clone();

        // 새 값 쓰기
        for (int x = 0; x < returnTileMap.GetLength(0); x++)
        {
            for (int y = 0; y < returnTileMap.GetLength(1); y++)
            {
                // data가 null 이면 처리하지 않음
                if (tileMap[x, y] == null) continue;
                returnTileMap[x, y] = tileList[0];
                //returnTileMap[x, y].X = x;
                //returnTileMap[x, y].Y = y;
                tileList.RemoveAt(0);
            }
        }


        return returnTileMap;
    }

    /// <summary>
    /// 타일 셔플 이벤트.
    /// </summary>
    public void ItemShuffle(bool animation)
    {
        FieldDesignManager.tileMap = ShuffleTile(FieldDesignManager.tileMap); // 글로벌 타일 정보 업데이트하기 
        runShuffleTile(animation);
        AfterTileAction(FieldDesignManager.tileMap);
    }

    /// <summary>
    /// 힌트 사용
    /// </summary>
    public void ItemHint()
    {
        // 클리어 가능한 타일 가져오기
        //List<Tile> clearbleList = TileController.ClearableTiles(FieldDesignManager.tileMap);

        //if (clearbleList == null)
        //{
        //    Debug.LogError("사용할 힌트 없음");
        //}

        // 맨 앞에 2개만 가져오면 된다.
        // 노드 가져오기
        //List<Node> tileNode = TileController.PathFinding(clearbleList[0].X, clearbleList[0].Y, clearbleList[1].X, clearbleList[1].Y, FieldDesignManager.tileMap);

        // line 그리기 테스트 // todo: hint는 다른거 깨질 때까지 계속 표시해야 함
        //LineController.Instance.Init(tileNode, Color.green);
    }
}
