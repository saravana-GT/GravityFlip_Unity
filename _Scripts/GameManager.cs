using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Start, Playing, GameOver }
    public GameState currentGameState;

    [Header("Game Settings")]
    public float score;
    public float currentSpeed = 10f;
    public float maxSpeed = 25f;
    public float speedIncreaseRate = 0.1f;

    [Header("UI References")]
    public GameObject startUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        currentGameState = GameState.Start;
        startUI.SetActive(true);
        gameOverUI.SetActive(false);
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (currentGameState == GameState.Playing)
        {
            score += Time.deltaTime * (currentSpeed / 2);
            scoreText.text = "SCORE: " + Mathf.FloorToInt(score).ToString();

            if (currentSpeed < maxSpeed)
            {
                currentSpeed += speedIncreaseRate * Time.deltaTime;
            }
        }
    }

    public void StartGame()
    {
        currentGameState = GameState.Playing;
        startUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        currentGameState = GameState.GameOver;
        gameOverUI.SetActive(true);
        int finalScore = Mathf.FloorToInt(score);
        finalScoreText.text = "FINAL SCORE: " + finalScore.ToString();

        // Leaderboard submission
        if (LeaderboardManager.Instance != null) {
            LeaderboardManager.Instance.SubmitScore(finalScore);
        }

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
