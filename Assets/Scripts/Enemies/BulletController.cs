using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletController : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;

    private Vector3 direction;
    private bool isInitialized = false;

    void Start()
    {
        Destroy(gameObject, lifetime);

        if (!isInitialized)
        {
            Initialize(Vector3.down);
        }
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initialize(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        isInitialized = true;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RestartLevel();
        }

        Destroy(gameObject);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}