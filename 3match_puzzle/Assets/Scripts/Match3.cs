using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour
{
    public int width = 9;
    public int height = 14;

    Node[,] board;

    System.Random random;
    private void Start()
    {
        
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
                board[x, y] = new Node(-1,new Point(x,y));
            }
        }
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