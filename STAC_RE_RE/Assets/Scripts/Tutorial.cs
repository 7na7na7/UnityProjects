using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private int currentIndex = -1;
    public float delay;
    private string tutoKey = "tutorial";
    public GameObject[] TutorialPanels;
    void Start()
    {
        int save = PlayerPrefs.GetInt(tutoKey, 0);
        if (save == 0)
        {
            PlayerPrefs.SetInt(tutoKey,1);
            StartCoroutine(tutorial());
        }
    }

    IEnumerator tutorial()
    {
        Spawner.instance.isTutorial = true;
        for (int j= 0; j < TutorialPanels.Length; j++)
        {
            currentIndex++;
            for (int i = 0; i < TutorialPanels.Length; i++)
            {
                if(i==currentIndex)
                    TutorialPanels[i].SetActive(true);
                else
                    TutorialPanels[i].SetActive(false);
            }
            if(j==0) 
                yield return new WaitForSeconds(delay+1f);
            else
                yield return new WaitForSeconds(delay);
        }
        for (int i = 0; i < TutorialPanels.Length; i++)
        {
            TutorialPanels[i].SetActive(false);
        }

        Spawner.instance.isTutorial = false;
    }
}
