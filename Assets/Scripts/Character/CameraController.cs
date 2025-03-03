using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Knight").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posicion = player.position;
        posicion.z = transform.position.z;
        transform.position = posicion;

    }
}
