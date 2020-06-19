using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InputField input;
    public GameObject setNamePanel;
    public Text text;
    public string name;

    private void Awake()
    {
        setNamePanel.SetActive(true);
    }
    public void Enter()
    {
        if (setNamePanel != null)
        {
            
                if (input.text != null)
                {
                    name = input.text;
                    setNamePanel.SetActive(false);
                    FindObjectOfType<PlayerScript>().isRecording = false;
                }
            
        }

        text.text = name;
    }
}

    
