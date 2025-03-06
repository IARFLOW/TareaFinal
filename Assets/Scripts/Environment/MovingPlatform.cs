using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 5f;
    public float distance = 5f;
    public float pauseTime = 2f;
    public enum MovementType { Vertical, Horizontal }
    public MovementType movementType = MovementType.Vertical;

    private Vector3 startPosition;
    private bool movingForward = true;
    private bool isPaused = false;

    void Start()
    {
        startPosition = transform.position;
    }

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
        Vector3 targetPosition;

        if (movementType == MovementType.Vertical)
        {
            targetPosition = movingForward ? startPosition + Vector3.up * distance : startPosition;
        }
        else
        {
            targetPosition = movingForward ? startPosition + Vector3.right * distance : startPosition;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (transform.position == targetPosition)
        {
            StartCoroutine(PausePlatform());
            movingForward = !movingForward;
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
            if (movementType == MovementType.Vertical && !movingForward &&
                collision.transform.position.y < transform.position.y)
            {
                collision.transform.position = new Vector3(
                    collision.transform.position.x,
                    transform.position.y - collision.collider.bounds.size.y,
                    collision.transform.position.z
                );
            }

            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}