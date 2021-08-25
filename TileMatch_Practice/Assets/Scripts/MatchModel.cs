using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 선택한 타일 구조체
/// </summary>
public struct PickTile //픽한 타일 데이터를 저장할 구조체 
{
    public int X, Y, ImageID, TileID; //타일 선택 시 TilePresenter에서 넘겨주는 값
    public PickTile(int x, int y, int imageId, int tileId)
    {
        X = x;
        Y = y;
        ImageID = imageId;
        TileID = tileId;
    }
}

/// <summary>
/// 라인 및 별을 그리기 위한 정보
/// </summary>
public struct DrawNode
{
    public bool lineIsMatched;
    public Color lineColor; //라인 색 설정
    public float lineLiveTime; //라인이 유지되는 시간

    public DrawNode(Color color, float time, bool isMatched)
    {
        lineIsMatched = isMatched;
        lineColor = color;
        lineLiveTime = time;
    }
}

/// <summary>
/// 게임 타일 처리에 대한 모델
/// </summary>
public static class MatchModel
{
    private static bool isFirstPick = true; //첫번쨰 고른 상태인가?

    static Node[,] NodeArray; //노드배열
    static Node StartNode, TargetNode, CurNode; //시작노드, 타겟노드, 현재노드
    static List<Node> OpenList, ClosedList; //갈수있는 노드리스트들과 갈수없는 노드리스트들
    public static List<Node> FinalNodeList; //길찾기를 거친 최종 노드리스트
    public static Vector2Int bottomLeft, topRight, startPos, targetPos; //길찾기가능범위 그리드를 알게 해주는 왼쪽아래와 오른쪽위 모서리
    public static PickTile firstTile; //픽한 첫번째 타일 

    // 라인 및 별을 그리기 위한 용도
    //public static BoolReactiveProperty drawNode = new BoolReactiveProperty(false);
    public static ReactiveProperty<DrawNode> drawNode = new ReactiveProperty<DrawNode>(); //UniRX활용
    public static ReactiveProperty<int> nodeStarAmount = new ReactiveProperty<int>(); //new IntReactiveProperty();로 해도 같다

    /// <summary>
    /// 타일 선택
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="imageId"></param>
    /// <param name="tileId"></param>
    public static void PickTile(int x, int y, int imageId, int tileId) //타일 고르기 
    {
        //Debug.Log($"firstpick? {isFirstPick}");
        if (isFirstPick)
        {
            firstTile = new PickTile(x, y, imageId, tileId); //첫번째 고른 타일 저장
            FieldModel.GlowTile(tileId); //선택한 타일을 반짝거리게 함
            isFirstPick = false; //이제 다음선택할 타일은 첫번째 타일이 아니니까...
        }
        else //두번째에서는 두 개가 동일할지 여부를 판단해야 하지 때문에 더 복잡해진다.
        {
            // 두 번째 픽(매칭 여부 검토)
            // 이미지가 서로 같은지 체크
            if (firstTile.ImageID == imageId && firstTile.TileID != tileId) //두 개의 이미지아이디가 같고, 동일한 타일이 아니라면 매치 대상이 된다!
            {
                // 경로가 맞는지 체크
                bool isCorrect = MatchTile(firstTile.X, firstTile.Y, x, y);
                //Debug.Log(isCorrect);
                if (isCorrect)
                {
                    // 경로 맞음
                    // 라인 그려주기
                    //IngameUIPresenter.DrawLine(FinalNodeList, Color.yellow, 1.0f);
                    //drawNode.Value = true;
                    //drawNode.Value = new DrawNode(Color.yellow, 3.0f);

                    // 두 타일 비활성화 하기
                    FieldModel.SetLive(firstTile.TileID, false);
                    FieldModel.SetLive(tileId, false);
                    //SetValueAndForceNotify - 같은 값을 넘겨도 같은 값으로 제대로 통지해 준다.
                    drawNode.SetValueAndForceNotify(new DrawNode(Color.yellow, 0.275f, true));//맞으면 노랑으로 선을 표시해 준다! 0.275초 동안 유지된다.
                    //이걸로 FieldPresenter에 있는 구독이 탐지할 수 있게 된다!

                    FieldModel.GlowTile(-1);
                    isFirstPick = true;
                }
                else
                {
                    // 경로 안 맞음
                    //IngameUIPresenter.DrawLine(FinalNodeList, Color.red, 0.5f); // debug
                    //drawNode.Value = new DrawNode(Color.red, 3.0f);
                    drawNode.SetValueAndForceNotify(new DrawNode(Color.red, 0.5f, false)); //틀리면 빨강으로 선을 표시해 준다! 0.5초 동안 유지된다.
                    //FieldModel.SetLive(firstTile.TileID, false)
                    //FieldModel.SetLive(tileId, false);
                    FieldModel.GlowTile(-1);
                    isFirstPick = true;
                }
            }
            else
            {
                // 매칭되지 않는 다른 이미지를 선택함
                firstTile = new PickTile(x, y, imageId, tileId);
                FieldModel.GlowTile(tileId);
                isFirstPick = false; //원래코드 : isFirstPick = true에서 고침
            }

        }
    }


