using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class LevelModel
{
    public ReactiveProperty<int> limitTime { get; }

    public LevelModel(int mmr)
    {
        // todo: mmr에 따라서 다른 값들로 초기화됨
        limitTime = new ReactiveProperty<int>(120);
    }
}
