using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject touchEffect;
   public void TouchEffect(Vector2 pos)
    {
        Instantiate(touchEffect, pos, Quaternion.identity);
    }
}
