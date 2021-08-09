using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 길찾기용 Node 클래스
/// </summary>
public class Node
{
    public bool isWall;
    public Node prev;
    public List<Node> neighbors;
    public int x, y, G, H;
    public bool directionChanged;
    public int totalDirectionChanged;
    public int turn;
    public double F;

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