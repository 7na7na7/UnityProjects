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

   public void AddScore()
   {
      // 1000점을 등록
      Social.ReportScore(1000, GPGSIds.leaderboard, (bool bSuccess) => { });  
   }
 
   // 업적보기
   public void OnShowAchievement()
   {
      Social.ShowAchievementsUI();
   }
 
   // 업적추가
   public void Achievement1()
   {
      Social.ReportProgress(GPGSIds.achievement, 100.0f, (bool bSuccess) => { }); //업적 달성!
   }

   public void ToTitle()
   {
      FadePanel.instance.Fade();
      SceneManager.LoadScene("Title");
   }
}