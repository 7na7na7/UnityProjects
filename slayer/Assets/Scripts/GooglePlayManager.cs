using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
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
      if (instance == null)
      {
         instance = this;
         DontDestroyOnLoad(gameObject);
         PlayGamesPlatform.DebugLogEnabled = true;
         PlayGamesPlatform.Activate();
         LogIn();
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
   public void LogIn()
   {
      if (Social.localUser.authenticated)
      {
      }
      else
      {
         Social.localUser.Authenticate((bool success) =>
            { });  
      }
   }

   public void LogInOrLogOut()
   {
      if (Social.localUser.authenticated)
      {
         LogOut();
         GameObject.Find("GoogleText").GetComponent<Text>().text = "로그아웃됨";
      }
      else
      {
         Social.localUser.Authenticate((bool success) =>
         {
            if (success)
            {
               GameObject.Find("GoogleText").GetComponent<Text>().text = "로그인됨";
            }
            else
            {
               GameObject.Find("GoogleText").GetComponent<Text>().text = "로그인 실패";
            }
         });  
      }
   }
   public void LogOut()
   {
      ((PlayGamesPlatform)Social.Active).SignOut();
   }
   // 리더보드에 점수등록 후 보기
   public void OnShowLeaderBoard()
   {
      Social.ShowLeaderboardUI();
   }

   public void AddScore1(int score)
   {
      Social.ReportScore(score, GPGSIds.leaderboard______1, (bool bSuccess) =>
      {
      });
   }
   public void AddScore2(int score)
   {
      Social.ReportScore(score, GPGSIds.leaderboard______2, (bool bSuccess) =>
      {
      });
   }
   public void AddScore3(int score)
   {
      Social.ReportScore(score, GPGSIds.leaderboard______3, (bool bSuccess) =>
      {
      });
   }

   public void AddCombo1(int combo)
   {
      Social.ReportScore(combo, GPGSIds.leaderboard______1_2, (bool bSuccess) =>
      {
      });
   }
   public void AddCombo2(int combo)
   {
      Social.ReportScore(combo, GPGSIds.leaderboard______2_2, (bool bSuccess) =>
      {
      });
   }
   // 업적보기
   public void OnShowAchievement()
   {
      Social.ShowAchievementsUI();
   }
 
   // 업적추가
   public void Achievement1()
   {
      Social.ReportProgress(GPGSIds.achievement__6, 100.0f, (bool bSuccess) => { }); //업적 달성!
   }
   public void Achievement2()
   {
      Social.ReportProgress(GPGSIds.achievement__12, 100.0f, (bool bSuccess) => { }); //업적 달성!
   }
   public void Achievement3()
   {
      Social.ReportProgress(GPGSIds.achievement__24, 100.0f, (bool bSuccess) => { }); //업적 달성!
   }
   public void Achievement4()
   {
      Social.ReportProgress(GPGSIds.achievement____1, 100.0f, (bool bSuccess) => { }); //업적 달성!
   }
   public void Achievement5()
   {
      Social.ReportProgress(GPGSIds.achievement____1_2, 100.0f, (bool bSuccess) => { }); //업적 달성!
   }
   public void Achievement6()
   {
      Social.ReportProgress(GPGSIds.achievement____1_3, 100.0f, (bool bSuccess) => { }); //업적 달성!
   }
   public void Achievement7()
   {
      Social.ReportProgress(GPGSIds.achievement____2, 100.0f, (bool bSuccess) => { }); //업적 달성!
   }
   public void Achievement8()
   {
      Social.ReportProgress(GPGSIds.achievement____2_2, 100.0f, (bool bSuccess) => { }); //업적 달성!
   }
   public void Achievement9()
   {
      Social.ReportProgress(GPGSIds.achievement____2_3, 100.0f, (bool bSuccess) => { }); //업적 달성!
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