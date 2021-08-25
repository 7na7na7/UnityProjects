using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 선택한 타일 구조체
/// </summary>
public struct PickTile
{
    public int X, Y, ImageID, TileID;
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
    public Color lineColor;
    public float lineLiveTime;

    public DrawNode(Color color, float time,bool isMatched)
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
    private static bool isFirstPick = true;

    static Node[,] NodeArray;
    static Node StartNode, TargetNode, CurNode;
    static List<Node> OpenList, ClosedList;
    public static List<Node> FinalNodeList;
    public static Vector2Int bottomLeft, topRight, startPos, targetPos;
    public static PickTile firstTile;

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
    public static void PickTile(int x, int y, int imageId, int tileId)
    {
        //Debug.Log($"firstpick? {isFirstPick}");
        if (isFirstPick)
        {
            firstTile = new PickTile(x, y, imageId, tileId);
            FieldModel.GlowTile(tileId);
            isFirstPick = false;
        }
        else
        {
            // 두 번째 픽(매칭 여부 검토)
            // 이미지가 서로 같은지 체크
            if (firstTile.ImageID == imageId && firstTile.TileID != tileId)
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
                    drawNode.SetValueAndForceNotify(new DrawNode(Color.yellow, 0.275f,true));//맞으면 노랑으로 선을 표시해 준다! 그리고 1초 유지된다.
                    //이걸로 FieldPresenter에 있는 구독이 탐지할 수 있게 된다!

                    FieldModel.GlowTile(-1);
                    isFirstPick = true;
                }
                else
                {
                    // 경로 안 맞음
                    //IngameUIPresenter.DrawLine(FinalNodeList, Color.red, 0.5f); // debug
                    //drawNode.Value = new DrawNode(Color.red, 3.0f);
                    drawNode.SetValueAndForceNotify(new DrawNode(Color.red, 0.5f,false)); //틀리면 빨강으로 선을 표시해 준다! 그리고 0.5초동안 짧게 유지된다.
                    //FieldModel.SetLive(firstTile.TileID, false);
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
                isFirstPick = false;
            }

        }
    }


    /// <summary>
    /// 선택한 타일 매칭
    /// </summary>
    /// <param name="startTile"></param>
    /// <param name="endTile"></param>
    /// <returns></returns>
    public static bool MatchTile(int dptX, int dptY, int avlX, int avlY)
    {
        // 경로 찾기 요청
        List<Node> FinalNodeList = RunPathFinding(dptX, dptY, avlX, avlY);
        if (FinalNodeList == null) return false;

        PathLog(FinalNodeList);
        int totalDirectionCount = TotalDirectionCount(FinalNodeList);

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
        List<Node> forwardPath = PathFinding(dptX, dptY, avlX, avlY);
        List<Node> backwardPath = PathFinding(avlX, avlY, dptX, dptY);

        /*
        for(int i=0;i<backwardPath.Count;i++)
        {
            if(backwardPath[i].isWall)
                id=backwardPath[i].is
        }
        return forwardPath;
        */
        if (TotalDirectionCount(forwardPath) < TotalDirectionCount(backwardPath)) //횟수가 더 작은것을 선택
        {
            return forwardPath;
        }
        else if (TotalDirectionCount(forwardPath) == TotalDirectionCount(backwardPath)) //횟수가 같으면 최단경로를 선택
        {
            try
            {
                return (forwardPath.Count <= backwardPath.Count) ? forwardPath : backwardPath;
            }
            catch
            {
                Debug.Log("연결할 수 없어요!");
                return null;
            }
        }
        else
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
        /*
        Debug.Log("<color=cyan>match: ================================</color>");
        foreach(Node node in nodes)
        {
            Debug.Log($"x: {node.x} y: {node.y} t: {node.turn}");
        }
        */
    }

    /// <summary>
    /// 최종 노드에서 꺾임이 몇개인지 가져옴
    /// </summary>
    /// <param name="finalNodeList"></param>
    /// <returns></returns>
    private static int TotalDirectionCount(List<Node> finalNodeList)
    {
        if (finalNodeList == null) return 0;
        return finalNodeList[finalNodeList.Count - 1].turn;
    }

    public static List<Node> PathFinding(int dptX, int dptY, int avlX, int avlY) 
    {
        //sizeX = topRight.x - bottomLeft.x + 1;
        //sizeY = topRight.y - bottomLeft.y + 1;
        int sizeX = FieldModel.tileMap.GetLength(0);
        int sizeY = FieldModel.tileMap.GetLength(1);
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
        
        /*
        Debug.Log("----------------------------------------");
        string a = "";
     for(int i=7;i>0;i--)
        {
            a = "";
            for(int j=1;j<6;j++)
            { 
                a += NodeArray[j, i].isWall == true ? "ㅁ" : "ㄴ" + " "; //월 체크용 코드
            }
            Debug.Log(a + "\n");
        }
        */
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
                if (i+1 < sizeX) //오른쪽
                {
                    NodeArray[i, j].neighbors.Add(NodeArray[i + 1, j]);
                }
                if (j+1 < sizeY) //위쪽
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
        while(true)
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

            // 이웃 처리? 코드를 봐야 할듯
            List<Node> neighbors = CurNode.neighbors;
            for (int i = 0; i < neighbors.Count; i++) 
            {
                Node n = neighbors[i];
                if (n == null || ClosedList.Contains(n) || n.isWall == true) //이미 닫힌 이웃(갈수없는 이웃)이면
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

    private static double Heuristic(Node start, Node end)
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
