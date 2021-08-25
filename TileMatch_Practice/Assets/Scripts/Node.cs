using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 길찾기용 Node 클래스
/// </summary>
public class Node
{
    public bool isWall; //벽인가?
    public Node prev; //이전 노드
    public List<Node> neighbors; //인접해 있는 이웃 노드리스트
    public int x, y, G, H; //x좌표,y좌표, G(시작노드부터 해당 노드까지의 실제소요코스트), H(해당 노드에서 최종 목적지까지 도달하는데 소요될 값)
    public bool directionChanged;
    public int totalDirectionChanged;
    public int turn; //회전수, 초기값 0
    public double F; //G + H

    public Node(bool _isWall, int _x, int _y)
    {
        isWall = _isWall;
        x = _x;
        y = _y;
        directionChanged = false;
        neighbors = new List<Node>();
        turn = 0;
    }
}