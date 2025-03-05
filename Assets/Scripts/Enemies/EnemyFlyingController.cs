using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyFlyingController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public bool isStatic = false;
    public float flySpeed = 1.0f;
    public float amplitude = 1.0f;
    public float frequency = 1.0f;

    public enum FlyingPattern { Sinusoidal, Circular }
    public FlyingPattern patternType = FlyingPattern.Sinusoidal;

    [Header("Configuración de Disparo")]
    public GameObject bulletPrefab;
    public float fireRate = 1.0f;

    [Range(0f, 1f)]
    public float randomFireVariation = 0.5f;
    public float minTimeBetweenShots = 2.0f;

    [Range(0f, 1f)]
    public float fireChance = 0.7f;
    public int numberOfBullets = 3;

    [Range(10, 180)]
    public float spreadAngle = 45.0f;

    private Vector3 startPosition;
    private float timeCounter = 0;
    private float fireTimer = 0;
    private float nextFireTime;

    void Start()
    {
        startPosition = transform.position;
        fireTimer = UnityEngine.Random.Range(0f, minTimeBetweenShots);
        CalculateNextFireTime();
    }

    void Update()
    {
        if (!isStatic)
        {
            MoveEnemy();
        }

        fireTimer += Time.deltaTime;
        if (fireTimer >= nextFireTime && UnityEngine.Random.value <= fireChance)
        {
            FireBullets();
            fireTimer = 0;
            CalculateNextFireTime();
        }
    }

    void CalculateNextFireTime()
    {
        float baseTime = 1f / fireRate;
        float variation = UnityEngine.Random.Range(0f, randomFireVariation * baseTime);
        nextFireTime = baseTime + variation + minTimeBetweenShots;
    }

    void MoveEnemy()
    {
        timeCounter += Time.deltaTime * flySpeed;

        switch (patternType)
        {
            case FlyingPattern.Sinusoidal:
                MoveSinusoidal();
                break;
            case FlyingPattern.Circular:
                MoveCircular();
                break;
        }
    }

    void MoveSinusoidal()
    {
        float horizontalMovement = Mathf.Sin(timeCounter) * amplitude;
        float verticalMovement = Mathf.Cos(timeCounter * frequency) * amplitude * 0.5f;
        transform.position = startPosition + new Vector3(horizontalMovement, verticalMovement, 0);
    }

    void MoveCircular()
    {
        float x = Mathf.Cos(timeCounter) * amplitude;
        float y = Mathf.Sin(timeCounter) * amplitude;
        transform.position = startPosition + new Vector3(x, y, 0);
    }

    void FireBullets()
    {
        if (bulletPrefab == null) return;

        Vector3 baseDirection = Vector3.down;
        float angleStep = numberOfBullets > 1 ? spreadAngle / (numberOfBullets - 1) : 0;
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BulletController bulletController = bullet.GetComponent<BulletController>();

            if (bulletController != null)
            {
                Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * baseDirection;
                bulletController.Initialize(direction);
            }
        }
    }
}