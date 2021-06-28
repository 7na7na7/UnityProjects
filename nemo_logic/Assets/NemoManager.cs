using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class grid
{
    public bool[] X;
}
public class NemoManager : MonoBehaviour
{
    public int correctCount = 0;
    public int unCorrectCount = 0;
    public grid[] Y;
    public GameObject ElementPrefab;
    public GameObject BlackNumberPrefab;
    private Element[,] ElementArray;
    private float xSize, ySize;

    void Start()
    {
        xSize = GetSpriteSize(ElementPrefab).x;
        ySize = GetSpriteSize(ElementPrefab).y;
        transform.position+=new Vector3(xSize*Y[0].X.Length/-2,ySize*Y.Length/2,0);
        ElementArray=new Element[Y[0].X.Length+1,Y.Length+1];
        Generate();
        SetBlackCount();
    }

    private void Update()
    {
        if (correctCount == 0 && unCorrectCount == 0)
        {
            GameObject.Find("ClearText").GetComponent<Text>().color=Color.yellow;
        }
    }

    void Generate()
    {
        for (int y = 0; y < Y.Length; y++)
        {
            for (int x = 0; x < Y[y].X.Length; x++)
            {
                ElementArray[x, y]=CloneElement(x,-y,Y[y].X[x]);
            }
        }
    }

    void SetBlackCount()
    {
        //세로
        for (int y = 0; y < ElementArray.GetLength(1)-1; y++)
        {
            List<int> blackCounts=new List<int>();
            blackCounts.Add(0);
            int index = 0;
            bool canGo = true;
            for (int x = 0; x < ElementArray.GetLength(0)-1; x++)
            {
                if (ElementArray[x, y].isBlack)
                {
                    canGo = true;
                    if (blackCounts.Count > index)
                    {
                        blackCounts[index]++;
                    }
                    else
                    {
                        blackCounts.Add(1);
                    }
                }
                else
                {
                    if (canGo)
                    {
                        index++;
                        canGo = false;
                    }
                }
            }

            blackCounts.RemoveAll(i => i==0);
            string str = "";
            foreach (int i in blackCounts)
            {
                str += " "+i;
            }
            
            Text number=Instantiate(BlackNumberPrefab, ElementArray[0, y].transform.position+new Vector3(-.75f,0,0),quaternion.identity).GetComponent<Text>();
            number.text = str;
        }
        //가로
        for (int x = 0; x < ElementArray.GetLength(0)-1; x++)
        {
            List<int> blackCounts=new List<int>();
            blackCounts.Add(0);
            int index = 0;
            bool canGo = true;
            for (int y = 0; y < ElementArray.GetLength(1)-1; y++)
            {
                if (ElementArray[x, y].isBlack)
                {
                    canGo = true;
                    if (blackCounts.Count > index)
                    {
                        blackCounts[index]++;
                    }
                    else
                    {
                        blackCounts.Add(1);
                    }
                }
                else
                {
                    if (canGo)
                    {
                        index++;
                        canGo = false;
                    }
                }
            }
            blackCounts.RemoveAll(i => i==0);
            string str = "";
            foreach (int i in blackCounts)
            {
                str += i+"\n";
            }
            Text number=Instantiate(BlackNumberPrefab, ElementArray[x, 0].transform.position+new Vector3(0,.75f,0),quaternion.identity).GetComponent<Text>();
            number.text = str;
        }
    }
    Element CloneElement(int p_x,int p_y,bool isBlack)
    {
        GameObject copyObj = Instantiate(ElementPrefab);
        copyObj.transform.SetParent(this.transform);
        
        Vector2 tempPos = Vector2.zero;
        
        copyObj.transform.SetParent(this.transform);
        tempPos.Set(transform.position.x+p_x*xSize,transform.position.y+ p_y*ySize);
        copyObj.transform.position = tempPos;
        copyObj.name = "CloneBlock_" + p_x.ToString() + "_" + p_y.ToString();
        if (isBlack)
        {
            correctCount++;
            copyObj.GetComponent<Element>().SetData(isBlack);
        }
        return copyObj.GetComponent<Element>();
    }
    public Vector3 GetSpriteSize(GameObject _target)
    {
        Vector3 worldSize = Vector3.zero;
        if(_target.GetComponent<SpriteRenderer>())
        {
            Vector2 spriteSize = _target.GetComponent<SpriteRenderer>().sprite.rect.size;
            Vector2 localSpriteSize = spriteSize / _target.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            worldSize = localSpriteSize;
            worldSize.x *= _target.transform.lossyScale.x;
            worldSize.y *= _target.transform.lossyScale.y;
        }
        else
        {
            Debug.Log ("SpriteRenderer Null");
        }
        return worldSize;
    }
}
