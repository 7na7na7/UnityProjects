using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public static float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (GoalArea.goal == false) //게임 실행중이에만
        {
            time += Time.deltaTime;
        }
        int t = Mathf.FloorToInt(time);
        Text uiText = GetComponent<Text>();
        uiText.text = "Time : " + t.ToString();
    }
}
