using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 0;
    public int rows = 8; //게임 보드의 크기, 바꾸면 더 크거나 작게 만들 수 있음
    public Count wallcount = new Count(5, 9); //레벨 당 최소 5개에서 최대 9개의 벽
    public Count foodCount = new Count(1, 5); //레벨 당 최소 1개에서 5개의 음식
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder; //이거의 자식으로 꺠끗이 정렬할 수 있도록!
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear(); //리스트 초기화
        for (int x = 1; x < columns - 1; x++)//가로
        {
            for (int y = 1; y < rows - 1; y++)//세로
            {
                gridPositions.Add(new Vector3(x,y,0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder=new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)//가로
        {
            for (int y = -1; y < rows + 1; y++) //세로
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; //무작위로 바닥 타일 선택
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)]; //가장자리라면 가장자리타일로 오브젝트를 변경

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject; //생성
                
                instance.transform.SetParent(boardHolder); //부모를 boardHolder로 하여 생성
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int mininum, int maximum)
    {
        int objectCount = Random.Range(mininum, maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles,wallcount.minimum,wallcount.maximum);
        LayoutObjectAtRandom(foodTiles,foodCount.minimum,foodCount.maximum);
        int enemyCount = (int) Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles,enemyCount,enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
