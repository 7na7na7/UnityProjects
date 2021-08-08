using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point //노드의 위치를 담고 있
{
    public int x;
    public int y;

    public Point(int nx, int ny)
    {
        x = nx;
        y = ny;
    }

    //곱하기 메서드(자기자신)
    public void multiply(int m)
    {
        x *= m;
        y *= m;
    }

    //더하기 메서드(자기자신)
    public void add(Point o)
    {
        x += o.x;
        y += o.y;
    }

    //자기자신을 벡터로 바꿔 리터
    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }

    //같은지 확인
    public bool Equals(Point p)
    {
        return (x == p.x && y == p.y);
    }
    //벡터값을 주면 Point로 변환해 리턴해주는 메서드
    public static Point fromVector(Vector2 v)
    {
        return new Point((int)v.x, (int)v.y);
    }
    
    //곱하기 메서드
    public static Point multiply(Point p,int m)
    {
        return new Point(p.x * m, p.y * m);
    }

    //더하기 메서드
    public static Point add(Point p, Point o)
    {
        return new Point(p.x+o.x, p.y +o.y);
    }

    //포인트 복제
    public static Point clone(Point p)
    {
        return new Point(p.x, p.y);
    }

    //방향
    public static Point zero
    {
        get { return new Point(0, 0); }
    }
    public static Point up
    {
        get { return new Point(0, 1); }
    }
    public static Point down
    {
        get { return new Point(0, -1); }
    }
    public static Point right
    {
        get { return new Point(1, 0); }
    }
    public static Point left
    {
        get { return new Point(-1, 0); }
    }
}
