using UnityEngine;

public class seeTarget : MonoBehaviour
{
    public float value = 0;
    
    void Update()
    {
        if(Player.instance!=null) 
            transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, Player.instance.transform.position.x, Player.instance.transform.position.y)+value); 
    }
    private float getAngle(float x1, float y1, float x2, float y2) //Vector값을 넘겨받고 회전값을 넘겨줌
    {
        float dx = x2 - x1;
        float dy = y2 - y1;

        float rad = Mathf.Atan2(dx, dy);
        float degree = rad * Mathf.Rad2Deg;
        
        return degree;
    }
}
