using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar por tag en lugar de por nombre
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Meta alcanzada! Cargando siguiente nivel...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}