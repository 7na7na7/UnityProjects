using UnityEngine;

public class crewMove : MonoBehaviour
{
    private Vector2 dir;
    private void Start()
    {
        dir = Player.instance.transform.position - transform.position;
        dir.Normalize();
        Destroy(gameObject,5f);
    }

    void Update()
    {
       transform.Translate(dir*Time.deltaTime*10);
    }
}
