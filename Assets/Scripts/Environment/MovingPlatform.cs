using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float height = 5f;
    public float pauseTime = 2f;

    private Vector3 startPosition;
    private bool movingUp = true;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        float step = speed * Time.deltaTime;
        Vector3 targetPosition = movingUp ? startPosition + Vector3.up * height : startPosition;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (transform.position == targetPosition)
        {
            StartCoroutine(PausePlatform());
            movingUp = !movingUp;
        }
    }

    IEnumerator PausePlatform()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        isPaused = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Si el personaje está debajo de la plataforma, lo empujamos hacia abajo
            if (collision.transform.position.y < transform.position.y)
            {
                collision.transform.position = new Vector3(
                    collision.transform.position.x,
                    transform.position.y - collision.collider.bounds.size.y,
                    collision.transform.position.z
                );
            }
        }
    }
}
