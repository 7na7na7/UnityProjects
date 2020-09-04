using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color color;
    Sprite[] tileThemes;
    public GameObject tile;

    private void Start()
    {
        tileThemes=new Sprite[BulletData.instance.tileThemes.Length];
        for(int i=0;i<tileThemes.Length;i++)
        {
            tileThemes[i] = BulletData.instance.tileThemes[i];
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = tileThemes[BulletData.instance.currentColorIndex];
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = color;
        }
        GetComponent<SpriteRenderer>().sprite = tileThemes[BulletData.instance.currentColorIndex];
        GetComponent<SpriteRenderer>().color = color;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Edge1") || col.CompareTag("Edge2"))
        {
            float a, b;
            Vector3 posA=Vector3.zero;
            Vector3 posB=Vector3.zero;
            float spriteSize=GetSize.instance.GetSpriteSize(gameObject).x;
          
            if (Player.instance.transform.position.x < transform.position.x) //왼쪽이면
            {
                a=Mathf.Abs(Player.instance.transform.position.x - transform.position.x);
                posA=new Vector3(posA.x-spriteSize,posA.y,posA.z);
            }
            else //오른쪽이면
            {
                a=Mathf.Abs(Player.instance.transform.position.x - transform.position.x);
                posA=new Vector3(posA.x+spriteSize,posA.y,posA.z);
            } 
            if (Player.instance.transform.position.y < transform.position.y) //아래쪽이면
            {
                b =Mathf.Abs( Player.instance.transform.position.y - transform.position.y);
                posB=new Vector3(posB.x,posB.y-spriteSize,posB.z);
            }
            else //위쪽이면
            {
                b =Mathf.Abs( Player.instance.transform.position.y - transform.position.y);
                posB=new Vector3(posB.x,posB.y+spriteSize,posB.z);
            }
            if(a>b) 
                Instantiate(tile, transform.position+posA, Quaternion.identity);
            else
                Instantiate(tile, transform.position+posB, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
