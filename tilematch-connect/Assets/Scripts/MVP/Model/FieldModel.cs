using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine;

public static class FieldModel
{
    public static Tile[,] tileMap;
    public static float fieldXOffset;
    public static float fieldYOffset;


    // 남은 타일 수 처리
    public static ReactiveProperty<int> leftTileCount = new ReactiveProperty<int>();
    public static ReactiveProperty<int> leftClearableCount = new ReactiveProperty<int>();


    // 2차원 배열에 타일 초기값을 넣어주면 생성한다. 
    public static void Init(int col, int row, int totalTileCount, int imageTypeCount)
    {
        // 기존 타일맵 삭제
        tileMap = null;

        // offset 조정
        //가운데에 오게 하는 코드
        fieldXOffset = -col / 2.0f - 0.5f;
        fieldYOffset = -row / 2.0f - 0.5f;

        // totalTileCount가 이상한지 체크
        if (totalTileCount % 2 != 0 || totalTileCount > col * row) throw new Exception("tile count 잘못됨");

        tileMap = new Tile[col + 2, row + 2]; // 외곽에 한 칸 비워둬야 함

        // 이미지 아이디 만들기
        List<int> imageIdList = new List<int>();

        for (int j = 0; j < totalTileCount / 2; j++)
        {
            for (int i = 0; i < 2; i++)
            {
                imageIdList.Add(j % imageTypeCount);
            }
        }

        // 타일 배치
        int tileId = 0;
        for (int x = 1; x < col + 1; x++)
        {
            for (int y = 1; y < row + 1; y++)
            {
                if (tileId >= totalTileCount) break; // 짝수가 안맞을수 있기 때문에 브레이크 해야 한다.
                // 데이터 생성
                int thisImageId = imageIdList[tileId % imageIdList.Count];
                Tile.TileCoords coords = new Tile.TileCoords(x, y);
                tileMap[x, y] = new Tile(tileId, thisImageId, coords, Tile.TileEnum.normal, true, false);
                tileId++;
            }
        }

        // 타일 카운트 적어주기
        leftTileCount.Value = totalTileCount;
    }

    /// <summary>
    /// 타일의 상태를 활성/비활성화 한다.
    /// </summary>
    /// <param name="tileId">타일 아이디</param>
    /// <param name="liveStatus">타일 상태 bool</param>
    public static void SetLive(int tileId, bool liveStatus)
    {
        foreach (Tile item in tileMap)
        {
            if (item == null) continue;

            if (tileId == item.TileID.Value)
            {
                item.TileLive.Value = liveStatus;
                if (liveStatus)
                {
                    leftTileCount.Value++;
                }
                else
                {
                    leftTileCount.Value--; // 남은 타일 개수 감소
                }
                
                break;
            }

        }
    }

    /// <summary>
    /// 선택한 타일을 반짝이게 만든다.
    /// 반짝이는 타일은 1개만 존재하므로 나머지는 끈다.
    /// </summary>
    /// <param name="tileId"></param>
    public static void GlowTile(int tileId)
    {
        foreach (Tile item in tileMap)
        {
            if (item == null) continue;

            if (tileId == item.TileID.Value)
            {
                item.TileGlow.Value = true;
            }
            else
            {
                item.TileGlow.Value = false;
            }

        }
    }

    /// <summary>
    /// 타일을 섞는다.
    /// </summary>
    public static void ShuffleTile()
    {
        // 기존 좌표 리스트를 만들어 저장
        List <Tile.TileCoords> coordsList = new List<Tile.TileCoords>();

        foreach(Tile tile in tileMap)
        {
            if (tile == null) continue;
            coordsList.Add(tile.Coords.Value);
        }

        // 리스트 섞기
        System.Random range = new System.Random();
        int n = coordsList.Count;
        while(n > 1)
        {
            n--;
            int k = range.Next(n + 1);
            Tile.TileCoords value = coordsList[k];
            coordsList[k] = coordsList[n];
            coordsList[n] = value;
        }

        // 새 값 쓰기
        foreach(Tile tile in tileMap)
        {
            // data가 null 이면 처리하지 않음
            if (tile == null) continue;
            tile.Coords.Value = coordsList[0];
            coordsList.RemoveAt(0);
        }

        // 변경된 coords를 기반으로 tileMap 다시 만들기
        List<Tile> tileList = new List<Tile>();
        foreach(Tile tile in tileMap)
        {
            if (tile == null) continue;
            tileList.Add(tile);
        }

        // tileMap 덮어쓰
        foreach(Tile tile in tileList)
        {
            tileMap[tile.Coords.Value.X, tile.Coords.Value.Y] = tile;
        }
    }

    /// <summary>
    /// 힌트 타일
    /// </summary>
    /// <param name="clearTile">true면 타일을 자동으로 깨준다.</param>
    public static void HintTile(bool clearTile)
    {
        // 클리어 가능한 타일 가져오기
        List<Tile> clearbleList = MatchModel.ClearableTiles(tileMap);

        //if (clearbleList == null)
        //{
        //    Debug.LogError("사용할 힌트 없음");
        //}

        // 맨 앞에 2개만 가져오면 된다.
        // 노드 가져오기
        List<Node> tileNode = MatchModel.RunPathFinding(clearbleList[0].Coords.Value.X, clearbleList[0].Coords.Value.Y, clearbleList[1].Coords.Value.X, clearbleList[1].Coords.Value.Y);

        // line 그리기 테스트 // todo: hint는 다른거 깨질 때까지 계속 표시해야 함
        //LineController.Instance.Init(tileNode, Color.green);
        if (clearTile)
        {
            MatchModel.drawNode.SetValueAndForceNotify(new DrawNode(Color.green, 0.275f,false));
            clearbleList[0].TileLive.Value = false;
            clearbleList[1].TileLive.Value = false;
        }
        
    }


}
