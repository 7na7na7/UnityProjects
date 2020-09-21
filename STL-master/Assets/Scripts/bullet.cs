using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private player player;
    public float speed;
    private Transform target;
    private void Start()
    {
        Destroy(this.gameObject, 3f);
        player = FindObjectOfType<player>();
        // 타겟 방향으로 회전함
        target = player.transform;
        Vector3 dir = target.position - transform.position; //사이의 거리를 구함
        dir.z = 0;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    void Update()
    {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
    }
}
