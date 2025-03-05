using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public GameObject barrier; // La barrera que bloquea la meta
    public TextMeshProUGUI coinText; // Texto para mostrar contador de monedas
    public TextMeshProUGUI timerText; // Texto para mostrar el contador de tiempo
    public float timeLimit = 60f; // Tiempo límite en segundos (configurable)

    [SerializeField] private int totalCoins = 0;
    [SerializeField] private int collectedCoins = 0;
    private float timeRemaining;
    private bool barrierDestroyed = false;

    // Métodos públicos para acceder a los contadores
    public int GetCollectedCoins() { return collectedCoins; }
    public int GetTotalCoins() { return totalCoins; }

    void Awake()
    {
        // Inicialización temprana para asegurar que esté disponible para las monedas
        Debug.Log($"DoorController Awake en objeto: {gameObject.name}");

        // Si barrier no está asignado, intenta usar este mismo objeto
        if (barrier == null)
        {
            barrier = gameObject;
            Debug.Log("Barrera asignada automáticamente al objeto actual");
        }
    }

    void Start()
    {
        Debug.Log($"DoorController Start en objeto: {gameObject.name}");

        // Inicializar el temporizador
        timeRemaining = timeLimit;

        // Contar todas las monedas al inicio
        Coin[] allCoins = FindObjectsOfType<Coin>();
        totalCoins = allCoins.Length;
        Debug.Log($"Monedas detectadas en la escena: {totalCoins} por DoorController en {gameObject.name}");

        // Forzar actualización inmediata de la UI
        ForceUpdateUI();
    }

    void Update()
    {
        // Actualizar el temporizador
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            // Si el tiempo se acabó, reiniciar el nivel
            RestartLevel();
        }

        // Verificar si todas las monedas han sido recogidas
        if (collectedCoins >= totalCoins && totalCoins > 0 && !barrierDestroyed && barrier != null)
        {
            Debug.Log($"Verificación automática: collectedCoins={collectedCoins}, totalCoins={totalCoins}, barrierDestroyed={barrierDestroyed}, barrier={barrier.name}");
            DestroyBarrier();
        }

        // Debug: presiona F2 para forzar la actualización de la UI
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Forzando actualización de UI");
            ForceUpdateUI();
        }

        // Debug: presiona F3 para verificar estado
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log($"Estado actual: collectedCoins={collectedCoins}, totalCoins={totalCoins}, barrierDestroyed={barrierDestroyed}, barrier={(barrier != null ? barrier.name : "null")}");
        }

        // Debug: presiona F4 para forzar la destrucción de la barrera
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log("Forzando destrucción de la barrera mediante tecla F4");
            ForceDestroyBarrier();
        }

        // Debug: presiona F5 para incrementar manualmente el contador de monedas
        if (Input.GetKeyDown(KeyCode.F5))
        {
            collectedCoins++;
            Debug.Log($"Incremento manual: collectedCoins={collectedCoins}/{totalCoins}");
            ForceUpdateUI();
        }
    }

    // Método llamado por la moneda cuando es recogida
    public void CoinCollected()
    {
        Debug.Log($"CoinCollected llamado en DoorController ({gameObject.name}): collectedCoins antes={collectedCoins}");
        collectedCoins++;
        Debug.Log($"Moneda recogida: {collectedCoins}/{totalCoins} en {gameObject.name}");

        // Forzar actualización inmediata de la UI
        ForceUpdateUI();

        // Si se han recogido todas las monedas, destruir la barrera
        if (collectedCoins >= totalCoins && totalCoins > 0 && !barrierDestroyed && barrier != null)
        {
            Debug.Log($"¡Todas las monedas recogidas! {collectedCoins}/{totalCoins} - Destruyendo barrera.");
            DestroyBarrier();
        }
    }

    // Método para destruir la barrera
    private void DestroyBarrier()
    {
        if (barrier == null || barrierDestroyed)
        {
            Debug.Log($"No se puede destruir la barrera: barrier es null={barrier == null}, barrierDestroyed={barrierDestroyed}");
            return;
        }

        Debug.Log("Destruyendo barrera: " + barrier.name);

        // Intentar primero desactivar la barrera en caso de problemas
        barrier.SetActive(false);

        // También intentamos destruirla
        Destroy(barrier);
        barrierDestroyed = true;

        Debug.Log("Barrera destruida con éxito");
    }

    // Método público para forzar la destrucción de la barrera (para debug)
    public void ForceDestroyBarrier()
    {
        if (barrier != null)
        {
            Debug.Log("Forzando destrucción de barrera: " + barrier.name);
            barrier.SetActive(false);
            Destroy(barrier);
            barrierDestroyed = true;
            Debug.Log("Barrera destruida forzadamente");
        }
        else
        {
            Debug.LogError("No se pudo encontrar la barrera para destruir (es null)");
        }
    }

    // Actualizar la UI con el contador de monedas y tiempo
    private void UpdateUI()
    {
        // Solo actualiza el tiempo cada frame para evitar sobrecarga
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = $"Tiempo: {seconds}";
        }
    }

    // Forzar la actualización completa de la UI
    private void ForceUpdateUI()
    {
        // Actualizar texto de monedas - búsqueda directa si es null
        if (coinText == null)
        {
            coinText = GameObject.Find("ContadorMonedas")?.GetComponent<TextMeshProUGUI>();
            Debug.Log("Buscando ContadorMonedas: " + (coinText != null ? "Encontrado" : "No encontrado"));
        }

        if (coinText != null)
        {
            coinText.text = $"Monedas: {collectedCoins}/{totalCoins}";
            Debug.Log("UI actualizada: " + coinText.text);
        }
        else
        {
            Debug.LogError("No se puede actualizar el contador de monedas: coinText es null");
        }

        // Actualizar texto del temporizador - búsqueda directa si es null
        if (timerText == null)
        {
            timerText = GameObject.Find("Temporizador")?.GetComponent<TextMeshProUGUI>();
            Debug.Log("Buscando Temporizador: " + (timerText != null ? "Encontrado" : "No encontrado"));
        }

        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = $"Tiempo: {seconds}";
            Debug.Log("UI actualizada: " + timerText.text);
        }
        else
        {
            Debug.LogError("No se puede actualizar el temporizador: timerText es null");
        }
    }

    // Reiniciar el nivel
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}