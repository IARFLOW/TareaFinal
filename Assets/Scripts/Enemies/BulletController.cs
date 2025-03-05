using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletController : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f; // Tiempo de vida del proyectil en segundos

    private Vector3 direction;
    private bool isInitialized = false;

    void Start()
    {
        // Destruir el proyectil después de su tiempo de vida
        Destroy(gameObject, lifetime);

        // Si no se ha inicializado, usar dirección por defecto
        if (!isInitialized)
        {
            Initialize(Vector3.down);
        }
    }

    void Update()
    {
        // Mover el proyectil en la dirección establecida
        transform.position += direction * speed * Time.deltaTime;
    }

    // Método para inicializar la dirección del proyectil
    public void Initialize(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        isInitialized = true;

        // Opcional: Rotar el sprite para que mire en la dirección del movimiento
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90); // +90 para ajustar según el sprite
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si colisiona con el jugador
        if (other.CompareTag("Player"))
        {
            // Reiniciar nivel
            RestartLevel();
        }

        // No destruimos el proyectil si golpea otros objetos para simplificar
        // pero podrías agregar lógica para destruirlo al golpear paredes, etc.
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si colisiona con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reiniciar nivel
            RestartLevel();
        }

        // Destruir el proyectil al colisionar con cualquier cosa
        Destroy(gameObject);
    }

    void RestartLevel()
    {
        // Cargar la escena actual de nuevo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}