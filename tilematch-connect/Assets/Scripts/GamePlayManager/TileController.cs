using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 타일 게임 로직에 대한 정보가 있는 곳
/// </summary>
public class TileController : MonoBehaviour
{
    // 타일 게임 로직
    private static TilePresenter pickedObject;
    private static bool isFirstPick;

    // 타일 맵 정보를 가지고 있는다.
    public static List<Tile> tileDataList = new List<Tile>();


    // 타일 찾기 부분(astar 알고리즘)
    public static Vector2Int bottomLeft, topRight, startPos, targetPos;
    public static List<Node> FinalNodeList;

    static int sizeX, sizeY;
    static Node[,] NodeArray;
    static Node StartNode, TargetNode, CurNode;
    static List<Node> OpenList, ClosedList;

    // Start is called before the first frame update
    void Awake()
    {
        isFirstPick = true;
        pickedObject = null;
    }

    // 타일 선택
    public static void TilePick(TilePresenter obj)
    {
        if (isFirstPick)
        {
            // 타일 정보만 저장하고 넘어감
            pickedObject = obj;
            // 애니메이션 재생
            //obj.tileAnimation.
            obj.tileAnimator.SetTrigger("Select");
            obj.SetGlow(true);
            isFirstPick = false;
        }
        else
        {
            // FirstPick 에 자료가 있다면 로직을 비교해서 처리한다.
            // 이미지가 서로 같은지 체크
            if (pickedObject.ImageID == obj.ImageID && pickedObject.TileID != obj.TileID)
            {
                // 출발지와 목적지의 좌표를 담는다.
                //bool isCorrect = PathFinding(pickedObject.X, pickedObject.Y, obj.X, obj.Y, true);
                bool isCorrect = MatchTile(pickedObject, obj, true, FieldDesignManager.tileMap);
                //Debug.Log(isCorrect);
                //isCorrect = false; // 디버그용.
                if (isCorrect)
                {
                    // 같으면 없애기
                    GameAudioManager.Instance.PlaySound(GameAudioManager.SoundTag.FX_Tile_Dead);

                    // 타일을 깼으면 깨진 타일의 필드맵을 업데이트 해줘야 한다.
                    FieldDesignManager.tileMap[pickedObject.X, pickedObject.Y] = null;
                    FieldDesignManager.tileMap[obj.X, obj.Y] = null;

                    // 못깨는 타일이 생겼는지 체크한다.
                    UseItem.AfterTileAction(FieldDesignManager.tileMap);

                    // 클리어 애니메이션
                    pickedObject.GetComponent<Transform>().DOScale(0.0f, 0.3f).SetEase(Ease.InOutBack).OnComplete(() =>
                    {
                        Destroy(pickedObject.gameObject);
                        pickedObject = null;
                    });
                    obj.GetComponent<Transform>().DOScale(0.0f, 0.3f).SetEase(Ease.InOutBack).OnComplete(() =>
                    {
                        Destroy(obj.gameObject);
                    });
                }
                else
                {
                    // 안 맞는 경우
                    pickedObject.tileAnimator.SetTrigger("Idle");
                    pickedObject.SetGlow(false);
                    isFirstPick = false;
                }
            }
            else
            {
                // 이미지가 다른걸 선택한 경우
                //pickedObject.tileAnimation.Stop();
                pickedObject.tileAnimator.SetTrigger("Idle");
                pickedObject.SetGlow(false);
            }
            isFirstPick = true;
        }
    }


    /// <summary>
    /// 선택한 타일 매칭
    /// </summary>
    /// <param name="startTile"></param>
    /// <param name="endTile"></param>
    /// <returns></returns>
    public static bool MatchTile(TilePresenter startTile, TilePresenter endTile, bool lineOption, Tile[,] tileMap)
    {
        // 경로 찾기 요청
        List<Node> FinalNodeList = PathFinding(startTile.X, startTile.Y, endTile.X, endTile.Y, tileMap);
        if (FinalNodeList == null) return false;

        int totalDirectionCount = TotalDirectionCount(FinalNodeList);

        // 경로 시각화 // todo: 이 위치에 놓는것을 고민해 봐야 한다.
        if (totalDirectionCount < 3)
        {
            if (lineOption)
            {
                LineController.Instance.Init(FinalNodeList, Color.yellow);
            }
            // 별 만들기
            StarController.Instance.Init(FinalNodeList);
            
            return true;
        }
        else
        {
            // 타일은 매칭됐으나 경로가 없는 경우
            //if (lineOption)
            //    LineController.Instance.Init(FinalNodeList, Color.red);
            GameAudioManager.Instance.PlaySound(GameAudioManager.SoundTag.UI_BTN_Squeeky);
            return false;
        }
    }

