using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text alivetime, zombiekill;
    public int zombiecount = 0;
    private int currentTime = 0;
    public Transform parent;
    private bool ismade;
    public GameObject panel;

    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        ismade = true;
        StartCoroutine(getTime());
    }

    // Update is called once per frame
    void Update()
    {
        Move1 player = GameObject.Find("player").GetComponent<Move1>();
        alivetime.text = "Alive Time : " + currentTime;
        zombiekill.text="Killed Zombie : " + zombiecount;
        if (player.isdead)
        {
            if (ismade)
            {
                text.text = "Alive Time : " + currentTime+"       "+"Killed Zombie : " + zombiecount;
                Instantiate(panel, parent);
                Instantiate(text,parent);
                Time.timeScale = 0;
                ismade = false;
            }
        }
    }

    IEnumerator getTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currentTime += 1;
        }
    }
}