    /// <summary>
    /// 선택한 타일 매칭
    /// </summary>
    /// <param name="startTile"></param>
    /// <param name="endTile"></param>
    /// <returns></returns>
    public static bool MatchTile(int dptX, int dptY, int avlX, int avlY) //넘겨준 두 좌표의 위치에 있는 타일들이 서로 매치가 될 수 있는가?
    {
        // 경로 찾기 요청
        List<Node> FinalNodeList = RunPathFinding(dptX, dptY, avlX, avlY); //길찾기를 실행해 최단 경로를 FinalNodeList에 저장
        if (FinalNodeList == null) return false; //최단 경로가 비었다면(제대로 길찾기를 하지 못했다면) false반환

        PathLog(FinalNodeList);
        int totalDirectionCount = TotalDirectionCount(FinalNodeList); //회전수 계 

        // 경로 시각화 // todo: 이 위치에 놓는것을 고민해 봐야 한다.
        if (totalDirectionCount < 3)
        {
            //if (lineOption)
            //{
            //    LineController.Instance.Init(FinalNodeList, Color.yellow);
            //}
            //LineController.Instance.Init(FinalNodeList, Color.yellow);
            // 별 만들기
            //StarController.Instance.Init(FinalNodeList);
            //GameAudioManager.Instance.PlaySound(GameAudioManager.SoundTag.UI_BTN_Squeeky);

            return true;
        }
        else
        {
            // 타일은 매칭됐으나 경로가 없는 경우
            //if (lineOption)
            //    LineController.Instance.Init(FinalNodeList, Color.red);
            //GameAudioManager.Instance.PlaySound(GameAudioManager.SoundTag.UI_BTN_Squeeky);

            return false;
        }
    }

    /// <summary>
    /// 길찾기를 목적지와 도착지를 변경해서 교차 검증을 하고 더 나은쪽 결과를 돌려준다.
    /// </summary>
    /// <param name="dptX"></param>
    /// <param name="dptY"></param>
    /// <param name="avlX"></param>
    /// <param name="avlY"></param>
    /// <returns></returns>
    public static List<Node> RunPathFinding(int dptX, int dptY, int avlX, int avlY)
    {
        //시작 노드 좌표 xy와 마지막 노드 좌표 xy를 받음
        List<Node> forwardPath = PathFinding(dptX, dptY, avlX, avlY); //시작타일부터 목표타일까지 길찾기한 노드리스트
        List<Node> backwardPath = PathFinding(avlX, avlY, dptX, dptY); //목표타일부터 시작타일까지 길찾기한 노드리스트

        if (TotalDirectionCount(forwardPath) < TotalDirectionCount(backwardPath)) //회전수가 더 작은것을 선택
        {
            return forwardPath;
        }
        else if (TotalDirectionCount(forwardPath) == TotalDirectionCount(backwardPath)) //회전수가 같으면 최단경로를 선택
        {
            return (forwardPath.Count <= backwardPath.Count) ? forwardPath : backwardPath;
        }
        else //회전수가 더 적은것을 선택 
        {
            return backwardPath;
        }

    }

