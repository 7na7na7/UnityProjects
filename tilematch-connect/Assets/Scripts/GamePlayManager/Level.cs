using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레벨 정보를 가지고 있다.
/// 게임 플레이 시간, 필드의 사이즈, 타일의 종류 등의 정보를 담는다.
/// </summary>
public class Level
{
    public int col, row;
    public int limitTime; // 남은(제한) 시간. seconds
    public int tileSize; // 타일 사이즈
    public int tileComplexSize; // 타일 종류(max 30)

    public void Init(int _col, int _row, int _leftTime, int _tileSize, int _tileComplexSize)
    {
        if (_tileComplexSize > 30)
        {
            Debug.LogError("타일 컴플렉스 사이즈 초과");
        }
        int maxTileSize = (_col * _row) - (_col * _row % 2);
        if (maxTileSize < _tileSize )
        {
            Debug.LogError("요청한 타일 사이즈가 너무 많음");
        }

        col = _col;
        row = _row;
        limitTime = _leftTime;
        tileSize = _tileSize;
        tileComplexSize = _tileComplexSize;
    }
}
