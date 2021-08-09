using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;
    public Sprite[] pieces;
    int width = 9;
    int height = 14;

    Node[,] board;

    System.Random random;
    private void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        board = new Node[width, height];

        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());

        initializeBoard();
    }

    void initializeBoard()
    {
        board = new Node[width, height];
        for(int y=0;y<height;y++)
        {
            for(int x=0;x<width;x++)
            {
                board[x, y] = new Node((boardLayout.rows[y].row[x])?-1:fillPiece(),new Point(x,y));
            }
        }
    }

    void VerifyBoard() //매치가 있는지 검사
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Point p = new Point(x, y);
                int val = getValueAtPoint(p);
                if (val <= 0) continue; //비어있으면 넘김


            }
        }
    }

    int getValueAtPoint(Point p) //포인트가 어떤 도형인지 value리
    {
        return board[p.x, p.y].value;
    }

    int fillPiece() //채우는거
    {
        int val = 1;
        val = random.Next(0, 100)/(100/pieces.Length)+1; //1~5 반환
        return val;
    }

    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIGKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for(int i=0;i<20;i++)
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
       
        return seed;
    }
}

[System.Serializable]
public class Node
{
    public int value; //0=빈칸, 1=네모, 2=동그라미, 3=동그라미통, 4=세모, 5=다이아몬, -1=구멍
    public Point index;
    public Node(int v,Point i)
    {
        value = v;
        index = i;
    }
}