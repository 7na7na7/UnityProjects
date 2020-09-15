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
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public void Set()
   {
      PlayGamesPlatform.DebugLogEnabled = true;
      PlayGamesPlatform.Activate();
      LogIn();
   }
   public void LogIn()
   {
      Social.localUser.Authenticate((bool success) =>
      {
      });
   }

   public void LogInOrLogOut()
   {
      if (Social.localUser.authenticated) // GPGS 로그인 되어 있는 경우
      {
         ((PlayGamesPlatform) Social.Active).SignOut(); //로그아웃
         Social.localUser.Authenticate((bool success) =>
         {
         });
      }
      else // GPGS 로그인이 되어 있지 않은 경우
      {
         Social.localUser.Authenticate((bool success) =>
         {
         });
      }
   }

   public void LogOut()
   {
      ((PlayGamesPlatform) Social.Active).SignOut();
   }

   // 리더보드에 점수등록 후 보기
   public void OnShowLeaderBoard()
   {
      Social.ShowLeaderboardUI();
   }

   // 업적보기
   public void OnShowAchievement()
   {
      Social.ShowAchievementsUI();
   }

   public void SetScore(int score)
   {
      Social.ReportScore(score, GPGSIds.leaderboard_leaderboard, (bool bSuccess) => { });
   }

   public void Achievement(int index)
   {
      switch (index)
      {
         case 1:
            Social.ReportProgress(GPGSIds.achievement_jump_over_your_limit_l,100f,null);
            break;
         case 2:
            Social.ReportProgress(GPGSIds.achievement_jump_over_your_limit_ll,100f,null);
            break;
         case 3:
            Social.ReportProgress(GPGSIds.achievement_jump_over_your_limit_lll,100f,null);
            break;
         case 4:
            Social.ReportProgress(GPGSIds.achievement_combo_master_l,100f,null);
            break;
         case 5:
            Social.ReportProgress(GPGSIds.achievement_combo_master_ll,100f,null);
            break;
         case 6:
            Social.ReportProgress(GPGSIds.achievement_combo_master_lll,100f,null);
            break;
         case 7:
            Social.ReportProgress(GPGSIds.achievement_rich,100f,null);
            break;
         case 8:
            Social.ReportProgress(GPGSIds.achievement_new_color,100f,null);
            break;
         case 9:
            Social.ReportProgress(GPGSIds.achievement_rainbow_color,100f,null);
            break;
         case 10:
            Social.ReportProgress(GPGSIds.achievement_long_lifetime,100f,null);
            break;
         case 11:
            Social.ReportProgress(GPGSIds.achievement_complete_a_song,100f,null);
            break;
         case 12:
            Social.ReportProgress(GPGSIds.achievement_are_you_a_dj,100f,null);
            break;
      }
   }
}