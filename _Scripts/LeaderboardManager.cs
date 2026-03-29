using UnityEngine;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    [Header("Leaderboard IDs")]
    public string androidLeaderboardID = "YOUR_ANDROID_ID_HERE";
    public string iosLeaderboardID = "YOUR_IOS_ID_HERE";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AuthenticateUser();
    }

    void AuthenticateUser()
    {
        #if UNITY_ANDROID
        PlayGamesPlatform.Activate();
        #endif

        Social.localUser.Authenticate((bool success) => {
            if (success) {
                Debug.Log("Leaderboard: Authentication Successful");
            } else {
                Debug.Log("Leaderboard: Authentication Failed");
            }
        });
    }

    public void SubmitScore(int score)
    {
        string lbID = "";
        #if UNITY_ANDROID
        lbID = androidLeaderboardID;
        #elif UNITY_IOS
        lbID = iosLeaderboardID;
        #endif

        if (Social.localUser.authenticated) {
            Social.ReportScore(score, lbID, (bool success) => {
                if (success) Debug.Log("Leaderboard: Score Submitted Successfully");
                else Debug.Log("Leaderboard: Score Submission Failed");
            });
        }
    }

    public void ShowLeaderboardUI()
    {
        if (Social.localUser.authenticated) {
            Social.ShowLeaderboardUI();
        } else {
            AuthenticateUser();
        }
    }
}
