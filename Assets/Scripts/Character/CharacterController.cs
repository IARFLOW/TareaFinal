using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    private Animator heroAnimator;
    private Rigidbody2D rb;
    public float speed = 10f;
    public float jumpForce = 6f;
    private Vector2 direction;
    private int jumpCount = 0;
    private bool levelCompleted = false;
    public float fallThreshold = -10f;

    void Start()
    {
        heroAnimator = GetComponent<Animator>();
        heroAnimator.Play("walk");
        rb = GetComponent<Rigidbody2D>();
        if (tag != "Player")
        {
            tag = "Player";
        }
    }

    void Update()
    {
        if (levelCompleted) return;
        direction = new Vector2();
        if (Input.GetKey(KeyCode.A))
        {
            heroAnimator.Play("walk");
            direction = Vector2.left;
            transform.localScale = new Vector2(-1, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            heroAnimator.Play("walk");
            direction = Vector2.right;
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
        transform.rotation = Quaternion.identity;
        CheckFallOutOfBounds();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (levelCompleted) return;
        if (other.CompareTag("Finish") || other.gameObject.name.ToLower().Contains("door"))
        {
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        if (levelCompleted) return;
        levelCompleted = true;
        StartCoroutine(LoadNextLevelWithDelay(0.5f));
    }

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
            SceneManager.LoadScene(0);
        }
    }

    void CheckFallOutOfBounds()
    {
        if (transform.position.y < fallThreshold)
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        if (levelCompleted) return;
        levelCompleted = true;
        StartCoroutine(RestartLevelWithDelay(0.5f));
    }

    IEnumerator RestartLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}