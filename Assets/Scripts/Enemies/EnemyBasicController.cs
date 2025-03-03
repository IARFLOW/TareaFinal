using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBasicController : MonoBehaviour
{
    public float speed = 2f;
    public float patrolDistance = 2f;
    public float bounceForce = 1f;

    private float leftBound;
    private float rightBound;
    private bool movingRight = true;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftBound = transform.position.x;
        rightBound = transform.position.x + patrolDistance;
    }

    void Update()
    {
        PatrolMovement();
    }

    private void PatrolMovement()
    {
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                Flip();
            }
        }
        this.transform.rotation = Quaternion.identity;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Si el contacto es en la parte superior del enemigo
                if (contact.normal.y > 0.5f)
                {
                    Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
                        playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                    }
                    Destroy(gameObject);
                    return;
                }
            }
            // Si el contacto es lateral, reiniciar nivel
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
