using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonFunctions : MonoBehaviour
{

    public void GameStartButtonFunction(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void ExitbuttonFunction()
    {
        Application.Quit();
    }

}
