using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Hacemos el doorController static para asegurarnos que todas las monedas usan la misma instancia
    private static DoorController sharedDoorController;
    private bool hasBeenCollected = false;

    void Start()
    {
        // Buscar el DoorController en la escena solo si aún no lo hemos encontrado
        if (sharedDoorController == null)
        {
            // Buscar primero en el objeto "door" (más específico)
            GameObject doorObject = GameObject.Find("door");
            if (doorObject != null)
            {
                sharedDoorController = doorObject.GetComponent<DoorController>();
                Debug.Log("DoorController encontrado en objeto 'door'");
            }

            // Si no lo encontramos en el objeto door, buscar en toda la escena
            if (sharedDoorController == null)
            {
                sharedDoorController = FindObjectOfType<DoorController>();
                if (sharedDoorController != null)
                {
                    Debug.Log($"DoorController encontrado en objeto '{sharedDoorController.gameObject.name}'");
                }
                else
                {
                    Debug.LogError("No se encontró ningún DoorController en la escena. ¡El sistema de monedas no funcionará!");
                }
            }
        }

        // Asegurarnos de que el collider es un trigger
        Collider2D coinCollider = GetComponent<Collider2D>();
        if (coinCollider != null && !coinCollider.isTrigger)
        {
            coinCollider.isTrigger = true;
            Debug.Log("Collider de moneda configurado como trigger");
        }
    }

    // Usar múltiples métodos de detección para mayor seguridad
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collider2D collision)
    {
        // Solo procesamos la colisión una vez
        if (hasBeenCollected) return;

        // Verificar si el jugador tocó la moneda - comprobando tanto por tag como por nombre
        if (collision.CompareTag("Player") || collision.name.Contains("Player"))
        {
            Debug.Log($"Moneda {gameObject.name} detectó colisión con jugador");
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        // Marcar como recogida para evitar múltiples llamadas
        hasBeenCollected = true;

        // Notificar al DoorController compartido (static)
        if (sharedDoorController != null)
        {
            Debug.Log($"Moneda {gameObject.name} notificando al DoorController en '{sharedDoorController.gameObject.name}'");

            // Llamamos directamente a la función y verificamos su estado
            sharedDoorController.CoinCollected();

            // Verificamos el estado después de la llamada
            Debug.Log($"Después de recoger moneda {gameObject.name}: collectedCoins={sharedDoorController.GetCollectedCoins()}, totalCoins={sharedDoorController.GetTotalCoins()}");
        }
        else
        {
            // Intentar encontrar el DoorController una última vez
            sharedDoorController = FindObjectOfType<DoorController>();
            if (sharedDoorController != null)
            {
                Debug.Log("DoorController encontrado en último momento");
                sharedDoorController.CoinCollected();
            }
            else
            {
                Debug.LogError("No se pudo encontrar DoorController para notificar moneda recogida");
            }
        }

        // Destruir la moneda
        Destroy(gameObject);
    }
}