using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorController : MonoBehaviour
{
    public GameObject barrier;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI timerText;
    public float timeLimit = 60f;
    [SerializeField] private int totalCoins = 0;
    [SerializeField] private int collectedCoins = 0;
    private float timeRemaining;
    private bool barrierDestroyed = false;

    public int GetCollectedCoins() { return collectedCoins; }
    public int GetTotalCoins() { return totalCoins; }

    void Awake()
    {
        if (barrier == null)
        {
            barrier = gameObject;
        }
    }

    void Start()
    {
        timeRemaining = timeLimit;
        Coin[] allCoins = FindObjectsByType<Coin>(FindObjectsSortMode.None);
        totalCoins = allCoins.Length;
        ForceUpdateUI();
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            RestartLevel();
        }
        if (collectedCoins >= totalCoins && totalCoins > 0 && !barrierDestroyed && barrier != null)
        {
            DestroyBarrier();
        }
    }

    public void CoinCollected()
    {
        collectedCoins++;
        ForceUpdateUI();
        if (collectedCoins >= totalCoins && totalCoins > 0 && !barrierDestroyed && barrier != null)
        {
            DestroyBarrier();
        }
    }

    private void DestroyBarrier()
    {
        if (barrier == null || barrierDestroyed)
        {
            return;
        }
        barrier.SetActive(false);
        Destroy(barrier);
        barrierDestroyed = true;
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = $"Tiempo: {seconds}";
        }
    }

    private void ForceUpdateUI()
    {
        if (coinText == null)
        {
            coinText = GameObject.Find("ContadorMonedas")?.GetComponent<TextMeshProUGUI>();
        }
        if (coinText != null)
        {
            coinText.text = $"Monedas: {collectedCoins}/{totalCoins}";
        }
        if (timerText == null)
        {
            timerText = GameObject.Find("Temporizador")?.GetComponent<TextMeshProUGUI>();
        }
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = $"Tiempo: {seconds}";
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}