    /// <summary>
    /// 최종 노드에서 꺾임이 몇개인지 가져옴
    /// </summary>
    /// <param name="finalNodeList"></param>
    /// <returns></returns>
    private static int TotalDirectionCount(List<Node> finalNodeList)
    {
        int totalDirectionCount = 0;
        for (int i = 0; i < FinalNodeList.Count; i++)
        {
            //print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y + "방향 꺾임: " + FinalNodeList[i].directionChangeCounter);
            if (FinalNodeList[i].directionChanged)
            {
                totalDirectionCount++;
            }
        }
        return totalDirectionCount;
    }

    /// <summary>
    /// 길찾기 알고리즘
    /// </summary>
    /// <param name="dptX">시작 x좌표</param>
    /// <param name="dptY">시작 y좌표</param>
    /// <param name="avlX">도착 x좌표</param>
    /// <param name="avlY">도착 y좌표</param>
    /// <param name="tileMap">대상이 될 타일맵</param>
    /// <returns>노드를 반환한다.</returns>
    public static List<Node> PathFinding(int dptX, int dptY, int avlX, int avlY, Tile[,] tileMap)
    {
        //sizeX = topRight.x - bottomLeft.x + 1;
        //sizeY = topRight.y - bottomLeft.y + 1;
        sizeX = tileMap.GetLength(0);
        sizeY = tileMap.GetLength(1);
        NodeArray = new Node[sizeX, sizeY];

        // 장애물 체크
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                // 가장자리 초기화
                if (i == 0 || j == 0 || i == sizeX - 1 || j == sizeY - 1)
                {
                    NodeArray[i, j] = new Node(false, i, j);
                    continue;
                }

                bool isWall = false;
                // 타일맵 정보를 읽어와서 벽을 만든다.
                if (FieldDesignManager.tileMap[i, j] != null)
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

        // 시작과 끝 노드, 열린 리스트와 닫힌 리스트, 마지막 리스트 초기화
        StartNode = NodeArray[dptX, dptY];
        TargetNode = NodeArray[avlX, avlY];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        // 반복하며 길 찾기
        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 마지막
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

            // 진행 방향을 같이 넘겨준다.
            // 이전 노드의 정보가 있어야됨.
            // ↑:0, →:1, ↓:2, ←:3
            OpenListAdd(CurNode.x, CurNode.y + 1, CurNode, CurNode.prev, 0);
            OpenListAdd(CurNode.x + 1, CurNode.y, CurNode, CurNode.prev, 1);
            OpenListAdd(CurNode.x, CurNode.y - 1, CurNode, CurNode.prev, 2);
            OpenListAdd(CurNode.x - 1, CurNode.y, CurNode, CurNode.prev, 3);
        }
        // 못찾음
        return null;
    }

    /// <summary>
    /// 길찾기 시에 탐색 가능한 장소를 추가하는 메소드 -> 오픈리스트 추가
    /// </summary>
    /// <param name="checkX"></param>
    /// <param name="checkY"></param>
    /// <param name="currentNode"></param>
    /// <param name="parentNode"></param>
    /// <param name="direction"></param>
    static void OpenListAdd(int checkX, int checkY, Node currentNode, Node parentNode, int direction)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY && !NodeArray[checkX, checkY].isWall && !ClosedList.Contains(NodeArray[checkX, checkY]))
        {
            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

            // 현재 노드가 직진이 아니면 비용 증가
            bool isChangedDirection = false;
            // 총 꺾임 수에 따라 누적으로 패널티를 줘야 함
            // 진행 방향(이웃 노드), 현재 노드, 부모 노드를 참조하여 방향이 꺾이는지 체크하고 꺾인다면 이웃노드에 표시한다.

            //if (parentNode != null)
            //{
            //    isChangedDirection = true;
            //    // 세 노드의 x 혹은 y가 둘 다 같지 않으면 꺾임
            //    if (currentNode.x == parentNode.x && parentNode.x == NeighborNode.x)
            //        if (currentNode.y == parentNode.y && parentNode.y == NeighborNode.y)
            //        {
            //            isChangedDirection = false;
            //        }
            //}
            //else
            //{
            //    // 부모가 없으면 시작 상태므로 방향 변경 없음
            //    isChangedDirection = false;
            //}

            if (parentNode != null)
            {
                switch (direction)
                {
                    // up, down
                    case 0:
                    case 2:
                        if (currentNode.x != parentNode.x)
                        {
                            // 방향 바뀜
                            //MoveCost += 10000 * (changedCounter+1);
                            isChangedDirection = true;
                            Debug.Log($"방향 변경 현재x: {currentNode.x}, 부모x: {parentNode.x} {MoveCost}");
                        }
                        break;
                    // left, right
                    case 1:
                    case 3:
                        if (currentNode.y != parentNode.y)
                        {
                            // 방향 바뀜
                            //MoveCost += 10000 * (changedCounter + 1);
                            isChangedDirection = true;
                            Debug.Log($"방향 변경 현재y: {currentNode.y}, 부모y: {parentNode.y} {MoveCost}");
                        }
                        break;
                }
            }

            // 휴리스틱 가중치를 만든다. 여러번 꺾일 수록 가중치가 심해져야 한다.
            // 이전 노드에 꺾임이 있었는지 체크
            int changedCounter = DirectionChangedCounter(parentNode, 0);
            if (currentNode.directionChanged) changedCounter++;
            if (isChangedDirection) changedCounter++;

            int additionalH = 0;
            additionalH = 1000 * changedCounter;


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10 + additionalH;
                NeighborNode.prev = CurNode;
                if (isChangedDirection)
                {
                    NeighborNode.directionChanged = true;
                }

                // 오픈 리스트 추가
                OpenList.Add(NeighborNode);

            }
        }
    }

    /// <summary>
    /// 길찾기 node 중 몇 번이나 꺾는지 체크하는 counter
    /// </summary>
    /// <param name="node">node</param>
    /// <param name="counter">0부터 시작해야 함</param>
    /// <returns>카운트 결과</returns>
    private static int DirectionChangedCounter(Node node, int counter )
    {
        if (node == null)
        {
            return counter;
        }

        if (node.directionChanged) counter++;

        return DirectionChangedCounter(node.prev, counter);
    }

    /**
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
            // 이미 체크한 타일 아이디인지 확인
            bool isChecked = checkedImageId.Contains(tile.TileID);
            if (isChecked) continue;

            // 이미지 매칭되는 것을 찾아온다.
            List<Tile> matchTileList = MatchedImage(tile.ImageID, tileMap);

            // 해당 쌍을 돌며 매칭 시작
            for (int i = 0; i < matchTileList.Count; i += 2)
            {
                // 자신것과 다음것에 경로가 있는지 찾기 시작
                List<Node> finalNode = PathFinding(matchTileList[i].X, matchTileList[i].Y, matchTileList[i + 1].X, matchTileList[i + 1].Y, tileMap);

                // 노드 꺾임이 2 이하인 것으로 찾기
                if (finalNode != null && TotalDirectionCount(FinalNodeList) < 3)
                {
                    returnList.Add(matchTileList[i]);
                    returnList.Add(matchTileList[i+1]);
                }
                // 이미 찾은 아이디는 찾은 아이디 리스트에 추가
                checkedImageId.Add(matchTileList[i].TileID);
                checkedImageId.Add(matchTileList[i+1].TileID);
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

        foreach(Tile tile in tileMap)
        {
            if (tile == null) continue;
            if (tile.ImageID == imageId)
                returnTileList.Add(tile);
        }
            
        return returnTileList;
    }
    */

}