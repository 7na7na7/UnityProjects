using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataInput : MonoBehaviour
{
    public GameObject InputPanel;
    public InputField input;


    public void Input()
    {
        if (input.text == "1" || input.text == "2" || input.text == "3" || input.text == "4" || input.text == "5" ||
            input.text == "6" || input.text == "7" || input.text == "8" || input.text == "9")
        {
            FindObjectOfType<GameManager>().Set(int.Parse(input.text));
            
            InputPanel.SetActive(false);
        }
    }
}
