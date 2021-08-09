using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class TilePresenter : MonoBehaviour
{

    public int index;

    public Animator tileAnimator;

    public SpriteRenderer tileBG;
    public SpriteRenderer tileImage;

    public SpriteRenderer glow;

    //private int x, y, tileId, tileImageID;
    public int X;
    public int Y;
    public int TileID;
    public int ImageID;
    public GameObject glowObj;

    //public int tileID, tileImageID;

    //private Transform thisTransform;

    private void Awake()
    {
        //thisTransform = gameObject.transform;
   
        glow.DOFade(0, 0.5f).SetEase(Ease.InCirc).SetLoops(-1, LoopType.Yoyo);

    }

    public void Init(Sprite bg, Sprite img, int ID, int imageID, int x, int y)
    {
        tileBG.sprite = bg;
        tileImage.sprite = img;
        TileID = ID;
        ImageID = imageID;
        X = x;
        Y = y;
    }

    public void SetGlow(bool setValue)
    {
        glowObj.SetActive(setValue);
        //gameObject.transform.Find("Glow").gameObject.SetActive(setValue);
    }

    private void OnMouseDown()
    {
        //Debug.Log($"타일 선택: {X}, {Y}, id: {TileID}, {ImageID}");
        MatchModel.PickTile(X, Y, ImageID, TileID);
        //TileController.TilePick(this);
        //Debug.Log(string.Format("{0}, {1}", X, Y));
    }

}