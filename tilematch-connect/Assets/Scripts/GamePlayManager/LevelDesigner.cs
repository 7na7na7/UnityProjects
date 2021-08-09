using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레벨을 불러와 메모리에 올린다.
/// 레벨에 관련한 데이터는 여기서만 불러온다.
/// </summary>
public class LevelDesigner : MonoBehaviour
{
    public static Level thisLevel;

    /// <summary>
    /// mmr을 받아서 적정한 레벨 난이도를 리턴해준다.
    /// </summary>
    /// <param name="mmr"></param>
    /// <param name="userLevel"></param>
    public static void Init(int mmr, int userLevel)
    {
        Level _level = new Level();
        _level.Init(5, 7, 200, 34, 7);
        thisLevel = _level;
    }

    // todo: 타일 모양도 보내줘야 함
    /// <summary>
    /// 타일맵 정보를 받아서 모양을 다듬는다. (균형잡힌 모양을 만들기 위함)
    /// </summary>
    /// <param name="tileMap"></param>
    /// <returns></returns>
    public static Tile[,] Shape(Tile [,] tileMap)
    {
        return null;
    }
}
