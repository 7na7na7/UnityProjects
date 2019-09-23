using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ability : MonoBehaviour
{
    public int type;
    public Sprite[] img; //여기 스프라이트 넣고
    private Button btn; //여기 버튼 넣은 다음에

    private void Start()
    {
        setability();
    }

    public void setability()
    {
        btn = GetComponent<Button>();
        btn.gameObject.GetComponent<Image>().sprite = img[0];
    }
}
