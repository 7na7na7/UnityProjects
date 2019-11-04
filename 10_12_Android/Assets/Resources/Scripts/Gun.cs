
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Camera camera;
    private Vector3 MousePositon; //회전을 위한 벡터값
    void Awake()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (transform.parent.GetComponent<Player>().pv.IsMine)
        {
            MousePositon = Input.mousePosition;
            MousePositon =
                camera.ScreenToWorldPoint(MousePositon) - transform.position; //플레이어포지션을 빼줘야한다!!!!!!!!!!!
            //월드포지션은 절대, 카메라와 플레이어 포지션은 변할 수 있다!!!!!!!

            MousePositon.y -= 0.25f; //오차조정을 위한 코드

            float angle = Mathf.Atan2(MousePositon.y, MousePositon.x) * Mathf.Rad2Deg;
            float x = 0f;
            if (Mathf.Abs(angle) > 90)
            {
                x = 180f;
                angle *= -1f;
            }
            transform.rotation = Quaternion.Euler(x, 0f, angle);
        }
    }
}
