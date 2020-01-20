using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longBullet : MonoBehaviour
{
    public bool canLong = true;
    public float bulletSpeed;
    void Start()
    {
        float r = Random.Range(-2.5f,2.5f);
        transform.eulerAngles=new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z+r);
    }
    
    void FixedUpdate()
    {
        if(canLong) 
            transform.localScale+=new Vector3(Time.fixedDeltaTime*bulletSpeed,0,0);

        //RaycastHit2D hit = Physics2D.Raycast(GetSpriteSize(gameObject), Vector2.right, 0.01f);
        
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
