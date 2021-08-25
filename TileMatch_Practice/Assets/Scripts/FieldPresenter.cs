using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class FieldPresenter : MonoBehaviour
{
    public Transform field;
    public GameObject tilePrefab;

    private Sprite[] tileImage;
    private Sprite[] tileBG;

    // line
    public GameObject prefabLine;
    public Transform worldForeground;
    public Transform worldBackground;

    // effect
    /*public Explosion prefabExplosion;

    // objectpool
    ObjectPooling<Explosion> explosionObjPool;*/

    private void Start()
    {
        #region 레벨 가져오기
        // todo: 레벨 모델에서 가져와야 한다.
        int col = 5;
        int row = 7;

        #endregion

        #region 타일 배치
        FieldModel.Init(col, row, 34, 17);

        // 타일에서 사용할 이미지 및 백그라운드 가져오기
        tileImage = Resources.LoadAll<Sprite>("Tileset/TileImg/sample-tile");
        tileBG = Resources.LoadAll<Sprite>("Tileset/TileBG/Tile-01");

        // 타일 오브젝트 리스트
        List<GameObject> goList = new List<GameObject>();

        // 모델에서 타일 생성
        //int i = 0;
        foreach (Tile tile in FieldModel.tileMap)
        {
            if (tile == null) continue;

            // 오브젝트 생성
            //goList.Add(Instantiate(tilePrefab, field));
            GameObject _go = Instantiate(tilePrefab, field);

            _go.GetComponent<TilePresenter>().Init(tileBG[0], tileImage[tile.ImageID.Value], tile.TileID.Value, tile.ImageID.Value, tile.Coords.Value.X, tile.Coords.Value.Y);

            // 타일을 필드에 붙이기
            _go.transform.SetParent(field);

            // 타일 순서 잡아주기
            _go.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.Coords.Value.X * tile.Coords.Value.Y;
            _go.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.Coords.Value.X * tile.Coords.Value.Y + 1;

            // 위치 배치
            _go.transform.position = new Vector2(tile.Coords.Value.X + FieldModel.fieldXOffset, tile.Coords.Value.Y + FieldModel.fieldYOffset);

            // 타일 구독을 나눠서 진행해야 함

            // glow 
            tile.TileGlow.Subscribe(x =>
            {
                //Debug.Log($"glow {x}");
                _go.transform.Find("Glow").gameObject.SetActive(x);
            })
            .AddTo(this);

            // set coords
            tile.Coords.Subscribe(coords =>
            {
                //Debug.Log($"coords: {coords.X}, {coords.Y} go {coords.X + FieldModel.fieldXOffset}, {coords.Y + FieldModel.fieldYOffset}");
                // 정보 변경
                _go.GetComponent<TilePresenter>().X = coords.X;
                _go.GetComponent<TilePresenter>().Y = coords.Y;
                _go.transform.DOJump(new Vector2(coords.X + FieldModel.fieldXOffset, coords.Y + FieldModel.fieldYOffset), 1.0f, 2, 0.5f);
            });

            // 타일 라이브 상태, 타일 깨짐
            tile.TileLive.Where(x => x == false)
            .Subscribe(x =>
            {
                // todo: 소리 재생
                // todo: 파티클 같이 보여준다.
                TileExplode(tile.Coords.Value);
                _go.transform.DOScale(0, 0.3f).SetEase(Ease.InOutBack);
            });


            //var zipStream = Observable.Zip(tile.X, tile.Y);
            //zipStream.Subscribe(coords =>
            //{
            //    Debug.Log($"coords: {coords[0]}, {coords[1]}");
            //    _go.transform.DOJump(new Vector2(coords[0] + FieldModel.fieldXOffset, coords[1] + FieldModel.fieldYOffset), 1.0f, 2, 0.5f);
            //}).
            //AddTo(this);


            //tile.Subscribe(x =>
            //{
            //    Debug.Log($"변경됨: {x.TileID}, {x.ImageID}, {x.X}, {x.Y} glow: {x.TileGlow}");

            //    _go.transform.Find("Glow").gameObject.SetActive(x.TileGlow);

            //    // 이곳에서 데이터가 변경되면 해당 오브젝트를 옮긴다.
            //    // 정보 변경
            //    _go.GetComponent<TilePresenter>().X = x.X;
            //    _go.GetComponent<TilePresenter>().Y = x.Y;
            //    _go.transform.DOJump(new Vector2(x.X+ FieldModel.fieldXOffset, x.Y+ FieldModel.fieldYOffset), 1.0f, 2, 0.5f);
            //})
            //.AddTo(this);
        }


        // 카메라 거리 조정
        SetCameraDistance(col, row);

        // 필드 이동시키기
        // 필드 좌표 이동 (크기 기반)
        //field.transform.position = new Vector2(FieldModel.fieldXOffset, FieldModel.fieldYOffset);

        // 셔플
        FieldModel.ShuffleTile();
        #endregion

        #region tile
        // 경로가 클리어 되었을 때 감시
        MatchModel.drawNode.Subscribe(x =>
        {
            //Debug.Log($"draw line");
            DrawLine(MatchModel.FinalNodeList, x.lineColor, x.lineLiveTime);
        });
        #endregion

        #region object pool
        /*explosionObjPool = new ObjectPooling<Explosion>(worldForeground, prefabExplosion);*/
        #endregion

    }

    // 경로 따라서 선 그리기
    public void DrawLine(List<Node> nodes, Color lineColor, float destroyTime)
    {
        if (nodes == null) return; //선을 연결할 노드들이 없으면 종료

        foreach (Node node in nodes)
        {

            GameObject _go = Instantiate(prefabLine); //일단 라인 생성하고

            _go.GetComponent<LineRenderer>().startColor = lineColor; //색 바꾸고
            _go.GetComponent<LineRenderer>().endColor = lineColor; //끝색도 바궈준다

            if (node.prev != null) //이전 노드가 존재한다면(처음 생성하는 라인이 아니라면)
            {
                //노드가 있는 필드가 중심을 맞추기 위해 offset으로 조정되어 있으므로 라인도 동일하게 offset값을 더해 주어야 한다!
                float offsetX = FieldModel.fieldXOffset; 
                float offsetY = FieldModel.fieldYOffset;
                // 현재 노드와 이전 노드를 이어 주며 진행한다.
                _go.GetComponent<LineRenderer>().SetPosition(0, new Vector3(node.x + offsetX, node.y + offsetY, 0));
                _go.GetComponent<LineRenderer>().SetPosition(1, new Vector3(node.prev.x + offsetX, node.prev.y + offsetY, 0));
            }
            else //이전 노드가 존재하지 않는다면(처음 생성하는 라인이라면)
            {
                //_go.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                //_go.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                _go.SetActive(false); //그냥 없앰 필요없음 ㅋㅋ
            }

            // worldForeground에 생성한다.
            _go.GetComponent<Transform>().SetParent(worldForeground); //타일 앞에 보이게


            Destroy(_go, destroyTime); // todo: 이후에 삭제 이펙트 및 모션에 대해서 고민해야 함
        }
    }

    public void TileExplode(Tile.TileCoords coords)
    {
        /*
        // 오브젝트풀 사용
        Explosion explosion = explosionObjPool.Rent();

        explosion.Explode(coords).Subscribe(_ =>
        {
            explosionObjPool.Return(explosion);
        });
        */
    }

    /// <summary>
    /// 카메라 거리를 조정한다.
    /// todo: 함수형으로 변경할 필요가 있음
    /// </summary>
    private void SetCameraDistance(int col, int row)
    {
        // 카메라 거리 이동
        // 카메라 크기 계산
        float cameraDistance;
        if (col > row * 1.6)
        {
            cameraDistance = col * 1.6f;
        }
        else
        {
            cameraDistance = row;
        }
        Camera.main.orthographicSize = cameraDistance;

        // 배경 크기 변경
        float scaleRatio = cameraDistance / 5.0f;
        worldBackground.localScale = new Vector3(scaleRatio, scaleRatio, 1);
    }

}
