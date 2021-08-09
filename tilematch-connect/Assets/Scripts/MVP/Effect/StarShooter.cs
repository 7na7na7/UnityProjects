using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using System;

/// <summary>
/// 인게임 내 별을 그린다.
/// </summary>
public class StarShooter : MonoBehaviour
{
    //public GameObject prefabStar;

    public IObservable<Unit> ShootStar(RectTransform targetStarPos, Vector3 initPos, bool isMatched)
    {
        if (!isMatched)
            return Observable.Timer(TimeSpan.FromSeconds(0f)).ForEachAsync(_=>{ });

        // 시작 위치 세팅
        RectTransform _goRectTransform = gameObject.GetComponent<RectTransform>();
        _goRectTransform.anchoredPosition = initPos;

        // 별 날리기
        Sequence starSeq = DOTween.Sequence();
        starSeq.Append(gameObject.transform.DORotate(new Vector3(0, 360, 0), 0.25f, RotateMode.FastBeyond360));
        starSeq.Append(gameObject.transform.DORotate(new Vector3(0, 180, 0), 0.5f, RotateMode.FastBeyond360));
        starSeq.Join(gameObject.transform.DOMove(targetStarPos.position, 1f).SetEase(Ease.InOutCirc));
        starSeq.Join(gameObject.transform.DOScale(0.7f, 0.1f));
        //starSeq.Append(gameObject.transform.DOScale(0.0f, 0.2f)); // 자연스럽게 없어지긴 하는데 별이 이상해진다.

        return Observable.Timer(TimeSpan.FromSeconds(2.0f))
                .ForEachAsync(_ => { });
    }

    
}
