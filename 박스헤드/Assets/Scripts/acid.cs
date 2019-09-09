using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acid : MonoBehaviour
{
    private GameObject obj;
    private Transform parent;
    public float speed;
    private Transform target;
    private void Start()
    {
        obj = this.gameObject;
        Destroy(obj, 5f);
        parent = GameObject.Find("BG").GetComponent<Transform>();
        this.transform.SetParent(parent.transform);//child의 부모를 parent로 설정
        
        // 타겟 방향으로 회전함
        Move1 player = GameObject.Find("player").GetComponent<Move1>();
        target = player.transform;
        Vector3 dir = target.position - transform.position; //사이의 거리를 구함
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    void Update()
    {
        Move1 player = GameObject.Find("player").GetComponent<Move1>();
        target = player.transform;
        Vector3 dir = target.position - transform.position; //사이의 거리를 구함
        transform.Translate(Vector3.right*speed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(obj);
        }
    }
}
