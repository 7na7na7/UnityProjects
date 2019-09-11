using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bananagun : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip bulletsound;//총알소리
    
    private Color color;
    private Move1 player;
    public Transform tr;
    public GameObject banana;
    private Vector3 MousePositon;
    public Camera camera;

    private void Start()
    {
        player = FindObjectOfType<Move1>();
    }

    void Update()
    {
        if (player.isdead == true)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>(); //스프라이트로 함
            color.r = 255;
            color.g = 255;
            color.b = 255;
            color.a = 0.0f;
            sprite.color = color;
        }
        else
        {
            MousePositon = Input.mousePosition;
            MousePositon =
                camera.ScreenToWorldPoint(MousePositon) - player.transform.position; //플레이어포지션을 빼줘야한다!!!!!!!!!!!
            //월드포지션은 절대, 카메라와 플레이어 포지션은 변할 수 있다!!!!!!!


            float angle = Mathf.Atan2(MousePositon.y, MousePositon.x) * Mathf.Rad2Deg;
            float x = 0f;
            if (Mathf.Abs(angle) > 90)
            {
                x = 180f;
                angle *= -1f;
            }

            transform.rotation = Quaternion.Euler(x, 0f, angle);
            if (Input.GetMouseButtonDown(0))//손아픔
            {
                audiosource.PlayOneShot(bulletsound);
                Instantiate(banana, tr);
            }
        }
    }
}
