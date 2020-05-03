using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GooglePlayManager : MonoBehaviour
{
   public int canStage1 = 0;
   public int canStage2 = 0;
   private string canStage1Key = "canStage1";
   private string canStage2Key = "canStage2";
   private int isFirst = 0;
   private string isFirstKey = "isFirst";
   public static GooglePlayManager instance;
   private void Start()
   {
      Screen.SetResolution(1920,1080,true);
      if (instance == null)
      {
         instance = this;
         DontDestroyOnLoad(gameObject);
         canStage1 = PlayerPrefs.GetInt(canStage1Key,0);
         canStage2 = PlayerPrefs.GetInt(canStage2Key,0);
         isFirst = PlayerPrefs.GetInt(isFirstKey, 0);
         if (isFirst == 0)
         {
            isFirst = 1;
            PlayerPrefs.SetInt(isFirstKey, 1);
            FindObjectOfType<LoadScene>().Tutorial();
         }
      }
      else
      {
         Destroy(gameObject);
      }
   }
   
   public void CanStage1()
   {
      if (canStage1 != 1)
      {
         canStage1 = 1;
         PlayerPrefs.SetInt(canStage1Key, 1);
      }
   }
   public void CanStage2()
   {
      if (canStage2 != 1)
      {
         canStage2 = 1;
         PlayerPrefs.SetInt(canStage2Key, 1);
      }
   }


   public void ToTitle()
   {
      SceneManager.LoadScene("Title");
   }

   public void ToSetting()
   {
      SceneManager.LoadScene("Setting");
   }
}