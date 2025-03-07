using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISetup : MonoBehaviour
{
    [Header("Level UI")]
    public Canvas gameplayCanvas;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI coinsText;

    [Header("End Level UI")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI levelTimeText;
    public TextMeshProUGUI levelScoreText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Button restartButton;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    public Button resumeButton;
    public Button menuButton;

    // References
    private TimeManager timeManager;
    private LifeSystem lifeSystem;

    private void Start()
    {
        // Find managers
        timeManager = FindFirstObjectByType<TimeManager>();
        lifeSystem = FindFirstObjectByType<LifeSystem>();

        // Set initial UI
        SetupGameplayUI();

        // Setup buttons
        SetupButtons();

        // Ensure panels are hidden at start
        if (levelCompletePanel) levelCompletePanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
    }

    private void SetupGameplayUI()
    {
        // Connect timer to TimeManager if exists
        if (timeManager != null && timerText != null)
        {
            timeManager.timerText = timerText;
        }

        // Connect lives text to LifeSystem if exists
        if (lifeSystem != null && livesText != null)
        {
            lifeSystem.livesText = livesText;

            // Update to show initial lives
            livesText.text = "Vidas: " + lifeSystem.GetCurrentLives();
        }

        // Initialize coins text if needed
        if (coinsText != null)
        {
            coinsText.text = "Monedas: 0/0";
        }
    }

    private void SetupButtons()
    {
        // Restart button (Game Over)
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() =>
            {
                // Restart the current level
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            });
        }

        // Resume button (Pause)
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(() =>
            {
                // Resume game
                Time.timeScale = 1f;
                pausePanel.SetActive(false);
            });
        }

        // Menu button (Pause)
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(() =>
            {
                // Return to main menu
                Time.timeScale = 1f;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Assuming main menu is scene 0
            });
        }
    }

    private void Update()
    {
        // Check for pause button
        if (Input.GetKeyDown(KeyCode.Escape) && pausePanel != null)
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (pausePanel.activeSelf)
        {
            // Resume
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
        else
        {
            // Pause
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
    }

    // Called by the coin collection system
    public void UpdateCoinsText(int collected, int total)
    {
        if (coinsText != null)
        {
            coinsText.text = "Monedas: " + collected + "/" + total;
        }
    }

    // Used to show level complete data
    public void ShowLevelComplete(float time, int score)
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);

            if (levelTimeText != null)
            {
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                levelTimeText.text = "Tiempo: " + string.Format("{0:00}:{1:00}", minutes, seconds);
            }

            if (levelScoreText != null)
            {
                levelScoreText.text = "Puntuación: " + score;
            }
        }
    }
}
