using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.Find("Knight");
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 posicion = player.position;
            posicion.z = transform.position.z;
            transform.position = posicion;
        }
    }
}
