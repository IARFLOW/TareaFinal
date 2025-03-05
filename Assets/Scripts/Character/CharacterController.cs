using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    private Animator heroAnimator;
    private Rigidbody2D rb;
    public float speed = 3f;
    public float jumpForce = 5f;
    private Vector2 direction;

    private int jumpCount = 0;
    // private enum direcciones { IZQDA, DCHA, ARRIBA, ABAJO }
    // private direcciones direccion;
    // Para evitar cargar múltiples veces la escena
    private bool levelCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        heroAnimator = this.GetComponent<Animator>();
        heroAnimator.Play("walk");
        //direccion = direcciones.DCHA;
        rb = GetComponent<Rigidbody2D>();

        // Asegurar que el personaje tiene el tag correcto
        if (tag != "Player")
        {
            tag = "Player";
            Debug.Log("Tag 'Player' asignado al personaje");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // No procesar input si el nivel está completado
        if (levelCompleted) return;

        // Reiniciamos el vector de dirección
        direction = new Vector2();

        // Movimiento y animación según la tecla de dirección pulsada
        if (Input.GetKey(KeyCode.A))
        {
            // direccion = direcciones.IZQDA;
            heroAnimator.Play("walk");
            direction = Vector2.left;
            // Volteamos el personaje hacia la izquierda
            transform.localScale = new Vector2(-1, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //direccion = direcciones.DCHA;
            heroAnimator.Play("walk");
            direction = Vector2.right;
            // Volteamos el personaje hacia la derecha
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            heroAnimator.Play("Idle");
        }

        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W) && jumpCount < 2)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            heroAnimator.Play("Jump");
        }

        this.transform.rotation = Quaternion.identity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }

    // CRUCIAL: Detectar cuando entramos en un trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // Evitar múltiples cargas
        if (levelCompleted) return;

        // Detectar la barrera (door) o cualquier objeto con tag Finish
        if (other.CompareTag("Finish") || other.gameObject.name.ToLower().Contains("door"))
        {
            Debug.Log("¡Jugador tocó la meta/barrera!");
            CompleteLevel();
        }
    }

    // Añadido para mayor seguridad
    void OnTriggerStay2D(Collider2D other)
    {
        // Evitar múltiples cargas
        if (levelCompleted) return;

        // Redundancia para asegurar la detección
        if (other.CompareTag("Finish") || other.gameObject.name.ToLower().Contains("door"))
        {
            Debug.Log("¡Jugador permanece en la meta!");
            CompleteLevel();
        }
    }

    // Método unificado para completar nivel
    void CompleteLevel()
    {
        // Evitar múltiples cargas
        if (levelCompleted) return;

        levelCompleted = true;
        Debug.Log("¡Nivel completado! Cargando siguiente...");

        // Mostrar alguna animación o efecto opcional aquí

        // Cargar siguiente nivel
        StartCoroutine(LoadNextLevelWithDelay(0.5f));
    }

    // Cargar el siguiente nivel con un pequeño retraso
    IEnumerator LoadNextLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Si es el último nivel, volver al primero o a un menú
            SceneManager.LoadScene(0);
        }
    }
}