    /// <summary>
    /// 디버그용으로 사용되는 노드 추적기
    /// </summary>
    /// <param name="nodes"></param>
    public static void PathLog(List<Node> nodes)
    {
        Debug.Log("<color=cyan>match: ================================</color>");
        foreach(Node node in nodes)
        {
            Debug.Log($"x: {node.x} y: {node.y} t: {node.turn}");
        }
    }

    /// <summary>
    /// 최종 노드에서 꺾임이 몇개인지 가져옴
    /// </summary>
    /// <param name="finalNodeList"></param>
    /// <returns></returns>
    private static int TotalDirectionCount(List<Node> finalNodeList)
    {
        if (finalNodeList == null) return 0;
        return finalNodeList[finalNodeList.Count - 1].turn; //노드의 마지막에서 turn값이 어떻게 되었는지 반환
    }

    public static List<Node> PathFinding(int dptX, int dptY, int avlX, int avlY) //이게 실제로 길찾기 알고리즘을 수행하는 스크립트!
    {
        //sizeX = topRight.x - bottomLeft.x + 1;
        //sizeY = topRight.y - bottomLeft.y + 1;
        int sizeX = FieldModel.tileMap.GetLength(0); //tilemap[x,y]의 x
        int sizeY = FieldModel.tileMap.GetLength(1); //tilemap[x,y]의 y
        NodeArray = new Node[sizeX, sizeY];

        // 장애물을 체크해서 길찾기 지도를 만든다.
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                // 가장자리 초기화
                if (i == 0 || j == 0 || i == sizeX - 1 || j == sizeY - 1)
                {
                    NodeArray[i, j] = new Node(false, i, j); //가장자리는 전부 벽이 아님
                    continue;
                }
                bool isWall = false;
                // 타일맵 정보를 읽어와서 벽을 만든다.
                if (FieldModel.tileMap[i, j] != null && FieldModel.tileMap[i, j].TileLive.Value == true)
                {
                    isWall = true;
                }
                // 목적지는 벽에서 제외
                if (i == avlX && j == avlY)
                {
                    isWall = false;
                }

                NodeArray[i, j] = new Node(isWall, i, j);
            }
        }


        // 맵을 완성했으면 이웃을 추가한다.
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                // 조건에 따라 이웃 추가, 상 하 좌우 이웃을 다 추가해야 함
                if (i > 0) //왼쪽
                {
                    NodeArray[i, j].neighbors.Add(NodeArray[i - 1, j]);
                }
                if (j > 0) //아럐쪽
                {
                    NodeArray[i, j].neighbors.Add(NodeArray[i, j - 1]);
                }
                if (i + 1 < sizeX) //오른쪽
                {
                    NodeArray[i, j].neighbors.Add(NodeArray[i + 1, j]);
                }
                if (j + 1 < sizeY) //위쪽
                {
                    NodeArray[i, j].neighbors.Add(NodeArray[i, j + 1]);
                }
            }
        }


        // 시작과 끝 노드, 열린 리스트와 닫힌 리스트, 마지막 리스트 초기화
        StartNode = NodeArray[dptX, dptY];
        TargetNode = NodeArray[avlX, avlY];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        // 길찾기
        while (true)
        {
            if (OpenList.Count <= 0) return null; // 길 없음

            // 일단 노드 하나를 현재 노드로 선택
            CurNode = OpenList[0];

            //이동비용이 가장 적게 드는 것을 우선 선택
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            /*
            // 회전 수가 가장 적은것을 우선 선택 
            for (int i = 0; i < OpenList.Count; i++) 
            {
                if (OpenList[i].turn <= CurNode.turn)
                {
                    CurNode = OpenList[i];
                }
            }
            */
            // 이웃노드 탐색 같음
            if (CurNode.prev != null)
            {
                for (int i = 0; i < CurNode.neighbors.Count; i++)
                {
                    Node c = CurNode.neighbors[i];
                    if (ClosedList.Contains(c) && CurNode.prev != c && CurNode.turn > c.turn)
                    {
                        CurNode.prev = c;
                        CurNode.turn = c.turn;

                        if (c.prev != null && (c.y - c.prev.y != CurNode.y - c.y || c.x - c.prev.x != CurNode.x - c.x))
                        {
                            CurNode.turn++;
                        }
                    }
                }
            }

            // 결과에 도달 시
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.prev;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
                return FinalNodeList;
            }

            // 오픈 노드와 클로즈 노드 수정
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 갈수있는 이웃 검ㅅ
            List<Node> neighbors = CurNode.neighbors;
            for (int i = 0; i < neighbors.Count; i++)
            {
                Node n = neighbors[i];
                if (n == null || ClosedList.Contains(n) || n.isWall == true) //노드가 비었거나, 닫힌(갈수없는)리스트에 존재하거나, 벽이라면 갈수없음
                {
                    continue; //다음 이웃으로
                }

                int tg = CurNode.G + 1; //tg = 출발 지점까지의 거리 +1
                if (OpenList.Contains(n)) //이미 오픈리스트에 포함중이면
                {
                    n.G = (tg < n.G) ? tg : n.G; //G 재설정
                }
                else //포함되어 있지 않으면 G를 설정해 주고 오픈리스트에 포함시킴
                {
                    n.G = tg;
                    OpenList.Add(n);
                }

                n.turn = CurNode.turn; //회전 수 같게 해줌
                if (CurNode.prev != null && (CurNode.y - CurNode.prev.y != n.y - CurNode.y || CurNode.x - CurNode.prev.x != n.x - CurNode.x))
                {
                    n.turn++;
                }

                n.F = tg + Heuristic(n, TargetNode); //f=g+h
                n.prev = CurNode;

            }
        }

    }

    private static double Heuristic(Node start, Node end) //H - 시작부터 끝까지의 거리
    {
        return Math.Sqrt(Math.Pow(end.x - start.x, 2) + Math.Pow(end.y - start.y, 2)); //피타고라스로 스타트에서 엔드까지 거리 리턴
    }

    /// <summary>
    /// 깰 수 있는 타일이 몇개인지 알려주는 카운터.
    /// </summary>
    public static List<Tile> ClearableTiles(Tile[,] tileMap)
    {
        // 타일맵 복제
        Tile[,] checkTileMap = (Tile[,])tileMap.Clone();
        // 이미 체크한 쌍을 피해가기 위한 리스트
        List<int> checkedImageId = new List<int>();

        List<Tile> returnList = new List<Tile>();

        foreach (Tile tile in checkTileMap)
        {
            if (tile == null) continue;
            //if (tile.TileLive.Value == false) continue;
            // 이미 체크한 타일 아이디인지 확인
            bool isChecked = checkedImageId.Contains(tile.TileID.Value);
            if (isChecked) continue;

            // 이미지 매칭되는 것을 찾아온다.
            List<Tile> matchTileList = MatchedImage(tile.ImageID.Value, tileMap);

            // 해당 쌍을 돌며 매칭 시작
            for (int i = 0; i < matchTileList.Count; i += 2)
            {
                // 자신것과 다음것에 경로가 있는지 찾기 시작
                List<Node> finalNode = RunPathFinding(matchTileList[i].Coords.Value.X, matchTileList[i].Coords.Value.Y, matchTileList[i + 1].Coords.Value.X, matchTileList[i + 1].Coords.Value.Y);

                // 노드 꺾임이 2 이하인 것으로 찾기
                if (finalNode != null && TotalDirectionCount(FinalNodeList) < 3)
                {
                    returnList.Add(matchTileList[i]);
                    returnList.Add(matchTileList[i + 1]);
                }
                // 이미 찾은 아이디는 찾은 아이디 리스트에 추가
                checkedImageId.Add(matchTileList[i].TileID.Value);
                checkedImageId.Add(matchTileList[i + 1].TileID.Value);
            }
        }
        // 체크한 쌍은 검색에서 제외한다.
        return returnList;
    }

    /// <summary>
    /// 타일 맵에 있는 곳에서 이미지 아이디를 넘겨주면 매칭되는 타일 아이디를 반환한다.
    /// </summary>
    /// <param name="imageId"></param>
    /// <param name="tileMap"></param>
    /// <returns>타일 리스트</returns>
    private static List<Tile> MatchedImage(int imageId, Tile[,] tileMap)
    {
        List<Tile> returnTileList = new List<Tile>();

        foreach (Tile tile in tileMap)
        {
            if (tile == null) continue;
            if (tile.ImageID.Value == imageId)
                returnTileList.Add(tile);
        }

        return returnTileList;
    }

}
