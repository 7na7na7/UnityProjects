using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombie3 : MonoBehaviour
{
    private GameObject parent;

    
    private void Start()
    {
        parent = transform.parent.gameObject;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("banana"))
        {
            GameOver gameover = GameObject.Find("Canvas").GetComponent<GameOver>();
            gameover.zombiecount += 1;
            Level level = FindObjectOfType<Level>();
            level.zombiecount[level.i]--; //생성되면 zombiecount--
            Destroy(parent);
        }
    }
}
