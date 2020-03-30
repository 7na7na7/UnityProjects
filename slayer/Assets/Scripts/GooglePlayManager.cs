using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class GooglePlayManager : MonoBehaviour
{
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
      }
      else
      {
         Destroy(gameObject);
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
      Social.ReportProgress(GPGSIds.achievement_____2, 100.0f, (bool bSuccess) => { }); //업적 달성!
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