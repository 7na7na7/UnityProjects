using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowCursor : MonoBehaviour
{
    public float speed; 
    private Vector3 MousePositon;
    public GameObject tail;
    public LayerMask layer;
    void Update()
    {
        if (gameObject.name == "Head")
        {
            MousePositon = Input.mousePosition;
            MousePositon =
                Camera.main.ScreenToWorldPoint(MousePositon) - transform.position; //플레이어포지션을 빼줘야한다!!!!!!!!!!!
            //월드포지션은 절대, 카메라와 플레이어 포지션은 변할 수 있다!!!!!!!

            float angle = Mathf.Atan2(MousePositon.y, MousePositon.x) * Mathf.Rad2Deg;
            float x = 0f;

            transform.rotation = Quaternion.Euler(0, 0f, angle - 90);
            MousePositon.Normalize();
            //print(MousePositon);
            
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y)
                , Vector2.up, 0.1f,layer); //위쪽으로 발사
            
            if (hit.collider == null)
                transform.position += MousePositon * speed * Time.deltaTime;
            else
            {
                //transform.position -= MousePositon * 2;
                //StartCoroutine(TimeCtrl());
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, 0); //안사라진다!!!! 오예!
        }
        else
        {
            transform.LookAt(tail.transform);
            transform.rotation=Quaternion.Euler(transform.rotation.x,transform.rotation.y,0);
            transform.position =
                Vector3.Lerp(transform.position, tail.transform.position, speed * Time.deltaTime); //Vector3.Lerp()를 쓰면 부드럽게 움직인다.
        }
    }

    IEnumerator TimeCtrl()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.03f);
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
