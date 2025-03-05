using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Coin : MonoBehaviour
{
    private static DoorController sharedDoorController;
    private bool hasBeenCollected = false;

    void Start()
    {
        if (sharedDoorController == null)
        {
            GameObject doorObject = GameObject.Find("door");
            if (doorObject != null)
            {
                sharedDoorController = doorObject.GetComponent<DoorController>();
            }
            if (sharedDoorController == null)
            {
                sharedDoorController = FindFirstObjectByType<DoorController>();
            }
        }
        Collider2D coinCollider = GetComponent<Collider2D>();
        if (coinCollider != null && !coinCollider.isTrigger)
        {
            coinCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collider2D collision)
    {
        if (hasBeenCollected) return;
        if (collision.CompareTag("Player") || collision.name.Contains("Player"))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        hasBeenCollected = true;
        if (sharedDoorController != null)
        {
            sharedDoorController.CoinCollected();
        }
        else
        {
            sharedDoorController = FindFirstObjectByType<DoorController>();
            if (sharedDoorController != null)
            {
                sharedDoorController.CoinCollected();
            }
        }
        Destroy(gameObject);
    }
}