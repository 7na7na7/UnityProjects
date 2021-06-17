using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Project.Networking;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;

    public Vector2 Direction
    {
        set { direction = value; }
    }

    public float Speed
    {
        set { speed = value; }
    }

    public void Update()
    {
        Vector2 pos = direction * speed *NetworkClient.SERVER_UPDATE_TIME* Time.deltaTime;
        transform.position+=new Vector3(pos.x,pos.y,0);
    }
}
