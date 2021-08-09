using System;
using UniRx;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public IObservable<Unit> Explode(Tile.TileCoords pos)
    {
        var thisParticle = gameObject.GetComponent<ParticleSystem>();
        // 시작 위치 세팅
        gameObject.transform.position = new Vector2(pos.X + FieldModel.fieldXOffset, pos.Y + FieldModel.fieldYOffset);
        thisParticle.Play();

        return Observable.Timer(TimeSpan.FromSeconds(1.8f))
                .ForEachAsync(_ => { thisParticle.Stop(); });
    }
}
