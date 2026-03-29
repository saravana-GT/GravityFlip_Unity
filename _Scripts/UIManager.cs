using UnityEngine;

public class UIManager : MonoBehaviour
{
    // These methods can be linked to your UI Buttons in the Inspector
    
    public void OnPlayButtonPressed()
    {
        GameManager.Instance.StartGame();
    }

    public void OnRestartButtonPressed()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnLeaderboardButtonPressed()
    {
        if (LeaderboardManager.Instance != null) {
            LeaderboardManager.Instance.ShowLeaderboardUI();
        }
    }
}
